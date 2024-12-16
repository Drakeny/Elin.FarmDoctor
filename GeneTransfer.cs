
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace FarmDoctor;

[HarmonyPatch]
public class GeneTransfer
{
    [HarmonyPostfix, HarmonyPatch(typeof(RecipeCard), "Craft")]
    public static void Craft(Thing __result, bool model, List<Thing> ings)
    {
        ings.ForEach(x =>
        {
            if (x.trait is not TraitSeed) return;
            foreach (int k in x.elements.dict.Keys)
            {
                if (!__result.elements.dict.ContainsKey(k))
                {
                    __result.elements.dict.Add(k, x.elements.dict[k]);

                }
            }

        });
    }

    [HarmonyPostfix, HarmonyPatch(typeof(Act), "GetTextSmall")]
    public static void GetTextSmall(Card c, ref string __result)
    {
        if (c == null || c.trait == null || c.trait is not TraitGeneGun) return;

        foreach (int k in c.elements.dict.Keys)
        {
            Element e = c.elements.dict[k];
            if (e.IsFoodTrait)
            {
                int num3 = e.Value / 10;
                num3 = (e.Value < 0) ? (num3 - 1) : (num3 + 1);
                __result = "<size=28>" + c.Name + ": Lv." + num3 + " " + e.source.GetText("textExtra") + "</size>";
            }
        }
    }


    [HarmonyPostfix, HarmonyPatch(typeof(Thing), "WriteNote")]
    public static void WriteNote(Thing __instance, UINote n, IInspect.NoteMode mode, Recipe recipe)
    {
        if (__instance.id != "gene_gun") return;

        __instance.elements.AddNote(n, (Element e) => e.IsFoodTraitMain, null, ElementContainer.NoteMode.Trait, addRaceFeat: false, delegate (Element e, string s)
                {
                    string[] textArray = e.source.GetTextArray("textAlt");
                    int num2 = Mathf.Clamp(e.Value / 10 + 1, (e.Value < 0 || textArray.Length <= 2) ? 1 : 2, textArray.Length - 1);
                    string text = "altEnc".lang(textArray[0].IsEmpty(e.Name), textArray[num2], EClass.debug.showExtra ? (e.Value + " " + e.Name) : "");
                    string text10 = text;
                    string text11 = e.source.GetText("textExtra");
                    if (!text11.IsEmpty())
                    {
                        string text12 = "";
                        int num3 = e.Value / 10;
                        num3 = (e.Value < 0) ? (num3 - 1) : (num3 + 1);
                        text11 = "Lv." + num3 + text12 + " " + text11;
                        if (mode == IInspect.NoteMode.Info)
                        {

                            text11 += "traitAdditive".lang();
                        }
                        text10 += (" <size=12>" + text11 + "</size>").TagColor(FontColor.Passive);
                    }
                    return text10;
                });
    }

}