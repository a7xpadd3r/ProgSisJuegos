using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPills : ItemBase
{
    public override void Interact()
    {
        base.Interact();
        GManager.OnPillsGrab(this);
        Destroy(gameObject);
    }
}
