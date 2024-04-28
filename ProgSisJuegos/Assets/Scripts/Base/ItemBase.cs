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

    public virtual void Interact()
    {
        _glowScript.enabled = false;
        GManager.PlayUISound(SoundPickedUp);
    }

    private void Start()
    {
        _glowScript = GetComponent<ItemLightGlow>();
    }
}
