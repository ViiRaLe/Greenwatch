using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotObject : MonoBehaviour
{
    protected PlotGrid grid;
    protected int xValue;
    protected int zValue;

    [SerializeField]
    private PlotTemplate plotTemplate;

    private bool underAttack;

    [SerializeField]
    private GameObject[] skins;
    private int skinIndex = -1;

    private Renderer rend;
    private MaterialPropertyBlock highlightBlock;
    //[SerializeField]
    //private Color highlightColor;
    [SerializeField]
    private Texture highlightTexture;
    [SerializeField]
    private float flickerDelay = 0.3f;

    [SerializeField]
    protected float particlesOffset = 0.2f;



    [HideInInspector]
    public PlotObject EnemyPlot { get; set; }

    public PlotTemplate PlotTemplate
    {
        get
        {
            return plotTemplate;
        }
    }

    public PlotGrid Grid
    {
        get
        {
            return grid;
        }
    }

    public int XValue
    {
        get
        {
            return xValue;
        }
    }

    public int ZValue
    {
        get
        {
            return zValue;
        }
    }

    public int SkinIndex
    {
        get
        {
            return skinIndex;
        }
    }

    public bool UnderAttack
    {
        get
        {
            return underAttack;
        }
    }



    private void Awake()
    {
        rend = GetComponent<Renderer>();

        highlightBlock = new MaterialPropertyBlock();
        rend.GetPropertyBlock(highlightBlock);
        //highlightBlock.SetColor("_Color", highlightColor);

        if (highlightTexture != null)
        {
            highlightBlock.SetTexture("_MainTex", highlightTexture);
        }
    }

    // initialize plot values
    public void Initialize(PlotGrid grid, int xVal, int zVal, int index = -1)
    {
        this.grid = grid;
        this.xValue = xVal;
        this.zValue = zVal;

        if (skins.Length > 0)
        {
            if (index >= 0)
            {
                skinIndex = index;
            }
            else
            {
                // randomize skin index
                skinIndex = Random.Range(0, skins.Length);
            }

            transform.GetChild(skinIndex).gameObject.SetActive(true);
        }
    }

    public override string ToString()
    {
        return string.Format("Tile ({0}, {1}, {2}, {3})", xValue, zValue, plotTemplate, EnemyPlot);
    }

    public IEnumerator AttackPlot(float delay)
    {
        underAttack = true;

        StartCoroutine(Highlight());

        yield return new WaitForSeconds(delay);

        if (plotTemplate.aiAudioClip != null)
        {
            SoundManager.Instance.PlayAudioClip(plotTemplate.aiAudioClip, true, plotTemplate.aiClipLoop, plotTemplate.aiClipPriority,
                plotTemplate.aiClipVolume, Random.Range(plotTemplate.aiClipPitchMin, plotTemplate.aiClipPitchMax));
        }

        if (plotTemplate.aiParticlesPrefab != null)
        {
            Instantiate(plotTemplate.aiParticlesPrefab, new Vector3(transform.position.x, transform.position.y + particlesOffset, transform.position.z), Quaternion.identity);
        }

        grid.ChangeTile(this, plotTemplate.aiPlot, -plotTemplate.removeScore);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator Highlight()
    {
        rend.SetPropertyBlock(highlightBlock);

        yield return new WaitForSeconds(flickerDelay);

        rend.SetPropertyBlock(null);

        yield return new WaitForSeconds(flickerDelay);

        StartCoroutine(Highlight());
    }

    private void Unlit()
    {
        rend.SetPropertyBlock(null);
    }
}
