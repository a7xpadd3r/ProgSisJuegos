using System;
using Unity.VisualScripting;
using UnityEngine;

public class ItemBase : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private GameManager _manager;
    private ItemLightGlow _glowScript;

    [Header("Settings")]
    [SerializeField] private AudioClip _pickedUpSound;

    public AudioClip SoundPickedUp => _pickedUpSound;
    public GameManager GManager => _manager;

    public Action OnInteracted;

    void Start()
    {
        _glowScript = GetComponent<ItemLightGlow>();

        if (_manager == null)
            _manager = FindObjectOfType<GameManager>();
    }

    public virtual void Interact()
    {
        OnInteracted?.Invoke();
        if (_glowScript!= null) _glowScript.enabled = false;
        GManager?.PlayUISound(SoundPickedUp);
    }
}
