using UnityEngine;
using TuskAndTether;

namespace TuskAndTether.Helpers
{
    public static class WeightThresholdHelper
    {
        public static float GetWolfWeightLimit(Character wolf)
        {
            if (wolf == null) return 0f;

            int level = wolf.m_level;
            switch (level)
            {
                case 0:
                    return TuskAndTether.WolfWeightLimit_0Star.Value;
                case 1:
                    return TuskAndTether.WolfWeightLimit_1Star.Value;
                case 2:
                    return TuskAndTether.WolfWeightLimit_2Star.Value;
                default:
                    return TuskAndTether.WolfWeightLimit_0Star.Value;
            }
        }

        public static float GetCartWeight(GameObject cart)
        {
            if (cart == null) return 0f;

            Container container = cart.GetComponent<Container>();
            if (container == null) return 0f;

            float totalWeight = 0f;
            foreach (var item in container.GetInventory().GetAllItems())
            {
                totalWeight += item.GetWeight();
            }

            return totalWeight;
        }
    }
}