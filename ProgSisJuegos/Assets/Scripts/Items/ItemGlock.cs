using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGlock : ItemBase
{
    public override void Interact()
    {
        base.Interact();
        GManager.GiveWeaponToPlayer(WeaponTypes.Glock);
        Destroy(this.gameObject);
    }
}
