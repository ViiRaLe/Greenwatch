using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : InteractableObject
{
    [SerializeField]
    private ItemTemplate itemTemplate;



    public override void Interact(PlayerController playerController)
    {
        base.Interact(playerController);

        if (player.ItemInHand == null || player.ItemInHand.ItemTemplate != itemTemplate)
        {
            player.ChangeItem(itemTemplate.thisInHandItem);
            //InteractDifferentObject();
        }

        InteractSameObject();
    }

    protected override void InteractSameObject()
    {
        player.ItemInHand.ChangeCharges(chargesForInteraction);
    }

    //protected override void InteractDifferentObject()
    //{
    //    player.ChangeItem(interactableItem);
    //}
}
