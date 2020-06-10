using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using AOSharp.Common.GameData;
using AOSharp.Core.Imports;
using AOSharp.Core.GameData;
using System.CodeDom;

namespace AOSharp.Core
{
    public class DummyItem
    {
        public readonly string Name;
        private readonly Identity Identity;
        private readonly IntPtr Pointer;

        internal unsafe DummyItem(int lowId, int highId, int ql)
        {
            Identity none = Identity.None;
            Identity dummyItemId;
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (!CreateDummyItemID(lowId, highId, ql, out dummyItemId))
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
                return N3EngineClientAnarchy_t.CreateDummyItemID(pEngine, pDummyItemId, &itemInfo) == 1;
            }
        }

        public bool MeetsUseReqs()
        {
            return MeetsUseReqs(DynelManager.LocalPlayer);
        }

        public unsafe bool MeetsUseReqs(SimpleChar target)
        {
            IntPtr pCriteria = N3EngineClientAnarchy_t.GetItemActionInfo(Pointer, ItemActionInfo.UseCriteria);

            //Should I return true or false here? hmm.
            if (pCriteria == IntPtr.Zero)
                return true;

            bool[] unk = new bool[4];
            byte prevReqsMet = 0;
            SimpleChar skillCheckChar = DynelManager.LocalPlayer;

            //Default the end result to true
            unk[0] = true;

            foreach (IntPtr pReq in ((StdStructVector*)(pCriteria + 0x4))->ToList(0xC))
            {
                int param1 = *(int*)(pReq);
                int param2 = *(int*)(pReq + 0x4);
                int op = *(int*)(pReq + 0x8);
                bool metReq = false;
                bool result;
                bool lastResult;

                if (param1 == 0)
                {
                    switch((ItemCriteriaOperator)op)
                    {
                        case ItemCriteriaOperator.And:
                            if (prevReqsMet < 2)
                                return false;

                            lastResult = unk[(prevReqsMet--) - 1];
                            result = unk[(prevReqsMet--) - 1];

                            //We can early exit on AND 
                            if (!result || !lastResult)
                                return false;

                            metReq = true;
                            break;
                        case ItemCriteriaOperator.Or:
                            if (prevReqsMet < 2)
                                return false;

                            lastResult = unk[(prevReqsMet--) - 1];
                            result = unk[(prevReqsMet--) - 1];

                            metReq = result || lastResult;
                            break;
                        case ItemCriteriaOperator.Not:
                            if (prevReqsMet < 1)
                                return false;

                            metReq = unk[(prevReqsMet--) - 1];
                            break;
                        case ItemCriteriaOperator.OnTarget:
                            skillCheckChar = target;
                            continue;
                        case ItemCriteriaOperator.HasWornItem:
                            metReq = false;
                            break;
                        case ItemCriteriaOperator.IsNpc:
                            if(param2 == 3)
                                metReq = target.IsNPC;
                            break;
                        default:
                            //Chat.WriteLine($"Unknown Criteria -- Param1: {param1} - Param2: {param2} - Op: {op}");
                            return false;
                    }
                } 
                else
                {
                    int stat = skillCheckChar.GetStat((Stat)param1);

                    switch((ItemCriteriaOperator)op)
                    {
                        case ItemCriteriaOperator.EqualTo:
                            metReq = (stat == param2);
                            break;
                        case ItemCriteriaOperator.LessThan:
                            metReq = (stat < param2);
                            break;
                        case ItemCriteriaOperator.GreaterThan:
                            metReq = (stat > param2);
                            break;
                        case ItemCriteriaOperator.BitAnd:
                            metReq = (stat & param2) == param2;
                            break;
                        default:
                            //Chat.WriteLine($"Unknown Criteria -- Param1: {param1} - Param2: {param2} - Op: {op}");
                            break;
                    }
                }

                unk[prevReqsMet++] = metReq;

                //Chat.WriteLine($"Unk -- Param1: {param1} - Param2: {param2} - Op: {op} MetReq: {metReq} ------ {prevReqsMet} -- {unk[0]} {unk[1]} {unk[2]} {unk[3]}");
            }

            return unk[0];
        }

        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        private struct MemStruct
        {
            [FieldOffset(0x14)]
            public Identity Identity;

            [FieldOffset(0x9C)]
            public IntPtr Name;
        }
    }
}
