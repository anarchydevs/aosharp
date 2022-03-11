using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AOSharp.Common.GameData;
using AOSharp.Common.Unmanaged.Imports;
using AOSharp.Core.Combat;
using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;
using AOSharp.Core.GameData;
using SmokeLounge.AOtomation.Messaging.GameData;
using AOSharp.Core.UI;

namespace AOSharp.Core
{
    public class Spell : DummyItem, ICombatAction
    {
        private const float CAST_TIMEOUT = 0.5f;

        public readonly Identity Identity;
        public NanoLine Nanoline => (NanoLine)GetStat(Stat.NanoStrain);
        public int NCU => GetStat(Stat.Level);
        public int StackingOrder => GetStat(Stat.StackingOrder);
        public NanoSchool NanoSchool => (NanoSchool)GetStat(Stat.School);

        public int Cost => GetCost();

        public override float AttackRange => Math.Min(base.AttackRange * (1 + DynelManager.LocalPlayer.GetStat(Stat.NanoRange) / 100f), 40f);
        public bool IsReady => GetIsReady();

        public static IEnumerable<Spell> List => GetSpellList();
        public static bool HasPendingCast => _pendingCast.Spell != null;
        private static (Spell Spell, double Timeout) _pendingCast;

        internal Spell(Identity identity) : base(identity)
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
        public void Cast(bool setTarget = false)
        {
            Cast(DynelManager.LocalPlayer, setTarget);
        }

        public void Cast(SimpleChar target, bool setTarget = false)
        {
            if (target == null)
                target = DynelManager.LocalPlayer;

            if(setTarget)
                target.Target();

            Network.Send(new CharacterActionMessage()
            {
                Action = CharacterActionType.CastNano,
                Target = target.Identity,
                Parameter1 = (int)Identity.Type,
                Parameter2 = Identity.Instance
            });

            _pendingCast = (this, Time.NormalTime + CAST_TIMEOUT);
        }

        private unsafe bool GetIsReady()
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return false;

            Identity identity = Identity;
            return N3EngineClientAnarchy_t.IsFormulaReady(pEngine, ref identity);
        }

        internal static void Update()
        {
            try
            {
            if (_pendingCast.Spell != null && _pendingCast.Timeout <= Time.NormalTime)
                _pendingCast.Spell = null;
            }
            catch (Exception e)
            {
                Chat.WriteLine($"This shouldn't happen pls report (Spell): {e.Message}");
            }
        }

        public static Spell[] GetSpellsForNanoline(NanoLine nanoline)
        {
            return List.Where(x => x.Nanoline == nanoline).ToArray();
        }

        private static unsafe Spell[] GetSpellList()
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return new Spell[0];

            return N3EngineClientAnarchy_t.GetNanoSpellList(pEngine)->ToList().Select(x => new Spell(new Identity(IdentityType.NanoProgram, (*(MemStruct*)x).Id))).ToArray();
        }

        private int GetCost()
        {
            int costModifier = DynelManager.LocalPlayer.GetStat(Stat.NPCostModifier);
            int baseCost = GetStat(Stat.NanoPoints);

            switch (DynelManager.LocalPlayer.Breed)
            {
                case Breed.Nanomage:
                    costModifier = costModifier < 45 ? 45 : costModifier;
                    break;
                case Breed.Atrox:
                    costModifier = costModifier < 55 ? 55 : costModifier;
                    break;
                case Breed.Solitus:
                case Breed.Opifex:
                default:
                    costModifier = costModifier < 50 ? 50 : costModifier;
                    break;
            }

            return (int)(baseCost * ((double)costModifier / 100));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Spell);
        }

        public bool Equals(Spell other)
        {
            if (object.ReferenceEquals(other, null))
                return false;

            return Identity == other.Identity;
        }

        public override int GetHashCode()
        {
            return 91194611 + Identity.GetHashCode();
        }

        public static bool operator ==(Spell a, Spell b)
        {
            if (object.ReferenceEquals(a, null))
                return object.ReferenceEquals(b, null);

            return a.Equals(b);
        }


        public static bool operator !=(Spell a, Spell b) => !(a == b);

        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        private struct MemStruct
        {
            [FieldOffset(0x08)]
            public int Id;
        }
    }
}
