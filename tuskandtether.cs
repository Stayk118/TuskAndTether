using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace TuskAndTether
{
    [BepInPlugin("com.stayk.tuskandtether", "Tusk and Tether", "1.0.0")]
    public class TuskAndTether : BaseUnityPlugin
    {
        public static ConfigEntry<bool> EnableBoarFollow;
        public static ConfigEntry<bool> EnableLoxFollow;
        public static ConfigEntry<bool> EnableWolfCart;

        public static ConfigEntry<float> WolfWeightLimit_0Star;
        public static ConfigEntry<float> WolfWeightLimit_1Star;
        public static ConfigEntry<float> WolfWeightLimit_2Star;

        private void Awake()
        {
            EnableBoarFollow = Config.Bind("General", "EnableBoarFollow", true, "Allow boars to follow when pet.");
            EnableLoxFollow = Config.Bind("General", "EnableLoxFollow", true, "Allow lox to follow when pet.");
            EnableWolfCart = Config.Bind("General", "EnableWolfCart", true, "Allow wolves to pull carts.");

            WolfWeightLimit_0Star = Config.Bind("CartWeight", "WolfWeightLimit_0Star", 500f, "Max cart weight before slowdown for 0-star wolves.");
            WolfWeightLimit_1Star = Config.Bind("CartWeight", "WolfWeightLimit_1Star", 700f, "Max cart weight before slowdown for 1-star wolves.");
            WolfWeightLimit_2Star = Config.Bind("CartWeight", "WolfWeightLimit_2Star", 900f, "Max cart weight before slowdown for 2-star wolves.");

            Harmony harmony = new Harmony("com.stayk.tuskandtether");
            harmony.PatchAll();
        }
    }
}