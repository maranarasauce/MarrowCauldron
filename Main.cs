﻿using Il2CppSLZ.Serialize;
using MelonLoader.Utils;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppSLZ.Marrow.Warehouse;
using Il2CppNewtonsoft.Json.Linq;
using Il2CppSystem.Linq;
using Il2CppSLZ.Rig;
using System.Net;
using Il2CppSLZ.Marrow;
using Il2CppSLZ.VRMK;
using System.Runtime.CompilerServices;
using Avatar = Il2CppSLZ.VRMK.Avatar;
using static PlayerAvatarArtPatches;

namespace MarrowCauldron;

public class Main : MelonMod
{
    internal const string Name = "MarrowCauldron";
    internal const string Description = "Injector for Flasks";
    internal const string Author = "Maranara";
    internal const string Company = "Maranara";
    internal const string Version = "2.0.0";
    internal const string DownloadLink = "https://thunderstore.io/c/bonelab/p/Author/BONELABTemplate/";
    
    public override void OnInitializeMelon()
    {
        HarmonyInstance.PatchAll(typeof(Main));
        //HarmonyInstance.PatchAll(typeof(UpdateAvatarHead));

        SaveGamePath();
        InjectElixirs();
    }


    public override void OnUpdate()
    {
        
        base.OnUpdate();
        
    }

    private void SaveGamePath()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "cauldronsave.txt");
        string melonPath = MelonEnvironment.MelonLoaderDirectory;

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
                    try
                    {
                        if (typeof(MonoBehaviour).IsAssignableFrom(type) || typeof(ScriptableObject).IsAssignableFrom(type))
                            FieldInjector.SerialisationHandler.Inject(type, 0);
                    }
                    catch
                    {
                        MelonLogger.Msg($"Got error for pouring of Elixir: \"{type.FullName}\"");
                    }
                }
            }
        }
    }

    private const string FLASK_KEY = "Flask";

    [HarmonyPatch(typeof(Il2CppSystem.Type), "GetType", new Type[] { typeof(string)})]
    [HarmonyPrefix]
    public static bool GetType(ref Il2CppSystem.Type __result, string typeName)
    {
        if (typeName.Contains(FLASK_KEY))
        {
            __result = Il2CppType.From(Type.GetType(typeName));
            return false;
        }
        return true;
    }

    //[HarmonyPatch(typeof(ObjectStore), "TryResolveTypeId")]
    //[HarmonyPrefix]
    public static void TryResolveTypeId(ObjectStore __instance)
    {
        foreach (var ins  in __instance._types)
        {
            MelonLogger.Msg($"[ID] {ins.key.FullName}, {ins.value.ToString()}");
        }
    }

    [HarmonyPatch(typeof(Avatar), nameof(Avatar.Awake))]
    [HarmonyPrefix]
    public static void Prefix(Avatar __instance)
    {
        avatar = __instance;
    }
    public static Avatar avatar;

    [HarmonyPatch(typeof(Pallet), nameof(Pallet.Unpack))]
    [HarmonyPrefix]
    public static void RemoveFlaskReference(Pallet __instance, ref ObjectStore store, string objectId)
    {
        JToken dataCards = store._jsonDocument["objects"]["1"]["dataCards"];

        for (int i = 0; i < dataCards.Values<JToken>().Count(); i++)
        {
            var card = dataCards[i];
            string parse = card["type"].ToString();
            if (parse == "1")
            {
                card.Remove();
            }
        }
    }
}