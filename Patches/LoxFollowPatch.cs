using HarmonyLib;
using UnityEngine;
using TuskAndTether.Helpers;

namespace TuskAndTether.Patches
{
    [HarmonyPatch(typeof(global::Character), nameof(global::Character.Interact))]
    public static class LoxFollowPatch
    {
        static void Postfix(global::Character __instance, global::Humanoid character)
        {
            if (!TuskAndTether.EnableLoxFollow.Value) return;
            if (__instance == null || character == null) return;

            if (__instance.name.Contains("Lox") && __instance.IsTamed())
            {
                FollowStateManager.ToggleFollow(__instance, character);
            }
        }
    }
}