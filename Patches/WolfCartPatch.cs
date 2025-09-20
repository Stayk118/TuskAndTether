using HarmonyLib;
using UnityEngine;
using TuskAndTether.Helpers;

namespace TuskAndTether.Patches
{
    [HarmonyPatch(typeof(Character), "Interact")]
    public static class WolfCartPatch
    {
        static void Postfix(Character __instance, Humanoid character)
        {
            if (!TuskAndTether.EnableWolfCart.Value) return;
            if (__instance == null || character == null) return;

            // Check if it's a tamed wolf
            if (__instance.name.Contains("Wolf") && __instance.IsTamed())
            {
                FollowStateManager.ToggleFollow(__instance, character);

                // Check if player has a cart attached
                var cart = GetAttachedCart(character);
                if (cart != null)
                {
                    TransferCartToWolf(__instance, cart, character);
                    Debug.Log($"[TuskAndTether] Cart transferred from player to wolf: {__instance.name}");
                }
            }
        }

        private static GameObject GetAttachedCart(Humanoid player)
        {
            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            if (playerRb == null) return null;

            foreach (var joint in player.GetComponents<ConfigurableJoint>())
            {
                if (joint.connectedBody != null && joint.connectedBody.gameObject.name.Contains("Cart"))
                {
                    return joint.connectedBody.gameObject;
                }
            }

            return null;
        }

        private static void TransferCartToWolf(Character wolf, GameObject cart, Humanoid player)
        {
            Rigidbody cartRb = cart.GetComponent<Rigidbody>();
            if (cartRb == null) return;

            // Remove existing joints
            foreach (var joint in cart.GetComponents<ConfigurableJoint>())
            {
                Object.Destroy(joint);
            }

            // Create new joint to wolf
            ConfigurableJoint joint = cart.AddComponent<ConfigurableJoint>();
            joint.connectedBody = wolf.GetComponent<Rigidbody>();
            joint.xMotion = ConfigurableJointMotion.Limited;
            joint.zMotion = ConfigurableJointMotion.Limited;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            joint.angularZMotion = ConfigurableJointMotion.Locked;

            joint.anchor = new Vector3(0, 0.5f, -1f);
            joint.autoConfigureConnectedAnchor = true;

            // Ensure wolf follows player
            MonsterAI ai = wolf.GetComponent<MonsterAI>();
            if (ai != null)
            {
                ai.m_followTarget = player.transform;
                ai.m_alerted = true;
                ai.m_aggressive = false;
                ai.m_passiveAggressive = false;
            }

            // Update follow state
            FollowStateManager.SetFollowState(wolf, true);
        }
    }
}