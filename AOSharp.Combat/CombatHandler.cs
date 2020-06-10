using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Common.GameData;
using AOSharp.Core;

namespace AOSharp.Combat
{
    public class CombatHandler
    {
        private Identity _target = Identity.None;

        public CombatHandler()
        {
            Game.OnUpdate += OnUpdate;
        }

        protected virtual void OnUpdate(object s, float deltaTime)
        {
            if (_target == Identity.None)
                return;
        }

        protected virtual void SelfItemTick()
        {

        }

        protected virtual void SelfSpellTick()
        {

        }

        protected virtual void SelfPerkTick()
        {

        }

        protected virtual void TargetItemTick(SimpleChar target)
        {

        }

        protected virtual void TargetSpellTick(SimpleChar target)
        {

        }

        protected virtual void TargetPerkTick(SimpleChar target)
        {

        }
    }
}
