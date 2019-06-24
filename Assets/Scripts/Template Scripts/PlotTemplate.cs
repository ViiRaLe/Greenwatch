using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Plot Template", menuName = "Green Watch/Plot Template")]
public class PlotTemplate : ScriptableObject
{
    public GameObject thisPlot;
    public ItemTemplate interactableItem;
    public GameObject interactedPlot;
    public int addScore;
    public int removeScore;
    public GameObject aiPlot;

    [Space, Header("Artificial Intelligence Sound", order = 0)]
    public AudioClip aiAudioClip;
    public bool aiClipLoop = false;
    public int aiClipPriority = 128;
    public float aiClipVolume = 1;
    [Min(0.75f)]
    public float aiClipPitchMin = 0.85f;
    [Min(1)]
    public float aiClipPitchMax = 1.15f;

    [Space, Header("Particles", order = 0)]
    public GameObject interactParticlesPrefab;
    public GameObject aiParticlesPrefab;
}
