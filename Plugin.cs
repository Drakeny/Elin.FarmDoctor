using System;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace FarmDoctor;

[BepInPlugin("DrakenyDev.Elin.FarmDoctor", "Farm Doctor", "1.0.0.0")]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log;
    public static ConfigEntry<int> minFarmingLevelBasicInfo;
    public static ConfigEntry<int> minFarmingLevelFullInfo;
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

        Log = base.Logger;
        harmony = new Harmony("DrakenyDev.Elin.FarmDoctor");
        harmony.PatchAll();
    }

    
private void OnDestroy()
{
    harmony.UnpatchSelf();
}
}

[HarmonyPatch]
public class DrakTest
{
    [HarmonyPostfix, HarmonyPatch(typeof(BaseTaskHarvest), "GetTextSmall")]
    public static void GetTextSmall(BaseTaskHarvest __instance, ref string __result){
        bool flag = __instance.pos.cell.growth != null;
        if(flag){
            bool flag2 = __instance.pos.cell.isWatered || (__instance.pos.cell.IsTopWater && __instance.pos.cell.growth.source.tag.Contains("flood"));

            int FarmingLevel = EClass.pc.elements.Value(286);  
            if (FarmingLevel < Plugin.minFarmingLevelBasicInfo.Value) return;

            bool WitherOnLastStage = (bool)Traverse.Create(__instance.pos.cell.growth).Property("WitherOnLastStage").GetValue();
            int lastStage = WitherOnLastStage ? __instance.pos.cell.growth.HarvestStage + 1 : __instance.pos.cell.growth.HarvestStage;
            bool withered = __instance.pos.cell.growth.IsWithered();

            if (FarmingLevel >= Plugin.minFarmingLevelFullInfo.Value)
            {
                float num = __instance.pos.cell.growth.Step * __instance.pos.cell.growth.MtpProgress * (flag2 ? 2 : 1);
                string phase = __instance.pos.cell.growth.HarvestStage == __instance.pos.cell.growth.stage.idx ? "Withering" : "Growth";

                string rdy = ") / Ready to Harvest] ";

                if (__instance.pos.cell.IsFarmField && __instance.pos.cell.growth.stage.idx == __instance.pos.cell.growth.HarvestStage) { // lazyyyyyyyyyyy

                    if (__instance.pos.cell.isHarvested) {
                        rdy = ") / Already harvested] ";
                    }
                    __result = string.Concat(new string[]
                    {
                        __result,
                        " [Growth stage (",
                        __instance.pos.cell.growth.stage.idx.ToString(),
                        "/",
                        lastStage.ToString(),
                        rdy
                    });

                } else {
                    __result = string.Concat(new string[]
                    {
                        __result,
                        " [Growth stage (",
                        __instance.pos.cell.growth.stage.idx.ToString(),
                        "/",
                        lastStage.ToString(),
                        ") / ",
                        CalculateDaysTillNextStage(num, __instance.pos.cell.objVal).ToString(),
                        "~ Day(s) until ",
                        withered ? "Perish" : phase,
                        "]"
                    });
                }

                
            }else {
                __result = string.Concat(new string[]
                {
                    __result,
                    " [Growth stage (",
                    __instance.pos.cell.growth.stage.idx.ToString(),
                    "/",
                    lastStage.ToString(),
                    ")] "
                });
            }
        
        }
    }

    public static int CalculateDaysTillNextStage(float num, int objVal)
    {
        int nextStageObjVal = (int)Math.Ceiling((double)objVal / 30) * 30;
        if (nextStageObjVal == objVal) // plant has just grown
        {
            objVal = (int)Math.Ceiling((double)objVal / 30) + 1; // update objVal to new value after growth
        }
        int remainingObjVal = (int)Math.Ceiling((double)objVal / 30) * 30 - objVal;
    
        int daysToReachNextStage = (int)Math.Ceiling((double)remainingObjVal / num);
        return daysToReachNextStage;
    }

}
