using HarmonyLib;
using MelonLoader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SLZ.Marrow.SceneStreaming;
using SLZ.Marrow.Warehouse;
using SLZ.Rig;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace PalletFlasks
{
    public class PalletFlasks : MelonMod
    {
        public override void OnInitializeMelon()
        {
            base.OnInitializeMelon();
            HarmonyInstance.PatchAll(typeof(PalletFlasks));

            InjectElixirs();

            #region AntiAntiHeadset
            HarmonyInstance.Patch(typeof(ControllerRig).GetProperty("IsPaused").GetSetMethod(), typeof(PalletFlasks).GetMethod(nameof(DontRunMe)).ToNewHarmonyMethod());
            HarmonyInstance.Patch(typeof(Control_GlobalTime).GetMethod("PAUSE"), typeof(PalletFlasks).GetMethod(nameof(DontRunMe)).ToNewHarmonyMethod());
            #endregion
        }

        public void InjectElixirs()
        {
            foreach (string pallet in Utilities.GetPallets())
            {
                MelonLogger.Msg($"Loading assemblies for {Path.GetFileName(pallet)}");
                string flaskPath = Path.Combine(pallet, "flasks");

                if (!Directory.Exists(flaskPath))
                    continue;

                string[] flasks = Directory.GetFiles(flaskPath);
                foreach (string flask in flasks)
                {
                    MelonLogger.Msg($"Loading {flask}");

                    Assembly assembly = System.Reflection.Assembly.LoadFile(flask);

                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (typeof(MonoBehaviour).IsAssignableFrom(type))
                            FieldInjector.SerialisationHandler.Inject(type, 1);
                    }
                }
            }
        }


        public void ScanForPallets()
        {
            MelonLogger.Msg("Reading pallets...");
            foreach (string pallet in Utilities.GetPallets())
            {
                MelonLogger.Msg($"Found pallet with path: {pallet}");
                string manifestPath = Path.Combine(pallet, "pallet.json");
                string manifest = File.ReadAllText(manifestPath);
                JObject store = JObject.Parse(manifest);

                if (store.TryGetValue("objects", System.StringComparison.Ordinal, out JToken crates))
                {
                    MelonLogger.Msg("Got objects.");

                    foreach (JToken crate in crates.Children())
                    {
                        foreach (JToken crateInfo in crate)
                        {
                            MelonLogger.Msg($"{crateInfo.Value<string>("barcode")}");
                        }
                    }
                } else
                {
                    MelonLogger.Msg("Did NOT get objects.");
                }

            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);
            MelonLogger.Msg($"Scene loaded {sceneName}!");
        }

        public static bool DontRunMe() => false;
    }
}