using HarmonyLib;
using UnityEngine;
using TuskAndTether.Helpers;

namespace TuskAndTether.Patches
{
    [HarmonyPatch(typeof(global::Character), nameof(global::Character.Interact))]
    public static class WolfCartPatch
    {
        static void Postfix(global::Character __instance, global::Humanoid character)
        {
            if (!TuskAndTether.EnableWolfCart.Value) return;
            if (__instance == null || character == null) return;

            if (__instance.name.Contains("Wolf") && __instance.IsTamed())
            {
                FollowStateManager.ToggleFollow(__instance, character);

                var cart = GetAttachedCart(character);
                if (cart != null)
                {
                    TransferCartToWolf(__instance, cart, character);
                    Debug.Log($"[TuskAndTether] Cart transferred from player to wolf: {__instance.name}");
                }
            }
        }

        private static GameObject GetAttachedCart(global::Humanoid player)
        {
            foreach (var joint in player.GetComponents<ConfigurableJoint>())
            {
                if (joint.connectedBody != null && joint.connectedBody.gameObject.name.Contains("Cart"))
                {
                    return joint.connectedBody.gameObject;
                }
            }

            return null;
        }

        private static void TransferCartToWolf(global::Character wolf, GameObject cart, global::Humanoid player)
        {
            foreach (var joint in cart.GetComponents<ConfigurableJoint>())
            {
                Object.Destroy(joint);
            }

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

            MonsterAI ai = wolf.GetComponent<MonsterAI>();
            if (ai != null)
            {
                ai.m_followTarget = player.transform;
                ai.m_alerted = true;
                ai.m_aggressive = false;
                ai.m_passiveAggressive = false;
            }

            FollowStateManager.SetFollowState(wolf, true);
            wolf.gameObject.AddComponent<CartTracker>().AttachedCart = cart;
        }
    }
}