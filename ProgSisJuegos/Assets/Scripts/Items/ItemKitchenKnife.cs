using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemKitchenKnife : ItemBase
{
    [SerializeField] private bool _triggerSequence = true;
    public KitchenKnifeToRockletsSequence rockletsSequencer;
    private bool firstInteraction = true;

    public override void Interact()
    {
        if (_triggerSequence) 
        {
            if (firstInteraction)
            {
                rockletsSequencer.StartRockletsSequence();
                firstInteraction = false;
            }

            else
            {
                base.Interact();
                GManager.GiveWeaponToPlayer(WeaponTypes.KitchenKnife);
                Destroy(this.gameObject);
            }
        }

        else
        {
            base.Interact();
            GManager.GiveWeaponToPlayer(WeaponTypes.KitchenKnife);
            Destroy(this.gameObject);
        }
    }
}
