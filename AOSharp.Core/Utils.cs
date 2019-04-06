namespace AOSharp.Core
{
    public static class Utils
    {
        public unsafe static string N3MessageKeyToString(int key)
        {
            return (*N3InfoItemRemote_t.KeyToString(key)).ToString();
        }
    }
}
