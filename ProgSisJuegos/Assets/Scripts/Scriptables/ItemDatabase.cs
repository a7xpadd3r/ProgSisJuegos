using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Data/Items")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] private ItemTypes _itemType;

    [Header("Generic settings")]
    [SerializeField] private AudioClip _pickedUpSound;

    [Header("Healing items")]
    [SerializeField] private float _healAmount;

    public ItemTypes ItemType => _itemType;
    public AudioClip SoundPickedUp => _pickedUpSound;

    public float HealAmount => _healAmount;
}
