using HarmonyLib;
using UnityEngine;
using TuskAndTether.Helpers;

namespace TuskAndTether.Patches
{
    [HarmonyPatch(typeof(Character), "UpdateMovement")]
    public static class WolfMovementPatch
    {
        static void Prefix(Character __instance, float dt)
        {
            if (__instance == null || !__instance.IsTamed()) return;
            if (!__instance.name.Contains("Wolf")) return;

            // Check if this wolf has a cart attached
            CartTracker tracker = __instance.GetComponent<CartTracker>();
            if (tracker == null || tracker.AttachedCart == null) return;

            float cartWeight = WeightThresholdHelper.GetCartWeight(tracker.AttachedCart);
            float weightLimit = WeightThresholdHelper.GetWolfWeightLimit(__instance);

            if (cartWeight > weightLimit)
            {
                float excess = cartWeight - weightLimit;
                float slowFactor = Mathf.Clamp01(excess / 300f); // Max slowdown at 300 over limit
                float originalSpeed = __instance.m_speed;
                __instance.m_speed *= (1f - slowFactor * 0.5f); // Up to 50% reduction

                Debug.Log($"[TuskAndTether] Wolf speed reduced due to cart weight ({cartWeight} > {weightLimit}). Speed: {originalSpeed} → {__instance.m_speed}");
            }
        }
    }
}