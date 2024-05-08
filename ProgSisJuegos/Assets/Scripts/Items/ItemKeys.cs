using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemKeys : ItemBase
{
    [SerializeField] private List<string> _unlockablesIds;

    public override void Interact()
    {
        base.Interact();

        foreach (string id in _unlockablesIds)
            GManager.OnPlayerKeyGrab(id);

        Destroy(gameObject);
    }
}
