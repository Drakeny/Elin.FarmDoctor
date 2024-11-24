using System;
using System.Linq;
using BepInEx;
using HarmonyLib;
using UnityEngine;


namespace FarmDoctor;


[HarmonyPatch]
public class CropInfo
{
    [HarmonyPostfix, HarmonyPatch(typeof(ActionMode), "UpdateInput")]
    public static void UpdateInput(ActionMode __instance)
    {

        if (EInput.action == EAction.Examine && !__instance.IsBuildMode && !EClass.ui.isPointerOverUI)
        {
            Cell _cell = EClass.scene.mouseTarget.pos.cell;
            if (_cell != null)
            {
                PlantData plantData = EClass._map.TryGetPlant(_cell);
                Thing thing = (plantData != null) ? plantData.seed : null;
                if (thing != null)
                {
                    EClass.ui.AddLayer<LayerInfo>().Set(thing, true);
                }
            }
        }
    }
}