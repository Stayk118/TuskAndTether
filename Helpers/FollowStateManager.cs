using System.Collections.Generic;
using UnityEngine;

namespace TuskAndTether.Helpers
{
    public static class FollowStateManager
    {
        private static readonly Dictionary<int, bool> followStates = new Dictionary<int, bool>();

        public static void ToggleFollow(Character creature, Humanoid player)
        {
            if (creature == null || player == null) return;

            int id = creature.GetInstanceID();
            bool isFollowing = followStates.ContainsKey(id) && followStates[id];

            MonsterAI ai = creature.GetComponent<MonsterAI>();
            if (ai == null) return;

            if (isFollowing)
            {
                ai.m_followTarget = null;
                ai.m_alerted = false;
                followStates[id] = false;
                Debug.Log($"[TuskAndTether] {creature.name} has stopped following.");

                // Detach cart if present
                CartTracker tracker = creature.GetComponent<CartTracker>();
                if (tracker != null && tracker.AttachedCart != null)
                {
                    DetachCart(tracker.AttachedCart);
                    Object.Destroy(tracker);
                }
            }
            else
            {
                ai.m_followTarget = player.transform;
                ai.m_alerted = true;
                ai.m_aggressive = false;
                ai.m_passiveAggressive = false;
                followStates[id] = true;
                Debug.Log($"[TuskAndTether] {creature.name} is now following {player.GetPlayerName()}.");
            }
        }

        public static void SetFollowState(Character creature, bool isFollowing)
        {
            if (creature == null) return;
            int id = creature.GetInstanceID();
            followStates[id] = isFollowing;
        }

        public static bool IsFollowing(Character creature)
        {
            if (creature == null) return false;
            int id = creature.GetInstanceID();
            return followStates.ContainsKey(id) && followStates[id];
        }

        private static void DetachCart(GameObject cart)
        {
            foreach (var joint in cart.GetComponents<ConfigurableJoint>())
            {
                Object.Destroy(joint);
            }

            Debug.Log("[TuskAndTether] Cart detached from creature.");
        }
    }
}