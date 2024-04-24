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
            manager.PlayUISound(pickedUpSound);
            manager.KitchenKnife();
            Destroy(this.gameObject);
        } 
    }

    void Update()
    {
        float delta = Time.deltaTime;
        LightCycle(delta);
    }
}
