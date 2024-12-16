using System;
using System.Linq;
using System.Runtime.CompilerServices;
using BepInEx;
using HarmonyLib;
using UnityEngine;


namespace FarmDoctor;


[HarmonyPatch]
public class Regrowth
{
    [HarmonyPostfix, HarmonyPatch(typeof(Zone), "OnActivate")]
    static void OnActivate()
    {
        if (!EClass._zone.IsPCFaction) return;
        foreach (var plantPair in EClass._map.plants)
        {
            PlantData plant = plantPair.Value.IsNull() ? null : plantPair.Value;
            if (plant.seed != null)
            {
                string[] fruit_trees = ["banana", "fruit", "fruit_pear", "fruit_orange"];

                if (!fruit_trees.Contains(plant.seed.Cell.growth.source.GetAlias))
                {
                    return;
                }
                if (plant.seed.Cell.IsFarmField && plant.seed.Cell.gatherCount > 0)
                {
                    if (plant.seed.Cell.growth.source.GetAlias == "banana")
                    {
                        plant.seed.Cell.growth.stages[3].tiles[0] = 157;
                    }
                    else
                    {
                        plant.seed.Cell.growth.stages[3].tiles[0] = 195;
                    }
                }
            }
        }

    }

    [HarmonyPrefix, HarmonyPatch(typeof(TraitFertilizer), "OnSimulateHour")]
    public static bool OnSimulateHourPrefix(TraitFertilizer __instance, VirtualDate date, out Cell __state)
    {
        //Hmm, could probably run everything on prefix, but i'll leave like this for now.
        __state = __instance.owner.Cell;
        return true;
    }

    [HarmonyPostfix, HarmonyPatch(typeof(TraitFertilizer), "OnSimulateHour")]
    public static void OnSimulateHour(TraitFertilizer __instance, VirtualDate date, Cell __state)
    {
        if (!EClass._zone.IsPCFaction) return;
        if (Plugin.minLevelRegrowth.Value > EClass.pc.elements.Value(286)) return;
        try
        {
            Cell cell = __state;
            if (cell.growth == null || !cell.IsFarmField) return;
            string[] fruit_trees = ["banana", "fruit", "fruit_pear", "fruit_orange"];
            if (!fruit_trees.Contains(cell.growth.source.GetAlias))
            {
                return;
            }

            if (cell.isHarvested)
            {
                if (cell.growth.source.GetAlias == "banana")
                {
                    cell.growth.stages[3].tiles[0] = 157;
                }
                else
                {
                    cell.growth.stages[3].tiles[0] = 195;
                }
                cell.objVal = 90;
                cell.isHarvested = false;
                cell.gatherCount++;
            }
        }
        catch (NullReferenceException)
        {
            Plugin.Log.LogError("Congratulations! You've triggered a NullReferenceException! I have no idea how to fix it, but don’t worry—it causes no harm. All is good. May Kumiromi be with you!");
        }
    }
}
