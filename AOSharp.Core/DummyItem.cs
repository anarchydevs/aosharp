using AOSharp.Common.GameData;
using AOSharp.Common.Helpers;
using AOSharp.Common.Unmanaged.DataTypes;
using AOSharp.Common.Unmanaged.Imports;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using AOSharp.Core.Inventory;
using AOSharp.Core.UI;

namespace AOSharp.Core
{
    public class DummyItem
    {
        public readonly string Name;
        private readonly Identity Identity;
        private readonly IntPtr Pointer;

        public float AttackDelay => GetStat(Stat.AttackDelay) / 100;

        public virtual float AttackRange => GetStat(Stat.AttackRange);
        public virtual CanFlags CanFlags => (CanFlags)GetStat(Stat.Can);


        internal unsafe DummyItem(int lowId, int highId, int ql)
        {
            Identity none = Identity.None;
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (!CreateDummyItemID(lowId, highId, ql, out Identity dummyItemId))
                throw new Exception($"Failed to create dummy item. LowId: {lowId}\tLowId: {highId}\tLowId: {ql}");

            IntPtr pItem = N3EngineClientAnarchy_t.GetItemByTemplate(pEngine, dummyItemId, &none);

            if (pItem == IntPtr.Zero)
                throw new Exception($"DummyItem::DummyItem - Unable to locate item. LowId: {lowId}\tLowId: {highId}\tLowId: {ql}");

            Pointer = pItem;
            Identity = (*(MemStruct*)pItem).Identity;
            Name = Utils.UnsafePointerToString((*(MemStruct*)pItem).Name);
        }

        internal unsafe DummyItem(Identity identity)
        {
            Identity none = Identity.None;
            IntPtr pEngine = N3Engine_t.GetInstance();
            IntPtr pItem = N3EngineClientAnarchy_t.GetItemByTemplate(pEngine, identity, &none);

            if (pItem == IntPtr.Zero)
                throw new Exception($"DummyItem::DummyItem - Unable to locate item {identity}");

            Pointer = pItem;
            Identity = (*(MemStruct*)pItem).Identity;
            Name = Utils.UnsafePointerToString((*(MemStruct*)pItem).Name);
        }

        public static unsafe bool CreateDummyItemID(int lowId, int highId, int ql, out Identity dummyItemId)
        {
            ACGItem itemInfo = new ACGItem
            {
                LowId = lowId,
                HighId = highId,
                QL = ql
            };

            IntPtr pEngine = N3Engine_t.GetInstance();

            fixed (Identity* pDummyItemId = &dummyItemId)
            {
                return N3EngineClientAnarchy_t.CreateDummyItemID(pEngine, pDummyItemId, &itemInfo);
            }
        }

        //TODO: Make this ignore target checks
        public bool MeetsSelfUseReqs()
        {
            return MeetsUseReqs();
        }

        public unsafe bool MeetsUseReqs(SimpleChar target = null)
        {
            IntPtr pEngine;
            if ((pEngine = N3Engine_t.GetInstance()) == IntPtr.Zero)
                return false;

            IntPtr pCriteria = N3EngineClientAnarchy_t.GetItemActionInfo(Pointer, ItemActionInfo.UseCriteria);

            //Should I return true or false here? hmm.
            if (pCriteria == IntPtr.Zero)
                return true;

            bool[] unk = new bool[12];
            byte prevReqsMet = 0;
            SimpleChar skillCheckChar = DynelManager.LocalPlayer;
            CriteriaSource criteriaSource = CriteriaSource.Self;

            //Default the end result to true
            unk[0] = true;

            foreach (IntPtr pReq in ((StdStructVector*)(pCriteria + 0x4))->ToList(0xC))
            {
                int param1 = *(int*)(pReq);
                int param2 = *(int*)(pReq + 0x4);
                UseCriteriaOperator op = *(UseCriteriaOperator*)(pReq + 0x8);

                bool metReq = false;

                if (op == UseCriteriaOperator.OnUser)
                {
                    criteriaSource = CriteriaSource.User;
                    skillCheckChar = DynelManager.LocalPlayer;
                    continue;
                }
                else if (op == UseCriteriaOperator.OnTarget)
                {
                    criteriaSource = CriteriaSource.Target;
                    skillCheckChar = target;
                    continue;
                }

                if (target != null || criteriaSource != CriteriaSource.Target)
                {
                    bool result;
                    bool lastResult;
                    switch (op)
                    {
                        case UseCriteriaOperator.EqualTo:
                        case UseCriteriaOperator.LessThan:
                        case UseCriteriaOperator.GreaterThan:
                        case UseCriteriaOperator.BitAnd:
                        case UseCriteriaOperator.NotBitAnd:
                            if ((Stat)param1 == Stat.TargetFacing)
                            {
                                SimpleChar fightingTarget;
                                if ((fightingTarget = DynelManager.LocalPlayer.FightingTarget) != null)
                                {
                                    bool isFacing = fightingTarget.IsFacing(DynelManager.LocalPlayer);
                                    metReq = (param2 == 1) ? !isFacing : isFacing;
                                }
                            }
                            else if ((Stat)param1 == Stat.MonsterData) // Ignore this check because something funky is going on with it.
                            {
                                metReq = true;
                            }
                            else
                            {
                                int stat = skillCheckChar.GetStat((Stat)param1);

                                switch (op)
                                {
                                    case UseCriteriaOperator.EqualTo:
                                        metReq = (stat == param2);
                                        break;
                                    case UseCriteriaOperator.LessThan:
                                        metReq = (stat < param2);
                                        break;
                                    case UseCriteriaOperator.GreaterThan:
                                        metReq = (stat > param2);
                                        break;
                                    case UseCriteriaOperator.BitAnd:
                                        metReq = (stat & param2) == param2;
                                        break;
                                    case UseCriteriaOperator.NotBitAnd:
                                        metReq = (stat & param2) != param2;
                                        break;
                                    default:
                                        //Chat.WriteLine($"Unknown Criteria -- Param1: {param1} - Param2: {param2} - Op: {op}");
                                        break;
                                }
                            }
                            break;
                        case UseCriteriaOperator.And:
                            if (prevReqsMet < 2)
                                return false;

                            lastResult = unk[--prevReqsMet];
                            result = unk[--prevReqsMet];

                            //We can early exit on AND 
                            if (!result || !lastResult)
                                return false;

                            metReq = true;
                            break;
                        case UseCriteriaOperator.Or:
                            if (prevReqsMet < 2)
                                return false;

                            lastResult = unk[--prevReqsMet];
                            result = unk[--prevReqsMet];

                            metReq = result || lastResult;
                            break;
                        case UseCriteriaOperator.Not:
                            if (prevReqsMet < 1)
                                return false;

                            metReq = !unk[--prevReqsMet];
                            break;
                        case UseCriteriaOperator.HasWornItem:
                            metReq = Inventory.Inventory.Find(param2, out Item item) &&
                                        (item.Slot.Type == IdentityType.ArmorPage ||
                                        item.Slot.Type == IdentityType.ImplantPage ||
                                        item.Slot.Type == IdentityType.WeaponPage);
                            break;
                        case UseCriteriaOperator.IsNpc:
                            if (param2 == 3)
                                metReq = target.IsNpc;
                            break;
                        case UseCriteriaOperator.HasRunningNano:
                            metReq = skillCheckChar.Buffs.Any(x => x.Identity.Instance == param2);
                            break;
                        case UseCriteriaOperator.HasNotRunningNano:
                            metReq = skillCheckChar.Buffs.All(x => x.Identity.Instance != param2);
                            break;
                        case UseCriteriaOperator.HasPerk:
                            metReq = N3EngineClientAnarchy_t.HasPerk(pEngine, param2);
                            break;
                        case UseCriteriaOperator.IsPerkUnlocked:
                            metReq = true;
                            break;
                        case UseCriteriaOperator.HasRunningNanoLine:
                            metReq = skillCheckChar.Buffs.Contains((NanoLine)param2);
                            break;
                        case UseCriteriaOperator.HasNotRunningNanoLine:
                            metReq = !skillCheckChar.Buffs.Contains((NanoLine) param2);
                            break;
                        case UseCriteriaOperator.HasNcuFor:
                            //TODO: check against actual nano program NCU cost
                            metReq = skillCheckChar.GetStat(Stat.MaxNCU) - skillCheckChar.GetStat(Stat.CurrentNCU) > 0;
                            break;
                        case UseCriteriaOperator.TestNumPets:
                            Pet[] pets = DynelManager.LocalPlayer.Pets;
                            if (pets.Any(x => x.Type == PetType.Unknown))
                            {
                                metReq = false;
                                break;
                            }

                            PetType type = PetType.Unknown;
                            if (param2 == 1)
                                type = PetType.Attack;
                            else if (param2 == 1001)
                                type = PetType.Heal;
                            else if (param2 == 2001)
                                type = PetType.Support;
                            else if (param2 == 4001)
                                type = PetType.Social;

                            metReq = !pets.Any(x => x.Type == type);
                            break;
                        case UseCriteriaOperator.HasWieldedItem:
                            if (criteriaSource == CriteriaSource.Target)
                            {
                                metReq = true;
                            }
                            else
                            {
                                metReq = Inventory.Inventory.Items.Any(i =>
                                    (i.LowId == param2 || i.HighId == param2) &&
                                    (i.Slot.Instance >= (int)EquipSlot.Weap_Hud1 &&
                                        i.Slot.Instance <= (int)EquipSlot.Imp_Feet));
                            }
                            break;
                        case UseCriteriaOperator.IsSameAs:
                            //Not sure what these parmas correlate to but I don't know any other item that uses this operator either.
                            if (param1 == 1 && param2 == 3)
                            {
                                if (target == null)
                                    metReq = false;
                                else
                                    metReq = target.Identity == DynelManager.LocalPlayer.Identity;
                            }
                            break;
                        case UseCriteriaOperator.AlliesNotInCombat:
                            if (Team.Members.Contains(skillCheckChar.Identity))
                            {
                                Identity[] teamMembers = Team.Members.Select(x => x.Identity).ToArray();
                                metReq = !DynelManager.Characters.Any(x => x.FightingTarget != null && teamMembers.Contains(x.FightingTarget.Identity));
                            }
                            else
                            {
                                metReq = !DynelManager.Characters.Any(x => x.FightingTarget != null && (x.FightingTarget.Identity == skillCheckChar.Identity || x.FightingTarget.Identity == DynelManager.LocalPlayer.Identity));
                            }

                            break;
                        case UseCriteriaOperator.IsOwnPet:
                            metReq = DynelManager.LocalPlayer.Pets.Contains(skillCheckChar.Identity);

                            break;
                        default:
                            //Chat.WriteLine($"Unknown Criteria -- Param1: {param1} - Param2: {param2} - Op: {op}");
                            return false;
                    }
                }
                else
                {
                    metReq = true;
                }

                unk[prevReqsMet++] = metReq;

                //Chat.WriteLine($"Name: {Name} -- Unk -- Param1: {param1} - Param2: {param2} - Op: {op} MetReq: {metReq} ------ {prevReqsMet} -- {unk[0]} {unk[1]} {unk[2]} {unk[3]}");
            }

            return unk[0];
        }

        public int GetStat(Stat stat, int detail = 2)
        {
            return DummyItem_t.GetStat(Pointer, stat, detail);
        }

        public virtual bool IsInRange(SimpleChar target)
        {
            return DynelManager.LocalPlayer.GetLogicalRangeToTarget(target) < AttackRange;
        }

        internal IntPtr GetItemActionInfo(ItemActionInfo itemActionInfo) => N3EngineClientAnarchy_t.GetItemActionInfo(Pointer, itemActionInfo);

        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        private struct MemStruct
        {
            [FieldOffset(0x14)]
            public Identity Identity;

            [FieldOffset(0x9C)]
            public IntPtr Name;
        }

        private enum CriteriaSource
        {
            Target,
            Self,
            User
        }
    }
}
