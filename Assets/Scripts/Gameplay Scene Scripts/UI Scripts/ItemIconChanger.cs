using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemIconChanger : MonoBehaviour
{
    private Image image;
    private PlayerController player;

    private Color transparency, fullColor;

    private void Awake()
    {
        image = GetComponent<Image>();
        player = GetComponentInParent<PlayerController>();
        transparency = new Color(255, 255, 255, 0);
        fullColor = new Color(255, 255, 255, 255);
    }


    private void OnEnable()
    {
        player.RegisterOnWeightChange(ChangeIcon);
    }

    private void OnDisable()
    {
        player.UnregisterOnWeightChange(ChangeIcon);
    }

    private void ChangeIcon(int charges)
    {
        if (charges > 0)
        {
            image.sprite = player.ItemInHand.ItemTemplate.iconSprites[charges - 1];
            image.color = fullColor;
        }
        else
        {

            image.sprite = null;
            image.color = transparency;
        }
    }
}
