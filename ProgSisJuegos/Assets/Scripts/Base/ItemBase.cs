using Unity.VisualScripting;
using UnityEngine;

public class ItemBase : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private Light _lightObject;
    public GameManager manager;

    [Header("Settings")]
    public AudioClip pickedUpSound;
    [SerializeField] private bool _useIlumination = true;
    [SerializeField] private Color _lightColor = Color.white;
    [SerializeField, Range(0.1f, 2)] private float _lightGlowSpeed = 1;

    private bool _brightnessUp;
    private bool _alreadyInteracted;

    public virtual void Awake()
    {
        if (_useIlumination && _lightObject != null) _lightObject.color = _lightColor;
    }

    public virtual void Interact()
    {
        
    }

    public virtual void LightCycle(float deltaTime)
    {
        if (_useIlumination && _lightObject != null) 
        { 
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
