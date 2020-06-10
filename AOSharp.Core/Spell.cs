using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using AOSharp.Common.GameData;
using AOSharp.Core.Imports;
using AOSharp.Core.Combat;
using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;
using AOSharp.Core.GameData;

namespace AOSharp.Core
{
    public class Spell : DummyItem, ICombatAction
    {
        public readonly Identity Identity;
        public bool IsReady => GetIsReady();

        public static IEnumerable<Spell> List => GetSpellList();

        internal unsafe Spell(Identity identity) : base(identity)
        {
            Identity = identity;
        }

        public static bool Find(int id, out Spell spell)
        {
            return (spell = List.FirstOrDefault(x => x.Identity.Instance == id)) != null;
        }

        public static bool Find(string name, out Spell spell)
        {
            return (spell = List.FirstOrDefault(x => x.Name == name)) != null;
        }

        public void Cast()
        {
            Cast(DynelManager.LocalPlayer);
        }

        public void Cast(SimpleChar target)
        {
            Connection.Send(new CharacterActionMessage()
            {
                Action = CharacterActionType.CastNano,
                Target = target.Identity,
                Parameter1 = (int)Identity.Type,
                Parameter2 = Identity.Instance
            });
        }

        private unsafe bool GetIsReady()
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return false;

            Identity identity = Identity;
            return N3EngineClientAnarchy_t.IsFormulaReady(pEngine, &identity) == 1;
        }

        private unsafe static IEnumerable<Spell> GetSpellList()
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return new List<Spell>();

            return N3EngineClientAnarchy_t.GetNanoSpellList(pEngine)->ToList().Select(x => new Spell(new Identity(IdentityType.NanoProgram, (*(MemStruct*)x).Id)));
        }

        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        private struct MemStruct
        {
            [FieldOffset(0x08)]
            public int Id;
        }
    }
}
