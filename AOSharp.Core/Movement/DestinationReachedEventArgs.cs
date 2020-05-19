using System;

namespace AOSharp.Core.Movement
{
    public class DestinationReachedEventArgs : EventArgs
    {
        public bool Halt { get; set; }

        public DestinationReachedEventArgs(bool halt = true)
        {
            Halt = halt;
        }
    }
}
