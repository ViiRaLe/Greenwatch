using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public GameObject[] aiPrefabs;
    public Transform[] playerPositions;
    public GameTimer gameTimer;

    public GameObject pauseMenu;
    public GameObject endGameMenu;



    private void OnEnable()
    {
        SceneLoader.Instance.RegisterOnEventAction(SceneLoader.Instance.OnGameSceneLoaded, Initialize);
        gameTimer.RegisterOnGameEnd(EndGame);
    }

    private void OnDisable()
    {
        SceneLoader.Instance.UnregisterOnEventAction(SceneLoader.Instance.OnGameSceneLoaded, Initialize);
        gameTimer.UnregisterOnGameEnd(EndGame);
    }


    private void Update()
    {
        // activate pause menu
        if (InputManager.Instance.CancelButton)
        {
            pauseMenu.SetActive(true);
        }
    }

    // initialize game players
    private void Initialize()
    {
        GameState gs = GameManager.Instance.GameState;

        for (int i = 0; i < gs.playerCount; i += 1)
        {
            PlayerController pc = Instantiate(playerPrefabs[(int)gs.gender[i]], playerPositions[i].position, Quaternion.identity).GetComponent<PlayerController>();
            pc.SetNumber(i);
        }

        Instantiate(aiPrefabs[gs.playerCount - 1]);
    }

    // enable end game
    public void EndGame()
    {
        InputManager.Instance.Activate(false);
        endGameMenu.SetActive(true);
        Time.timeScale = 0;
    }
}
