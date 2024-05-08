using System.Collections.Generic;
using UnityEngine;

public class ControlsKitchenKnifeHide : MonoBehaviour
{
    public List<GameObject> checkThese;

    private void LateUpdate()
    {
        foreach (var item in checkThese)
            if (item != null) return;

        Destroy(gameObject);
    }
}
