using System;
using System.Linq;
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
        foreach (var plantPair in EClass._map.plants)
        {
            PlantData plant = plantPair.Value;
            if (plant.seed.Cell.IsFarmField && plant.seed.Cell.gatherCount > 0)
            {
                if(plant.seed.Cell.growth.source.GetAlias == "banana")
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

    [HarmonyPostfix, HarmonyPatch(typeof(TraitFertilizer), "OnSimulateHour")]
    public static void OnSimulateHour(TraitFertilizer __instance, VirtualDate date)
    {
        if(!EClass._zone.IsPCFaction) return;
        if(Plugin.minLevelRegrowth.Value < EClass.pc.elements.Value(286)) return;
        Cell cell = __instance.owner.Cell;
        if (cell.IsFarmField && cell.isHarvested)
        {
            if(cell.growth.source.GetAlias == "banana")
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
}
