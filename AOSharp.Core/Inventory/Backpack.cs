using AOSharp.Common.GameData;
using AOSharp.Common.Unmanaged.Interfaces;

namespace AOSharp.Core.Inventory
{
    public class Backpack : Container
    {
        public string Name => InventoryGUIModule.GetBackpackName(Identity);

        internal Backpack(Identity identity, Identity slot) : base(identity, slot)
        {
        }

        public void SetName(string name)
        {
            InventoryGUIModule.SetBackpackName(Identity, name);
        }
    }
}
