using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Template", menuName = "Green Watch/Item Template")]
public class ItemTemplate : ScriptableObject
{
    public GameObject thisInHandItem;
    public GameObject thisShowcaseItem;
    public Sprite[] iconSprites;

    [Space, Header("Sounds", order = 0), Header("Increase Charges Sound", order = 1)]
    public AudioClip increaseChargesAudioClip;
    public bool increaseChargesLoop = false;
    public int increaseChargesPriority = 128;
    public float increaseChargesVolume = 1;
    [Min(0.75f)]
    public float increaseChargesPitchMin = 0.85f;
    [Min(1)]
    public float increaseChargesPitchMax = 1.15f;

    [Space(order = 0), Header("Use Item Sound", order = 1)]
    public AudioClip useItemAudioClip;
    public bool useItemLoop = false;
    public int useItemPriority = 128;
    public float useItemVolume = 1;
    [Min(0.75f)]
    public float useItemPitchMin = 0.85f;
    [Min(1)]
    public float useItemPitchMax = 1.15f;
}
