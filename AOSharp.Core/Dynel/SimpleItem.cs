using System;
using System.Runtime.InteropServices;
using AOSharp.Core.GameData;
using AOSharp.Common.GameData;
using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

namespace AOSharp.Core
{
    public unsafe class SimpleItem : Dynel
    {

        public SimpleItem(IntPtr pointer) : base(pointer)
        {
        }

        public SimpleItem(Dynel dynel) : base(dynel.Pointer)
        {
        }

        public void Use()
        {
            Network.Send(new GenericCmdMessage()
            {
                Action = GenericCmdAction.Use,
                User = DynelManager.LocalPlayer.Identity,
                Target = Identity
            });
        }
    }
}
