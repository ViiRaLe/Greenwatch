using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// SCUSA ALBERTOOOOOOOOOOOOOOOO
/// </summary>

public class MenuManager : MonoBehaviour
{
    public Sprite[] tutorialSprites;
    private int counterSprite;
    public GameObject tutorialRender;

    public GameObject noEnter;

    public GameObject hudSelection;

    public GameObject mainMenu;
    public GameObject singlePlayerMenu;
    public GameObject multiPlayerMenu;
    public GameObject tutorialMenu;
    public GameObject creditsMenu;
    public GameObject exitPost;

    public GameObject boySingle, girlSingle;

    public GameObject boyP1, boyP2, girlP1, girlP2;
    public GameObject arrowLeft, arrowRight, arrowLeft1, arrowLeft2, arrowRight1, arrowRight2, arrowLeft3, arrowRight3;
    public GameObject P1ReadyIcon, P2ReadyIcon;

    [SerializeField]
    private Camera cameraMain;
    private bool noEnterBool = false;

    private bool p1Ready = false;
    private bool p2Ready = false;

    private bool levelLoad = false;

    private EventSystem eventSystem;



    private void Awake()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    

    private void Update()
    {
        if (cameraMain.GetComponent<Animation>().isPlaying)
        {
            //InputManager.Instance.Activate(false);
            NullAxis();
            return;
        }
        else
        {
            // initialize main menu first button
            if (noEnterBool && eventSystem.currentSelectedGameObject == null && mainMenu.activeSelf)
            {
                SetButtonUI("SinglePlayer");
            }

            // main menu to title
            //if (InputManager.Instance.CancelButton && !singlePlayerMenu.activeSelf && !multiPlayerMenu.activeSelf && !creditsMenu.activeSelf && !tutorialMenu.activeSelf && noEnter.activeSelf)
            if (Input.GetButtonDown("Cancel") && !exitPost.activeSelf && !singlePlayerMenu.activeSelf && !multiPlayerMenu.activeSelf && !creditsMenu.activeSelf && !tutorialMenu.activeSelf && noEnter.activeSelf)
            {
                ReverseAnim("cameraAnim");
                cameraMain.GetComponent<Animation>().Play("cameraAnim");
                noEnter.SetActive(false);
                noEnterBool = false;
                mainMenu.SetActive(false);
            }

            #region AXIS RESET /// VARIOUS MENU STATES / INTERACTIONS

            //InputManager.Instance.Activate(true);
            ResetAxis();
            
            if (exitPost.activeSelf && eventSystem.currentSelectedGameObject == null)
            {
                SetButtonUI("NoButton");
            }

            // single player menu
            if (singlePlayerMenu.activeSelf)
            {
                // set player count
                if (GameManager.Instance.GameState.playerCount != 1)
                {
                    GameManager.Instance.GameState.SetPlayerCount(1);
                }

                //change inputs
                eventSystem.GetComponent<StandaloneInputModule>().submitButton = "NullAxis";

                BlinkArrow(arrowLeft, true);
                BlinkArrow(arrowRight, true);

                if (Input.GetButtonDown("UIHorizontal Left"))
                {
                    boySingle.SetActive(!boySingle.activeSelf);
                    girlSingle.SetActive(!girlSingle.activeSelf);

                    BlinkArrow(arrowLeft, false);

                    Debug.Log("MULTI PREMO A");
                }
                else if (Input.GetButtonDown("UIHorizontal Right"))
                {
                    boySingle.SetActive(!boySingle.activeSelf);
                    girlSingle.SetActive(!girlSingle.activeSelf);

                    BlinkArrow(arrowRight, false);

                    Debug.Log("MULTI PREMO D");
                }

                if (Input.GetButtonDown("FireP1"))
                {
                    // set player gender
                    bool sex = boySingle.activeSelf;
                    if (sex)
                    {
                        GameManager.Instance.GameState.SetGender(1, Gender.Boy);
                    }
                    else
                    {
                        GameManager.Instance.GameState.SetGender(1, Gender.Girl);
                    }

                    if (levelLoad == false)
                    {
                        levelLoad = true;
                        SceneLoader.Instance.LoadSceneAsync(3);
                    }
                }

                if (Input.GetButtonDown("Cancel"))
                {
                    BackToMenu("SinglePlayerAnim");
                }
            }

            // multi player menu
            if (multiPlayerMenu.activeSelf)
            {
                // set player count
                if (GameManager.Instance.GameState.playerCount != 2)
                {
                    GameManager.Instance.GameState.SetPlayerCount(2);
                }

                //change inputs
                eventSystem.GetComponent<StandaloneInputModule>().submitButton = "NullAxis";

                BlinkArrow(arrowLeft1, true);
                BlinkArrow(arrowRight1, true);
                BlinkArrow(arrowLeft2, true);
                BlinkArrow(arrowRight2, true);

                if (p1Ready && p2Ready && levelLoad == false)
                {
                    levelLoad = true;
                    SceneLoader.Instance.LoadSceneAsync(3);
                }

                // set player genders
                //CONFIRM P1 e P2
                if (Input.GetButtonDown("FireP1"))
                {
                    bool sex = boyP1.activeSelf;
                    if (sex)
                    {
                        GameManager.Instance.GameState.SetGender(1, Gender.Boy);
                    }
                    else
                    {
                        GameManager.Instance.GameState.SetGender(1, Gender.Girl);
                    }
                    p1Ready = !p1Ready;
                    P1ReadyIcon.SetActive(p1Ready);
                    Debug.Log("XD MULTI P1");
                }

                if (Input.GetButtonDown("FireP2"))
                {
                    bool sex = boyP2.activeSelf;
                    if (sex)
                    {
                        GameManager.Instance.GameState.SetGender(2, Gender.Boy);
                    }
                    else
                    {
                        GameManager.Instance.GameState.SetGender(2, Gender.Girl);
                    }

                    p2Ready = !p2Ready;
                    P2ReadyIcon.SetActive(p2Ready);
                }


                //INPUT PLAYER 1
                if (!p1Ready)
                {
                    if (Input.GetButtonDown("UIHorizontal Left"))
                    {
                        boyP1.SetActive(!boyP1.activeSelf);
                        girlP1.SetActive(!girlP1.activeSelf);

                        BlinkArrow(arrowLeft1, false);
                    }
                    else if (Input.GetButtonDown("UIHorizontal Right"))
                    {
                        boyP1.SetActive(!boyP1.activeSelf);
                        girlP1.SetActive(!girlP1.activeSelf);

                        BlinkArrow(arrowRight1, false);
                    }
                }
                


                //INPUT PLAYER 2
                if (!p2Ready)
                {
                    if (Input.GetButtonDown("UIHorizontal Sinistra"))
                    {
                        boyP2.SetActive(!boyP2.activeSelf);
                        girlP2.SetActive(!girlP2.activeSelf);

                        BlinkArrow(arrowLeft2, false);
                    }
                    else if (Input.GetButtonDown("UIHorizontal Destra"))
                    {
                        boyP2.SetActive(!boyP2.activeSelf);
                        girlP2.SetActive(!girlP2.activeSelf);

                        BlinkArrow(arrowRight2, false);
                    }

                    if (Input.GetButtonDown("Cancel"))
                    {
                        BackToMenu("MultiPlayerAnim");
                    }
                }
                
            }

            if (tutorialMenu.activeSelf)
            {
                BlinkArrow(arrowLeft3, true);
                BlinkArrow(arrowRight3, true);
                
                //change inputs
                eventSystem.GetComponent<StandaloneInputModule>().submitButton = "NullAxis";

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

                    BlinkArrow(arrowLeft3, false);

                    Debug.Log("TUTORIAL PREMO A");
                }
                else if (Input.GetButtonDown("UIHorizontal Right"))
                {
                    counterSprite += 1;

                    BlinkArrow(arrowRight3, false);

                    Debug.Log("TUTORIAL PREMO D");
                }

                if (Input.GetButtonDown("Cancel"))
                {
                    BackToMenu("TutorialAnim");
                }
            }

            if (creditsMenu.activeSelf)
            {
                if (Input.GetButtonDown("FireP1"))
                {
                    Debug.Log("XD CREDITS");
                }

                if (Input.GetButtonDown("Cancel"))
                {
                    BackToMenu("CreditsAnim");
                }
            }

            #endregion
        }

        //First camera move 
        if (Input.GetButtonDown("MenuFire") && !noEnterBool)
        {
            cameraMain.GetComponent<Animation>().Play("cameraAnim");
            noEnter.SetActive(true);
            noEnterBool = true;
            mainMenu.SetActive(true);
            DeactivateHUD();

            //set first interaction
            SetButtonUI("SinglePlayer");
        }

        

        #region ANIMATION RESETTERS


        if (!(cameraMain.GetComponent<Animation>().IsPlaying("cameraAnim")))
        {
            ResetAnim("cameraAnim");
        }

        if (!(cameraMain.GetComponent<Animation>().IsPlaying("SinglePlayerAnim")))
        {
            ResetAnim("SinglePlayerAnim");
        }

        if (!(cameraMain.GetComponent<Animation>().IsPlaying("MultiPlayerAnim")))
        {
            ResetAnim("MultiPlayerAnim");
        }

        if (!(cameraMain.GetComponent<Animation>().IsPlaying("TutorialAnim")))
        {
            ResetAnim("TutorialAnim");
        }

        if (!(cameraMain.GetComponent<Animation>().IsPlaying("CreditsAnim")))
        {
            ResetAnim("CreditsAnim");
        }

        #endregion
    }


    public void SinglePlayerButton()
    {
        cameraMain.GetComponent<Animation>().Play("SinglePlayerAnim");

        //change ui buttons
        DeactivateCanvas();
        DeactivateHUD();
        hudSelection.SetActive(true);
        singlePlayerMenu.SetActive(true);
      
    }

    public void MultiPlayerButton()
    {
        cameraMain.GetComponent<Animation>().Play("MultiPlayerAnim");

        //change ui buttons
        DeactivateCanvas();
        DeactivateHUD();
        hudSelection.SetActive(true);
        multiPlayerMenu.SetActive(true);
      
    }

    public void TutorialButton()
    {
        cameraMain.GetComponent<Animation>().Play("TutorialAnim");

        //change ui buttons
        DeactivateCanvas();
        DeactivateHUD();
        hudSelection.SetActive(true);
        tutorialMenu.SetActive(true);
        counterSprite = 0;
    }

    public void CreditsButton()
    {
        cameraMain.GetComponent<Animation>().Play("CreditsAnim");

        //change ui buttons
        DeactivateCanvas();
        DeactivateHUD();
        hudSelection.SetActive(true);
        creditsMenu.SetActive(true);
    }

    public void EscapeButton()
    {
        DeactivateCanvas();
        DeactivateHUD();
        exitPost.SetActive(true);
        SetButtonUI("NoButton");
    }

    public void escYes()
    {
        Application.Quit();
    }

    public void escNo()
    {
        DeactivateCanvas();
        DeactivateHUD();
        mainMenu.SetActive(true);
        SetButtonUI("SinglePlayer");
    }

    private void DeactivateHUD()
    {
        hudSelection.SetActive(false);
    }

    private void DeactivateCanvas()
    {
        mainMenu.SetActive(false);
        singlePlayerMenu.SetActive(false);
        multiPlayerMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        creditsMenu.SetActive(false);
        exitPost.SetActive(false);
    }


    public void BackToMenu(string currentState)
    {
        ReverseAnim(currentState);
        cameraMain.GetComponent<Animation>().Play(currentState);

        DeactivateCanvas();
        DeactivateHUD();
        mainMenu.SetActive(true);

        SetButtonUI("SinglePlayer");

        ResetAxis();
    }

    private void NullAxis()
    {
        eventSystem.GetComponent<StandaloneInputModule>().submitButton = "NullAxis";
        eventSystem.GetComponent<StandaloneInputModule>().verticalAxis = "NullAxis";
        eventSystem.GetComponent<StandaloneInputModule>().horizontalAxis = "NullAxis";
    }

    private void ResetAxis()
    {
        eventSystem.GetComponent<StandaloneInputModule>().submitButton = "MenuFire";
        eventSystem.GetComponent<StandaloneInputModule>().verticalAxis = "MenuVertical";
        eventSystem.GetComponent<StandaloneInputModule>().horizontalAxis = "MenuHorizontal";
    }

    private void SetButtonUI(string button)
    {
        eventSystem.SetSelectedGameObject(null);
        Button btn = GameObject.Find(button).GetComponent<Button>();
        eventSystem.SetSelectedGameObject(btn.gameObject, null);
        btn.Select();
    }

    private void ReverseAnim(string animation)
    {
        Animation anim = cameraMain.GetComponent<Animation>();

        if (anim[animation].speed == 1)
        {
            anim[animation].speed = -1;
            anim[animation].time = anim[animation].length;
        }
        
    }

    private void ResetAnim(string animation)
    {
        Animation anim = cameraMain.GetComponent<Animation>();

        if (anim[animation].speed == -1)
        {
            anim[animation].speed = 1;
            anim[animation].time = 0;
        }
    }

    private void BlinkArrow(GameObject arrow, bool blink)
    {
        arrow.SetActive(blink);
    }

}
