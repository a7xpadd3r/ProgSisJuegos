using System;
using Unity.VisualScripting;
using UnityEngine;

public class ItemBase : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private ItemDatabase _itemData;
    [SerializeField] private GameManager _manager;
    private ItemLightGlow _glowScript;

    public AudioClip SoundPickedUp => _itemData.SoundPickedUp;
    public GameManager GManager => _manager;

    public ItemDatabase ItemData => _itemData;
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
