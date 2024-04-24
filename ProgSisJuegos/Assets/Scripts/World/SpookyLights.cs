using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyLights : MonoBehaviour
{
    [Header("Lights references")]
    public Light light1;
    public Light light2;
    public Light light3;
    public Light light4;
    public Light light5;

    [Header("Timers")]
    public float tLight1;
    public float tLight2;
    public float tLight3;
    public float tLight4;
    public float tLight5;

    private bool _startSpookySequence;
    private bool _off = true;

    private float ogTLight1;
    private float ogTLight2;
    private float ogTLight3;
    private float ogTLight4;
    private float ogTLight5;

    private void Start()
    {
        ogTLight1 = tLight1;
        ogTLight2 = tLight2;
        ogTLight3 = tLight3;
        ogTLight4 = tLight4;
        ogTLight5 = tLight5;
    }

    void Update()
    {
        float delta = Time.deltaTime;

        if (_startSpookySequence)
        {
            if (_off)
            {
                if (tLight1 > 0) tLight1 -= delta;
                else light1.intensity = 0;

                if (tLight2 > 0) tLight2 -= delta;
                else light2.intensity = 0;

                if (tLight3 > 0) tLight3 -= delta;
                else light3.intensity = 0;

                if (tLight4 > 0) tLight4 -= delta;
                else light4.intensity = 0;

                if (tLight5 > 0) tLight5 -= delta;
                else light5.intensity = 0;

                if (light1.intensity <= 0 && light2.intensity <= 0 && light3.intensity <= 0 && light4.intensity <= 0 && light5.intensity <= 0)
                    _off = false;
            }
            else
            {
                if (tLight1 < ogTLight1) tLight1 += delta;
                else light1.intensity = 1;

                if (tLight2 < ogTLight2) tLight2 += delta;
                else light2.intensity = 1;

                if (tLight3 < ogTLight3) tLight3 += delta;
                else light3.intensity = 1;

                if (tLight4 < ogTLight4) tLight4 += delta;
                else light4.intensity = 1;

                if (tLight5 < ogTLight5) tLight5 += delta;
                else light5.intensity = 1;

                if (light1.intensity >= 1 && light2.intensity >= 1 && light3.intensity >= 1 && light4.intensity >= 1 && light5.intensity >= 1)
                    Destroy(this.gameObject);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _startSpookySequence = true;
    }
}
