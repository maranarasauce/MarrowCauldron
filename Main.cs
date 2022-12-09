using JetBrains.Annotations;
using MelonLoader;
using SLZ.Rig;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace MarrowCauldron
{
    public class MarrowCauldron : MelonMod
    {
        public override void OnInitializeMelon()
        {
            base.OnInitializeMelon();
            HarmonyInstance.PatchAll(typeof(MarrowCauldron));

            SaveGamePath();
            InjectElixirs();
#if DEBUG
            #region AntiAntiHeadset
            HarmonyInstance.Patch(typeof(ControllerRig).GetProperty("IsPaused").GetSetMethod(), typeof(MarrowCauldron).GetMethod(nameof(DontRunMe)).ToNewHarmonyMethod());
            HarmonyInstance.Patch(typeof(Control_GlobalTime).GetMethod("PAUSE"), typeof(MarrowCauldron).GetMethod(nameof(DontRunMe)).ToNewHarmonyMethod());
            #endregion
#endif
        }

        private void SaveGamePath()
        {
            string savePath = Path.Combine(Application.persistentDataPath, "cauldronsave.txt");
            string melonPath = MelonUtils.MelonLoaderDirectory;

            if (!File.Exists(savePath))
            {
                File.Create(savePath).Dispose();
                WriteGamePath(savePath, melonPath);
            }
            else if (File.ReadAllText(savePath) != melonPath)
            {
                WriteGamePath(savePath, melonPath);
            }
        }

        private void WriteGamePath(string savePath, string melonPath)
        {
            var fs = new FileStream(savePath, FileMode.Create);
            fs.Dispose();

            StreamWriter writer = new StreamWriter(savePath, true);
            writer.WriteLine(melonPath);
            writer.Close();
        }

        private void InjectElixirs()
        {
            foreach (string pallet in Utilities.GetPallets())
            {
                string flaskPath = Path.Combine(pallet, "flasks");

                if (!Directory.Exists(flaskPath))
                    continue;

                string[] flasks = Directory.GetFiles(flaskPath, "*.dll");
                foreach (string flask in flasks)
                {
                    MelonLogger.Msg($"Loading Flask: \"{flask}\"");

                    Assembly assembly = Assembly.LoadFile(flask);

                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (typeof(MonoBehaviour).IsAssignableFrom(type))
                            FieldInjector.SerialisationHandler.Inject(type, 1);
                    }
                }
            }
        }

        public static bool DontRunMe() => false;
    }
}