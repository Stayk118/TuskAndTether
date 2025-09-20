using UnityEngine;

namespace TuskAndTether.Helpers
{
    public static class WeightThresholdHelper
    {
        public static float GetWolfWeightLimit(global::Character wolf)
        {
            if (wolf == null) return 0f;

            return wolf.m_level switch
            {
                0 => TuskAndTether.WolfWeightLimit_0Star.Value,
                1 => TuskAndTether.WolfWeightLimit_1Star.Value,
                2 => TuskAndTether.WolfWeightLimit_2Star.Value,
                _ => TuskAndTether.WolfWeightLimit_0Star.Value
            };
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