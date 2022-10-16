using System.IO;
using UnityEngine;

namespace PalletFlasks
{
    public static class Utilities
    {
        public static string[] GetPallets()
        {
            string modPath = Path.Combine(Application.persistentDataPath, "Mods");
            return Directory.GetDirectories(modPath);
        }
    }
}
