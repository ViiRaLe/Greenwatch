using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InfoTextManager : MonoBehaviour
{
    [TextArea]
    public string[ ] infoBox;
    public Text infoText;
    [SerializeField]
    private Text loadingText;
    [SerializeField]
    private GameObject loadingParticles;
    [SerializeField]
    private string loadingFinishedText = "Premi \"Spazio\" per continuare";



    private void Awake()
    {
        infoText.text = infoBox[ Random.Range (0, infoBox.Length) ];
    }


    private void OnEnable()
    {
        SceneLoader.Instance.RegisterOnEventAction(SceneLoader.Instance.OnSceneLoaded, PressSpace);
    }

    private void OnDisable()
    {
        SceneLoader.Instance.UnregisterOnEventAction(SceneLoader.Instance.OnSceneLoaded, PressSpace);
    }

    // change text to press space and continue
    private void PressSpace()
    {
        loadingParticles.SetActive(false);
        loadingText.text = loadingFinishedText;
    }
}
