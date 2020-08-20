using AOSharp.Common.GameData;
using AOSharp.Common.Unmanaged.DataTypes;
using AOSharp.Core.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace AOSharp.Core
{
    //AVNpcHolder_t
    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public unsafe struct NpcHolder
    {
        [FieldOffset(0x1C)]
        public IntPtr pPetUnk;

        internal Pet[] GetPets()
        {
            List<Pet> petIdentities = new List<Pet>();

            if (pPetUnk == IntPtr.Zero)
                return petIdentities.ToArray();

            foreach (IntPtr pPet in ((StdObjList*)(pPetUnk + 0x4))->ToList())
            {
                PetEntry petEntry = (*(PetEntry*)pPet);

                petIdentities.Add(new Pet(petEntry.Identity));
            }

            return petIdentities.ToArray();
        }

        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        private struct PetEntry
        {
            [FieldOffset(0x0C)]
            public Identity Identity;
        }
    }
}
