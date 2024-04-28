using UnityEngine;

public class ItemLightGlow : MonoBehaviour
{
    private Light _lightObject;
    [SerializeField] private bool _useIlumination = true;
    [SerializeField] private Color _lightColor = Color.white;
    [SerializeField, Range(0.1f, 2)] private float _lightGlowSpeed = 1;

    private bool _brightnessUp;

    void Start()
    {
        _lightObject = GetComponent<Light>();
        _lightObject.color = _lightColor;
    }

    private void Update()
    {
        if (_useIlumination && _lightObject != null)
        {
            float deltaTime = Time.deltaTime;
            if (_brightnessUp)
            {
                _lightObject.intensity += deltaTime * _lightGlowSpeed;

                if (_lightObject.intensity >= 1)
                    _brightnessUp = false;
            }

            else
            {
                _lightObject.intensity -= deltaTime * _lightGlowSpeed;

                if (_lightObject.intensity <= 0.1)
                    _brightnessUp = true;
            }
        }
    }
}
