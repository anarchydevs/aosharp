using AOSharp.Common.GameData;
using AOSharp.Common.Helpers;
using AOSharp.Common.Unmanaged.DataTypes;
using AOSharp.Common.Unmanaged.Imports;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using AOSharp.Core.Inventory;
using AOSharp.Core.UI;
using System.Collections.Generic;

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

            IntPtr pItem = N3EngineClientAnarchy_t.GetItemByTemplate(pEngine, dummyItemId, ref none);

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
            IntPtr pItem = N3EngineClientAnarchy_t.GetItemByTemplate(pEngine, identity, ref none);

            if (pItem == IntPtr.Zero)
                throw new Exception($"DummyItem::DummyItem - Unable to locate item {identity}");

            Pointer = pItem;
            Identity = (*(MemStruct*)pItem).Identity;
            Name = Utils.UnsafePointerToString((*(MemStruct*)pItem).Name);
        }

        public static bool CreateDummyItemID(int lowId, int highId, int ql, out Identity dummyItemId)
        {
            ACGItem itemInfo = new ACGItem
            {
                LowId = lowId,
                HighId = highId,
                QL = ql
            };

            IntPtr pEngine = N3Engine_t.GetInstance();

            Identity templateId = Identity.None;
            bool result =  N3EngineClientAnarchy_t.CreateDummyItemID(pEngine, ref templateId, ref itemInfo);
            dummyItemId = templateId;
            return result;
        }

        public bool MeetsSelfUseReqs()
        {
            return MeetsUseReqs(ignoreTargetReqs: true);
        }

        public unsafe bool MeetsUseReqs(SimpleChar target = null, bool ignoreTargetReqs = false)
        {
            IntPtr pEngine;
            if ((pEngine = N3Engine_t.GetInstance()) == IntPtr.Zero)
                return false;

            IntPtr pCriteria = N3EngineClientAnarchy_t.GetItemActionInfo(Pointer, ItemActionInfo.UseCriteria);

            //Should I return true or false here? hmm.
            if (pCriteria == IntPtr.Zero)
                return true;

            List<RequirementCriterion> criteria = new List<RequirementCriterion>();

            foreach (IntPtr pReq in ((StdStructVector*)(pCriteria + 0x4))->ToList(0xC))
            {
                criteria.Add(new RequirementCriterion
                {
                    Param1 = *(int*)(pReq),
                    Param2 = *(int*)(pReq + 0x4),
                    Operator = *(UseCriteriaOperator*)(pReq + 0x8)
                });
            }

            //foreach(var req in criteria)
            //    Chat.WriteLine($"Param1: {req.Param1}, Param2: {req.Param2}, Op: {req.Operator}");

            ReqChecker reqChecker = new ReqChecker(criteria);

            return reqChecker.MeetsReqs(target, ignoreTargetReqs);
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
            FightingTarget,
            Target,
            Self,
            User
        }
    }
}
