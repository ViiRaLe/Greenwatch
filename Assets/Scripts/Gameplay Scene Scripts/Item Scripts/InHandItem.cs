using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHandItem : MonoBehaviour
{
    [SerializeField]
    private int chargesMax;

    private int charges = 0;

    [SerializeField]
    private ItemTemplate itemTemplate;

    private event Action<int> OnWeightChange;
    


    public ItemTemplate ItemTemplate
    {
        get
        {
            return itemTemplate;
        }
    }

    public int Charges
    {
        get
        {
            return charges;
        }
    }



    public void ChangeCharges(int charge)
    {
        int previousCharges = charges;

        charges += charge;
        charges = Mathf.Clamp(charges, 1, chargesMax);

        // play increase charges sound
        if (charges > previousCharges)
        {
            SoundManager.Instance.PlayAudioClip(itemTemplate.increaseChargesAudioClip, true, itemTemplate.increaseChargesLoop,
                itemTemplate.increaseChargesPriority, itemTemplate.increaseChargesVolume, UnityEngine.Random.Range(itemTemplate.increaseChargesPitchMin, itemTemplate.increaseChargesPitchMax));
        }

        OnWeightChange?.Invoke(charges);
    }

    public void Use(int charge)
    {
        charges -= charge;

        // play use item sound
        SoundManager.Instance.PlayAudioClip(itemTemplate.useItemAudioClip, true, itemTemplate.useItemLoop,
            itemTemplate.useItemPriority, itemTemplate.useItemVolume, UnityEngine.Random.Range(itemTemplate.useItemPitchMin, itemTemplate.useItemPitchMax));

        OnWeightChange?.Invoke(charges);

        if (charges <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void RegisterOnWeightChange(Action<int> action)
    {
        OnWeightChange += action;
        OnWeightChange?.Invoke(charges);
    }

    public void UnregisterOnWeightChange(Action<int> action)
    {
        OnWeightChange?.Invoke(charges);
        OnWeightChange -= action;
    }
}
