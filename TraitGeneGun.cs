using System.Linq;

class TraitGeneGun : TraitTool
{
    public override void TrySetHeldAct(ActPlan p)
    {
        if (p.pos.cell.TryGetPlant() == null || !EClass._zone.IsPCFaction) return;
        Thing targetPlant = p.pos.cell.TryGetPlant()?.seed;
        Thing gun = owner.Thing;

        p.TrySetAct("Shoot Gene", delegate
        {
            foreach (int k in targetPlant.elements.dict.Keys.Reverse())
            {
                if (targetPlant.elements.dict[k].IsFoodTraitMain || targetPlant.elements.dict[k].IsFoodTrait)
                {
                    targetPlant.elements.dict.Remove(k);
                }
            }
            foreach (int k in gun.elements.dict.Keys)
            {
                if (gun.elements.dict[k].IsFoodTraitMain || gun.elements.dict[k].IsFoodTrait)
                {
                    targetPlant.elements.dict.Add(k, gun.elements.dict[k]);
                }
            }

            gun.PlayEffect("bullet").Emit(1);
            gun.PlaySound("attack_gun");
            targetPlant.PlaySound("mutation");
            targetPlant.PlayEffect("mutation");


            if (owner.c_charges <= 0)
            {
                EClass.pc.Say("spellbookCrumble", owner);
                owner.Destroy();
            }
            return false;
        }, this.owner, CursorSystem.IconRange, 1);

    }
}