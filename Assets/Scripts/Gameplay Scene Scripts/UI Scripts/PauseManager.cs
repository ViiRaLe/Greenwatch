using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
{
    //TUTORIAL
    public Sprite[] tutorialSprites;
    private int counterSprite;
    public GameObject tutorialRender;
    public GameObject tutorialPanel;
    //end


    public GameObject checkPanel;

    public EventSystem eventSystem;
    private int command = 0;

    public event Action<bool> OnPause;

    private void Start()
    {
        Debug.Log("START PARTE");
        SetButtonUI("Resume");
    }

    private void Update()
    {
        #region Tutorial branch

        if (counterSprite < 0)
        {
            counterSprite = 3;
        }
        else if (counterSprite > 3)
        {
            counterSprite = 0;
        }

        if (tutorialRender.GetComponent<Image>().sprite != tutorialSprites[counterSprite])
        {
            tutorialRender.GetComponent<Image>().sprite = tutorialSprites[counterSprite];
        }


        if (Input.GetButtonDown("UIHorizontal Left"))
        {
            counterSprite -= 1;
            
            Debug.Log("TUTORIAL PREMO A");
        }
        else if (Input.GetButtonDown("UIHorizontal Right"))
        {
            counterSprite += 1;

            Debug.Log("TUTORIAL PREMO D");
        }

        #endregion

        if (eventSystem.currentSelectedGameObject == null)
        {
            if (checkPanel.activeSelf)
            {
                SetButtonUI("No");
            }
            else if (!tutorialPanel.activeSelf)
            {
                SetButtonUI("Resume");
            }
        }

        if (Input.GetButtonDown("Cancel") && !tutorialPanel.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else if(Input.GetButtonDown("Cancel") && tutorialPanel.activeSelf)
        {
            tutorialPanel.SetActive(false);
        }
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
        command = 0;
        InputManager.Instance.Activate(false);
        OnPause(!true);
    }

    private void OnDisable()
    {
        Time.timeScale = 1.0f;
        OnPause(!false);
        SetCheckPanel(false);
        tutorialPanel.SetActive(false);
        eventSystem.SetSelectedGameObject(null);
        InputManager.Instance.Activate(true);
    }

    public void OnResume()
    {
        tutorialPanel.SetActive(false);
        gameObject.SetActive(false);        
    }
    
    public void OnRestart()
    {
        SetCheckPanel(true);
        SetButtonUI("No");
        command = 1;
    }

    public void OnTutorial()
    {
        eventSystem.SetSelectedGameObject(null);
        tutorialPanel.SetActive(true);
        counterSprite = 0;
        command = 2;  //useless
    }

    public void OnMenu()
    {
        SetCheckPanel(true);
        SetButtonUI("No");
        command = 3;
    }

    public void CheckYes()
    {
        switch (command)
        {
            case 1: //RESTART
                {
                    eventSystem.SetSelectedGameObject(null);
                    Time.timeScale = 1.0f;
                    SceneLoader.Instance.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
                    break;
                }

            case 2: //TUTORIAL  useless
                {
                    eventSystem.SetSelectedGameObject(null);
                    tutorialPanel.SetActive(true);
                    break;
                }

            case 3: //MENU
                {
                    eventSystem.SetSelectedGameObject(null);
                    Time.timeScale = 1.0f;
                    SceneLoader.Instance.LoadSceneAsync(SceneLoader.Instance.StartingSceneIndex);
                    break;
                }

            default:
                {
                    break;
                }
        }
    }

    public void CheckNo()
    {
        SetCheckPanel(false);
        SetButtonUI("Resume");
        command = 0;
    }

    private void SetCheckPanel(bool active)
    {
        checkPanel.SetActive(active);
    }

    private void SetButtonUI(string button)
    {
        eventSystem.SetSelectedGameObject(null);
        Button btn = GameObject.Find(button).GetComponent<Button>();
        eventSystem.SetSelectedGameObject(btn.gameObject, null);
        btn.Select();
    }


    // register a method to on pause event
    public void RegisterOnPause(Action<bool> action)
    {
        OnPause += action;
    }

    // unregister a method to on pause event
    public void UnregisterOnPause(Action<bool> action)
    {
        OnPause -= action;
    }
}
