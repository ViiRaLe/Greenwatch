using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private PlotGrid grid;
    private GameTimer timer;

    [SerializeField]
    private float cooldown;
    private float cooldownCurrent = 0;

    [SerializeField]
    private float spawnTime;

    [Space, SerializeField, Header("Time Curve", order = 0)]
    private AnimationCurve timeProbabilityCurve;
    [SerializeField, Header("Attackable Curve", order = 0)]
    private AnimationCurve attackableCurve;
    [SerializeField, Header("Tree Curve", order = 0)]
    private AnimationCurve treeCurve;

    private float timeAverage;
    private float timeDelta;

    [SerializeField]
    private PlotTemplate buildableTemplate;

    private PlotObject plotToAttack = null;

    private void Awake()
    {
        timer =  FindObjectOfType(typeof(GameTimer)) as GameTimer;
        grid = FindObjectOfType(typeof(PlotGrid)) as PlotGrid;
    }

    private void Start()
    {
        InitializeTimer();
    }

    private void Update()
    {
        // check if there is at least an attackable plot
        if (grid.AttackablePlots.Count <= 0)
        {
            return;
        }

        // check cooldown and start enemy spawn timer
        if (cooldownCurrent > 0)
        {
            cooldownCurrent -= Time.deltaTime;
        }
        else
        {
            timeAverage = (attackableCurve.Evaluate(grid.AttackablePlotPercentage) + timeProbabilityCurve.Evaluate(timer.CurrentTimePercentage) + treeCurve.Evaluate(grid.treePlotPercentage)) / 3;

            if ((Time.deltaTime / timeAverage) >= Random.value)
            {
                List<PlotObject> plotList = new List<PlotObject>();

                grid.GetAttackablePlotsByTemplate(ref plotList, buildableTemplate);

                if (plotList.Count > 0)
                {
                    plotToAttack = plotList[Random.Range(0, plotList.Count - 1)];
                }
                else
                {
                    grid.GetAttackablePlotsByDifferences(ref plotList, buildableTemplate);

                    plotToAttack = plotList[Random.Range(0, plotList.Count - 1)];
                }

                // start plot attack
                if (plotToAttack != null)
                {
                    plotToAttack.StartCoroutine(plotToAttack.AttackPlot(spawnTime));
                    plotToAttack = null;
                    InitializeTimer();
                }
            }
        }
    }

    private void InitializeTimer()
    {
        cooldownCurrent = cooldown;
        timeDelta = 0;
    }
}
