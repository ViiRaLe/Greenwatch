using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private PauseManager pauseManager;

    [SerializeField]
    private GameTimer gameTimer;

    [SerializeField]
    private Text timerText;

    [SerializeField]
    private Text scoreText;



    private void Awake()
    {
        DisableCanvas();
    }

    // register to ui events
    public void OnEnable()
    {
        gameTimer.RegisterOnCountdownEnd(EnableCanvas);
        gameTimer.RegisterOnTimerUpdate(UpdateTimer);
        pauseManager.RegisterOnPause(CanvasSetActive);
        gameTimer.RegisterOnGameEnd(DisableCanvas);
        GameManager.Instance.RegisterOnChangeScore(UpdateScore);
    }

    // unregister from ui events
    public void OnDisable()
    {
        gameTimer.UnregisterOnCountdownEnd(EnableCanvas);
        gameTimer.UnregisterOnTimerUpdate(UpdateTimer);
        gameTimer.UnregisterOnGameEnd(DisableCanvas);
        pauseManager.UnregisterOnPause(CanvasSetActive);
        GameManager.Instance.UnregisterOnChangeScore(UpdateScore);
    }


    // update score text
    private void UpdateScore(int score)
    {
        scoreText.text = "Alberi: " + '\n' + "   " + score.ToString();
    }

    // update timer text
    private void UpdateTimer(int minutes, int seconds)
    {
        timerText.text = string.Format("Tempo:" + '\n' + "{0}:{1}", minutes, seconds.ToString("00"));
    }


    // enable gameplay hud
    private void EnableCanvas()
    {
        timerText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
    }

    // disable gameplay hud
    private void DisableCanvas()
    {
        timerText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
    }


    // activate/deactivate gameplay hud
    private void CanvasSetActive(bool enable)
    {
        timerText.gameObject.SetActive(enable);
        scoreText.gameObject.SetActive(enable);
    }
}
