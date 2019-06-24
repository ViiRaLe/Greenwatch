using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlotObject))]
public class InteractablePlot : InteractableObject
{
    protected PlotObject plotObject;

    [SerializeField]
    protected float particlesOffset = 0.2f;

    [SerializeField]
    private GameObject highlightObject;



    protected override void Awake()
    {
        base.Awake();
        plotObject = GetComponent<PlotObject>();
    }

    public override void Interact(PlayerController playerController)
    {
        base.Interact(playerController);

        if (player.ItemInHand != null && player.ItemInHand.ItemTemplate == plotObject.PlotTemplate.interactableItem)
        {
            InteractSameObject();
        }
        //else
        //{
        //    InteractDifferentObject();
        //}
    }

    protected override void InteractSameObject()
    {
        // instantiate particles prefab
        if (plotObject.PlotTemplate.interactParticlesPrefab != null)
        {
            Instantiate(plotObject.PlotTemplate.interactParticlesPrefab, new Vector3(transform.position.x, transform.position.y + particlesOffset, transform.position.z), Quaternion.identity);
        }
        
        DestroyPlot(plotObject.PlotTemplate.interactedPlot);
        player.ItemInHand.Use(chargesForInteraction);
    }

    protected virtual void DestroyPlot(GameObject nextPlot)
    {
        plotObject.Grid.ChangeTile(plotObject, nextPlot, plotObject.PlotTemplate.addScore);
    }


    public override void Highlight()
    {
        if (highlightObject != null)
        {
            highlightObject.SetActive(true);
        }
    }

    public override void Unlit()
    {
        if (highlightObject != null)
        {
            highlightObject.SetActive(false);
        }
    }
}
