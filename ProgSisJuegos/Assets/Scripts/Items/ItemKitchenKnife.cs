using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemKitchenKnife : ItemBase
{
    public KitchenKnifeToRockletsSequence rockletsSequencer;
    private bool firstInteraction = true;

    public override void Interact()
    {
        if (firstInteraction)
        {
            rockletsSequencer.StartRockletsSequence();
            firstInteraction = false;
        }

        else
        {
            GManager.GiveWeaponToPlayer(WeaponTypes.KitchenKnife);
            Destroy(this.gameObject);
        } 
    }
}
