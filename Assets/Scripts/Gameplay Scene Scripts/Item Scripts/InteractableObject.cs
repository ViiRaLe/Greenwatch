using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class InteractableObject : MonoBehaviour
{
    protected PlayerController player;

    [SerializeField]
    protected int chargesForInteraction;

    private Renderer rend;
    private MaterialPropertyBlock highlightBlock;
    [SerializeField]
    private Color highlightColor;



    protected virtual void Awake()
    {
        rend = GetComponent<Renderer>();

        highlightBlock = new MaterialPropertyBlock();
        rend.GetPropertyBlock(highlightBlock);
        highlightBlock.SetColor("_Color", highlightColor);
    }

    public virtual void Interact(PlayerController playerController)
    {
        player = playerController;
    }

    protected virtual void InteractSameObject() { }

    //protected virtual void InteractDifferentObject() { }




    public virtual void Highlight()
    {
        rend.SetPropertyBlock(highlightBlock);
    }

    public virtual void Unlit()
    {
        rend.SetPropertyBlock(null);
    }
}
