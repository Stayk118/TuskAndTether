using HarmonyLib;
using UnityEngine;
using TuskAndTether.Helpers;

namespace TuskAndTether.Patches
{
    [HarmonyPatch(typeof(Character), "Interact")]
    public static class LoxFollowPatch
    {
        static void Postfix(Character __instance, Humanoid character)
        {
            if (!TuskAndTether.EnableLoxFollow.Value) return;
            if (__instance == null || character == null) return;

            // Check if it's a tamed lox
            if (__instance.name.Contains("Lox") && __instance.IsTamed())
            {
                FollowStateManager.ToggleFollow(__instance, character);
            }
        }
    }
}