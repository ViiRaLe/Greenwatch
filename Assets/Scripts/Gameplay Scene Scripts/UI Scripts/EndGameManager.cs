using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EndGameManager : MonoBehaviour
{
    public EventSystem eventSystem;

    private int treeNumber = 0;
    private bool done = false;
    private bool timeCheck = false;
    private float cooldownTime = 0f;

    public Text treeText;
    public Text oxygenText;
    public Text titleText;
    public Text descriptionText;
    public Text oxygenProdText;
    public Text oxygenConsText;

    public Image medalImage;

    public Sprite[] medals;  //0 gold, 1 silver, 2 bronze
    public string[] nomination;
    public string[] description;
    private string nominationString;
    private string descriptionString;

    [SerializeField]
    private float delay = 2;

    private void Start()
    {
        SetButtonUI("EndBackMenu");

        treeNumber = GameManager.Instance.Score;

        CalculateThings(treeNumber);

        treeText.text = "Hai salvato " + treeNumber + " Alberi!";
        titleText.text = nominationString;
        descriptionText.text = descriptionString;
        oxygenText.text = treeNumber + " x 110 kg = " + treeNumber * 110 + " kg di ossigeno prodotti dal tuo boschetto in 1 anno." + '\n' + treeNumber * 110 + " kg : 290 kg = " + Mathf.RoundToInt(treeNumber * 110 / 290) + " persone"
             + '\n' + "Gli alberi che hai salvato produrranno ossigeno sufficiente per far respirare " + Mathf.RoundToInt(treeNumber * 110 / 290) + " persone!";
        oxygenProdText.text = "110 Kg = ossigeno prodotto da un albero in 1 anno.";
        oxygenConsText.text = "290 kg = ossigeno consumato da una persona in 1 anno.";
    }

    private void Update()
    {
        if (cooldownTime >= 1.8f && !timeCheck)
        {
            timeCheck = true;
        }
        else
        {
            cooldownTime += 0.01f;
        }

        if (eventSystem.currentSelectedGameObject == null && !done)
        {
            SetButtonUI("EndBackMenu");
        }
    }

    private void CalculateThings(int tree)
    {
        switch (treeNumber)
        {
            case int n when (n > 0 && n <= 20):  //sega
                {
                    nominationString = nomination[0];
                    descriptionString = description[0];
                    medalImage.sprite = medals[0];
                    break;
                }

            case int n when (n > 20 && n <= 30):  //pippa
                {
                    nominationString = nomination[1];
                    descriptionString = description[1];
                    medalImage.sprite = medals[1];
                    break;
                }

            case int n when (n > 30 && n <= 40): //scarso
                {
                    nominationString = nomination[2];
                    descriptionString = description[2];
                    medalImage.sprite = medals[2];
                    break;
                }

            case int n when (n > 40 && n <= 50): //normale
                {
                    nominationString = nomination[3];
                    descriptionString = description[3];
                    medalImage.sprite = medals[3];
                    break;
                }

            case int n when (n > 50 && n <= 60): //wow
                {
                    nominationString = nomination[4];
                    descriptionString = description[4];
                    medalImage.sprite = medals[4];
                    break;
                }

            case int n when (n > 60): //xd
                {
                    nominationString = nomination[5];
                    descriptionString = description[5];
                    medalImage.sprite = medals[5];
                    break;
                }

            default:
                {
                    nominationString = nomination[0];
                    descriptionString = description[0];
                    medalImage.sprite = medals[0];
                    break;
                }
        }
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
        InputManager.Instance.Activate(false);
    }

    private void OnDisable()
    {
        Time.timeScale = 1.0f;
        InputManager.Instance.Activate(true);

        eventSystem.SetSelectedGameObject(null);
    }



    public void OnRestart()
    {
        if (timeCheck)
        {
            eventSystem.SetSelectedGameObject(null);
            Time.timeScale = 1.0f;
            SceneLoader.Instance.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            done = true;
        }
    }

    public void OnMenu()
    {
        if (timeCheck)
        {
            eventSystem.SetSelectedGameObject(null);
            Time.timeScale = 1.0f;
            SceneLoader.Instance.LoadSceneAsync(SceneLoader.Instance.StartingSceneIndex);
            done = true;
        }
    }


    private void SetButtonUI(string button)
    {
        eventSystem.SetSelectedGameObject(null);
        Button btn = GameObject.Find(button).GetComponent<Button>();
        eventSystem.SetSelectedGameObject(btn.gameObject, null);
        btn.Select();
    }
}
