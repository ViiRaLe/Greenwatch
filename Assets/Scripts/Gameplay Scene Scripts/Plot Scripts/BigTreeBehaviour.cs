using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class BigTreeBehaviour : InteractablePlot
{
    [SerializeField]
    private float treeLifeMax;
    private float treeLife;

    [SerializeField]
    private float fireDamage;

    [SerializeField]
    private GameObject ediblePlotPrefab;
    [SerializeField]
    private GameObject gianniPrefab;

    public int scoreonFire;



    public float TreeLifeMax
    {
        get
        {
            return treeLifeMax;
        }
    }

    public float TreeLife
    {
        get
        {
            return treeLife;
        }
    }



    private void Start()
    {
        treeLife = treeLifeMax;
    }

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            treeLife -= fireDamage;

            if (treeLife <= 0f)
            {
                Instantiate(gianniPrefab, new Vector3(transform.position.x, transform.position.y + particlesOffset, transform.position.z), Quaternion.identity);
                plotObject.Grid.ChangeTile(plotObject, ediblePlotPrefab, -scoreonFire);
            }
        }
        else
        {
            GetComponent<AudioSource>().Stop();
        }
    }
}
