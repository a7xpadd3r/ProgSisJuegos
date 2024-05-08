using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFadeIn : MonoBehaviour
{
    private UIManager _ui;

    private void Start()
    {
        _ui = GetComponent<UIManager>();
        _ui.OnCameraFade(true, 8, Color.black);
    }
}
