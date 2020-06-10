//TODO: Search at runtime or create a tool to pop these into a config file..

namespace AOSharp.Core.GameData
{
    public static class FuncOffsets
    {
        public static int GetItemActionInfo = 0x812AF; // Gamecode.dll - 55 8B EC 8B 49 6C 8D 45 08 50 E8 ? ? ? ? 8B 00 5D C2 04 00
        public static int GetMissionList = 0x588D9; // Gamecode.dll - B8 ? ? ? ? E8 ? ? ? ? 51 56 8B F1 83 BE ? ? ? ? ? 75 25 6A 18 E8 ? ? ? ? 59 8B C8 89 4D F0 83 65 FC 00 85 C9 74 08 56 E8 ? ? ? ? EB 02 
        public static int GetWeapon = 0x681D2; // Gamecode.dll - 55 8B EC 8B 45 08 56 8B F1 85 C0 78 05
        public static int IsDynelInWeaponRange = 0x67B21; // Gamecode.dll - 55 8B EC 83 EC 18 33 C0 56 8B F1 39 45 08 75 07 
        public static int IsInRange = 0x68AC9; // Gamecode.dll - 55 8B EC 83 EC 18 56 8B F1 8B 4E 08 E8 ? ? ? ? 8B 48 5C 89 4D E8 8B 40 60 89 45 EC 8D 45 E8 50 FF 15 ? ? ? ? 59 89 45 F8 85 C0 0F 84 ? ? ? ? 
        public static int GetDummyWeapon = 0x68830;  // Gamecode.dll - 55 8B EC 51 56 8D 45 08 50 8B F1 8D 45 FC 50 8D 4E 0C E8 ? ? ? ? 8B 45 FC 3B 46 10 5E 74 05   
        public static int AppendText = 0x9BC98;  // GUI.dll - B8 ? ? ? ? E8 ? ? ? ? 83 EC 70 53 56 57 8B F1 68 ? ? ? ? 8D 4D D8 FF 15 ? ? ? ? 33 DB 89 5D FC 39 5D 0C 74 6B 
        public static int GetOptionWindow = 0xC3388; // GUI.dll - 55 8B EC 51 33 C0 39 41 10 74 10 8B 41 0C 8B 00 8D 4D FC 89 45 FC E8 ? ? ? ?  //POOR SIG!
    }
}
