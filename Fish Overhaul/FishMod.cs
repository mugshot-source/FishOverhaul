using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using BepInEx.Configuration;

namespace Fish_Overhaul
{
    [BepInPlugin("fish.overhaul", "Fish Overhaul", "1.1")]
    [BepInProcess("valheim.exe")]
    public class FishMod : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("fish.overhaul");
        public static ManualLogSource Log;
        private static ConfigEntry<float> pullLineSpeedMultiplier, forceMultiplier;

        void Awake()
        {
            pullLineSpeedMultiplier = Config.Bind<float>("Modify pull speed", "pullLineSpeedMultiplier", 5f, "Speed multiplier");
            forceMultiplier = Config.Bind<float>("Modify force", "forceMultiplier", 40f, "Force multiplier");
            harmony.PatchAll();
        }

        void OnDestroy()
        {
            harmony.UnpatchSelf();
        }

        [HarmonyPatch(typeof(Fish), "IsOutOfWater")]
        static class Fish_Patch
        {
            static void Prefix(ref float ___m_inWater, ref float ___m_speed)
            {
                ___m_inWater = -1000000000;
                ___m_speed = 4f;
            }
        }
        [HarmonyPatch(typeof(FishingFloat), "FixedUpdate")]
        static class Fishing_Patch
        {
            static void Prefix(ref float ___m_moveForce, ref float ___m_pullLineSpeed)
            {

                ___m_pullLineSpeed = pullLineSpeedMultiplier.Value;
                ___m_moveForce = forceMultiplier.Value;
            }
        }
    }
}
