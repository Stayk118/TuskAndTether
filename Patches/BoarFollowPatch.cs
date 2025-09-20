using HarmonyLib;
using UnityEngine;
using TuskAndTether.Helpers;

namespace TuskAndTether.Patches
{
    [HarmonyPatch(typeof(global::Character), nameof(global::Character.Interact))]
    public static class BoarFollowPatch
    {
        static void Postfix(global::Character __instance, global::Humanoid character)
        {
            if (!TuskAndTether.EnableBoarFollow.Value) return;
            if (__instance == null || character == null) return;

            if (__instance.name.Contains("Boar") && __instance.IsTamed())
            {
                FollowStateManager.ToggleFollow(__instance, character);
            }
        }
    }
}