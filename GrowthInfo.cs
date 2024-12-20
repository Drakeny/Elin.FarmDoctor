using System;
using System.Linq;
using BepInEx;
using HarmonyLib;
using UnityEngine;


namespace FarmDoctor;

[HarmonyPatch]
public class GrowthInfo
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
                float num = __instance.pos.cell.growth.Step * 1 * (flag2 ? 2 : 1); // Hardcoded '1' as the grow function is not using MtpProgress
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
                        CalculateDaysTillNextStage(num, __instance.pos.cell.objVal, flag2).ToString(),
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

    public static int CalculateDaysTillNextStage(float num, int objVal, bool isWatered)
    {
        int nextStageObjVal = (int)Math.Ceiling((double)objVal / 30) * 30;
        if (nextStageObjVal == objVal) // plant has just grown
        {
            objVal = (int)Math.Ceiling((double)objVal / 30) + 1; // update objVal to new value after growth
        }
        int remainingObjVal = (int)Math.Ceiling((double)objVal / 30) * 30 - objVal;
        int daysToReachNextStage;
        if (isWatered)
        {
            remainingObjVal -= (int)num/2;
            daysToReachNextStage = (int)Math.Ceiling((double)remainingObjVal / (num / 2));;
        }
        else
        {
            daysToReachNextStage = (int)Math.Ceiling((double)remainingObjVal / num);
        }
        return daysToReachNextStage;
    }
}