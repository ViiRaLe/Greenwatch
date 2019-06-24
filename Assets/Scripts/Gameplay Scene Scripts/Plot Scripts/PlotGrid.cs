using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Grid))]
public class PlotGrid : MonoBehaviour
{
    const int XVALUEMULTIPLIER = 1000;

    private Grid grid;

    private Dictionary<int, PlotObject> plotDict;

    [SerializeField]
    private PlotTemplate randomizableTemplate;
    [SerializeField]
    private GameObject[] randomizablePlots;
    [SerializeField]
    private PlotTemplate aiPlotTemplate;
    [SerializeField]
    private PlotTemplate treePlotTemplate;

    private List<PlotObject> attackablePlots = new List<PlotObject>();
    private List<PlotObject> treePlots = new List<PlotObject>();

    private int attackablePlotTotal = 0;

    [SerializeField]
    private PlotTemplate startingPointTemplate;

    [SerializeField]
    private GameObject popupPointsPrefab;
    [SerializeField]
    private float popupOffsetY = 0.3f;



    public List<PlotObject> AttackablePlots
    {
        get
        {
            return attackablePlots;
        }
    }

    public float AttackablePlotPercentage
    {
        get
        {
            return attackablePlots.Count / attackablePlotTotal;
        }
    }

    public float treePlotPercentage
    {
        get
        {
            return treePlots.Count / attackablePlotTotal;
        }
    }



    private void Awake()
    {
        // randomize seed
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);

        grid = GetComponent<Grid>();
    }

    private void Start()
    {
        GameManager.Instance.GameState.ResetScore();

        // initialize plot grid
        Initialize();

        // randomize attackable plots
        Randomize();

        // set nearest enemy plot and update score
        SetNearestEnemy();
    }

    private void Update()
    {
        // update attackable plots list
        UpdateAttackablePlots(ref attackablePlots);
        UpdateTreePlots(ref treePlots);
    }


    // initialize plot grid
    private void Initialize()
    {
        plotDict = new Dictionary<int, PlotObject>();

        foreach (Transform childTransform in transform)
        {
            int xVal = (int)childTransform.position.x;
            int zVal = (int)childTransform.position.z;

            PlotObject po = childTransform.GetComponent<PlotObject>();
            Debug.Assert(po != null);
            plotDict.Add(xVal * XVALUEMULTIPLIER + zVal, po);
            plotDict[xVal * XVALUEMULTIPLIER + zVal].Initialize(this, xVal, zVal);
        }
    }

    // randomize attackable plots
    private void Randomize()
    {
        var randomizablePlotsQuery =
            from plotObj in plotDict
            where plotObj.Value.PlotTemplate == randomizableTemplate
            select plotObj;

        List<KeyValuePair<int, PlotObject>> dictList = new List<KeyValuePair<int, PlotObject>>();
        dictList = randomizablePlotsQuery.ToList();

        foreach (KeyValuePair<int, PlotObject> plotObj in dictList)
        {
            int poIndex = plotObj.Value.XValue * XVALUEMULTIPLIER + plotObj.Value.ZValue;

            // randomize new plot object and instantiate it
            plotDict[poIndex] = Instantiate(randomizablePlots[UnityEngine.Random.Range(0, randomizablePlots.Length)], transform).GetComponent<PlotObject>();
            plotDict[poIndex].transform.position = new Vector3(plotObj.Value.XValue, 0, plotObj.Value.ZValue);

            // initialize the new plot object
            plotDict[poIndex].Initialize(this, plotObj.Value.XValue, plotObj.Value.ZValue);

            // destroy the old plot object
            Destroy(plotObj.Value.gameObject);
        }
    }

    // set nearest enemy plot and update score
    private void SetNearestEnemy()
    {
        foreach (PlotObject plot in plotDict.Values)
        {
            plot.EnemyPlot = GetNearestEnemy(plot);
        }

        var plotObjectQuery =
            from plotObj in plotDict.Values
            where plotObj.PlotTemplate.aiPlot != null
            select plotObj;

        attackablePlotTotal = plotObjectQuery.Count();

        var plotTreeQuery =
            from plotObj in plotDict.Values
            where (plotObj.PlotTemplate == startingPointTemplate)
            select plotObj;

        GameManager.Instance.ChangeScore(plotTreeQuery.Count());
    }


    // destroy a tile and replace it
    public void ChangeTile(PlotObject tileToReplace, GameObject tileToInstantiate, int score)
    {
        int poIndex = tileToReplace.XValue * XVALUEMULTIPLIER + tileToReplace.ZValue;

        plotDict[poIndex] = Instantiate(tileToInstantiate, transform).GetComponent<PlotObject>();
        plotDict[poIndex].transform.position = new Vector3(tileToReplace.XValue, 0, tileToReplace.ZValue);
        
        plotDict[poIndex].Initialize(this, tileToReplace.XValue, tileToReplace.ZValue, tileToReplace.SkinIndex);

        // get nearest enemy plot for the new plot and the four adjacent plots (recursive my wildest dream!!!)
        plotDict[poIndex].EnemyPlot = GetNearestEnemy(plotDict[tileToReplace.XValue * XVALUEMULTIPLIER + tileToReplace.ZValue]);

        plotDict[(tileToReplace.XValue - 1) * XVALUEMULTIPLIER + tileToReplace.ZValue].EnemyPlot = GetNearestEnemy(plotDict[(tileToReplace.XValue - 1) * XVALUEMULTIPLIER + tileToReplace.ZValue]);
        plotDict[(tileToReplace.XValue + 1) * XVALUEMULTIPLIER + tileToReplace.ZValue].EnemyPlot = GetNearestEnemy(plotDict[(tileToReplace.XValue + 1) * XVALUEMULTIPLIER + tileToReplace.ZValue]);
        plotDict[tileToReplace.XValue * XVALUEMULTIPLIER + tileToReplace.ZValue - 1].EnemyPlot = GetNearestEnemy(plotDict[tileToReplace.XValue * XVALUEMULTIPLIER + tileToReplace.ZValue - 1]);
        plotDict[tileToReplace.XValue * XVALUEMULTIPLIER + tileToReplace.ZValue + 1].EnemyPlot = GetNearestEnemy(plotDict[tileToReplace.XValue * XVALUEMULTIPLIER + tileToReplace.ZValue + 1]);

        // destroy the old plot
        Destroy(tileToReplace.gameObject);

        if (score != 0)
        {
            Bounds bounds = plotDict[poIndex].gameObject.GetComponentInChildren<Renderer>().bounds;

            PopupPoint popup = Instantiate(popupPointsPrefab, new Vector3(plotDict[poIndex].transform.position.x, plotDict[poIndex].transform.position.y + popupOffsetY, plotDict[poIndex].transform.position.z), Quaternion.identity).GetComponent<PopupPoint>();

            if (score > 0)
            {
                popup.SetText("+" + score.ToString());
            }
            else
            {
                popup.SetText(score.ToString());
            }
        }

        GameManager.Instance.ChangeScore (score);
    }


    // get nearest enemy plot
    public PlotObject GetNearestEnemy(PlotObject plot)
    {
        if (plot.PlotTemplate.aiPlot == null)
        {
            return null;
        }

        PlotObject returnPlot;

        // upper row
        if (plotDict.TryGetValue((plot.XValue + 1) * XVALUEMULTIPLIER + plot.ZValue, out returnPlot))
        {
            if (returnPlot.PlotTemplate == aiPlotTemplate)
            {
                return returnPlot;
            }
        }

        // lower row
        if (plotDict.TryGetValue((plot.XValue - 1) * XVALUEMULTIPLIER + plot.ZValue, out returnPlot))
        {
            if (returnPlot.PlotTemplate == aiPlotTemplate)
            {
                return returnPlot;
            }
        }

        // left column
        if (plotDict.TryGetValue(plot.XValue * XVALUEMULTIPLIER + (plot.ZValue - 1), out returnPlot))
        {
            if (returnPlot.PlotTemplate == aiPlotTemplate)
            {
                return returnPlot;
            }
        }

        // right column
        if (plotDict.TryGetValue(plot.XValue * XVALUEMULTIPLIER + (plot.ZValue + 1), out returnPlot))
        {
            if (returnPlot.PlotTemplate == aiPlotTemplate)
            {
                return returnPlot;
            }
        }

        return returnPlot = null;
    }


    // update attackable plots list
    public void UpdateAttackablePlots(ref List<PlotObject> plotList)
    {
        var plotObjectQuery =
            from plotObj in plotDict.Values
            where (plotObj.EnemyPlot != null && plotObj.UnderAttack == false)
            select plotObj;

        plotList = plotObjectQuery.ToList();
    }

    // update tree plots list
    public void UpdateTreePlots(ref List<PlotObject> plotList)
    {
        var plotObjectQuery =
            from plotObj in plotDict.Values
            where (plotObj.PlotTemplate == treePlotTemplate)
            select plotObj;

        plotList = plotObjectQuery.ToList();
    }

    // get attackable plots list by template
    public void GetAttackablePlotsByTemplate(ref List<PlotObject> plotList, PlotTemplate plotTemplate)
    {
        var plotObjectQuery =
            from plotObj in plotDict.Values
            where (plotObj.EnemyPlot != null && plotObj.PlotTemplate == plotTemplate && plotObj.UnderAttack == false)
            select plotObj;

        plotList = plotObjectQuery.ToList();
    }

    // get attackable plots by difference
    public void GetAttackablePlotsByDifferences(ref List<PlotObject> plotList, PlotTemplate plotTemplate)
    {
        var plotObjectQuery =
            from plotObj in plotDict.Values
            where (plotObj.EnemyPlot != null && plotObj.PlotTemplate != plotTemplate && plotObj.UnderAttack == false)
            select plotObj;

        plotList = plotObjectQuery.ToList();
    }


    // get a plot object from its grid position
    public PlotObject GetPlotObject(int xIndex, int zIndex)
    {
        return plotDict[xIndex * XVALUEMULTIPLIER + zIndex];
    }
}
