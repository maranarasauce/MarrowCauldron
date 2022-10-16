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
                string flaskPath = Path.Combine(pallet, "flasks");

                if (!Directory.Exists(flaskPath))
                    continue;

                string[] flasks = Directory.GetFiles(flaskPath);
                foreach (string flask in flasks)
                {
                    MelonLogger.Msg($"Loading Flask: \"{flask}\"");

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

        public static bool DontRunMe() => false;
    }
}