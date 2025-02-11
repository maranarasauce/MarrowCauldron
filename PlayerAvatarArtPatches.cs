using HarmonyLib;
using Il2CppSLZ.VRMK;
using UnityEngine;
using Avatar = Il2CppSLZ.VRMK.Avatar;

internal static class PlayerAvatarArtPatches
{
    [HarmonyPatch(typeof(Avatar), nameof(Avatar.Awake))]
    [HarmonyPrefix]
    public static void Prefix(Avatar __instance)
    {
        avatar = __instance;
    }
    public static Avatar avatar;
}

[HarmonyLib.HarmonyPatch(typeof(Il2CppSLZ.Marrow.PlayerAvatarArt), nameof(Il2CppSLZ.Marrow.PlayerAvatarArt.UpdateAvatarHead))]
internal static class UpdateAvatarHead
{
    internal static Vector3 preTransformHead;
    internal static Vector3 postTransformHead;

    internal static Vector3 calculatedHeadOffset;


    internal static void Prefix(Il2CppSLZ.Marrow.PlayerAvatarArt __instance)
    {
        Transform head = PlayerAvatarArtPatches.avatar.animator.GetBoneTransform(HumanBodyBones.Head);
        preTransformHead = head.position;
    }

    internal static void Postfix(Il2CppSLZ.Marrow.PlayerAvatarArt __instance)
    {
        Transform head = PlayerAvatarArtPatches.avatar.animator.GetBoneTransform(HumanBodyBones.Head);
        postTransformHead = head.position;

        calculatedHeadOffset = preTransformHead - postTransformHead;

        head.position += calculatedHeadOffset;
    }
}