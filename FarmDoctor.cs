using System;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace FarmDoctor;

[BepInPlugin("DrakenyDev.Elin.FarmDoctor", "Farm Doctor", "1.1.0")]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log;
    public static ConfigEntry<int> minFarmingLevelBasicInfo;
    public static ConfigEntry<int> minFarmingLevelFullInfo;
    public static ConfigEntry<bool> regrowth;
    public static ConfigEntry<int> minLevelRegrowth;
    private static Harmony harmony;

    private void Start()
    {
        
        minFarmingLevelBasicInfo = Config.Bind("General",      // The section under which the option is shown
                                        "minFarmingLevelBasicInfo",  // The key of the configuration option in the configuration file
                                        10, // The default value
                                        "The minimum farming level to show basic crop information."); // Description of the option to show in the config file

        minFarmingLevelFullInfo = Config.Bind("General", 
                                            "minFarmingLevelFullInfo",
                                            17,
                                            "Minimum farming level to show full crop information.");

        regrowth = Config.Bind("Experimental", 
                                "regrowth",
                                false,
                                "***EXPERIMENTAL*** Enables the ability to trigger regrowth on trees by re-applying fertilizer.");

        minLevelRegrowth = Config.Bind("Experimental", 
                                "minLevelRegrowth",
                                15,
                                "Minimum farming level required to trigger regrowth.");

        Log = base.Logger;
        harmony = new Harmony("DrakenyDev.Elin.FarmDoctor");
        harmony.PatchAll(typeof(GrowthInfo));
        if(regrowth.Value){
            Log.LogInfo("Experimental feature *regrowth* is enabled");
            harmony.PatchAll(typeof(Regrowth));
        }
        Log.LogInfo("DrakenyDev.Elin.FarmDoctor loaded");
    }

    
    private void OnDestroy()
    {
        harmony.UnpatchSelf();
    }
}

