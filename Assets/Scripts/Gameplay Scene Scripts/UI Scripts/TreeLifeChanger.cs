using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLifeChanger : MonoBehaviour
{
    public GameObject tree;
    private MaterialPropertyBlock materialBlockLife;
    private Renderer rend;

    private void Awake()
    {
        materialBlockLife = new MaterialPropertyBlock();
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        materialBlockLife.SetFloat("_Threshold", Mathf.Abs((tree.GetComponent<BigTreeBehaviour>().TreeLife / tree.GetComponent<BigTreeBehaviour>().TreeLifeMax) - 1));
        rend.SetPropertyBlock(materialBlockLife);
    }
}
