using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private event Action<int> OnScoreChanged;

    public GameState debugGameState;
    private GameState gameState;



    public float PreparationTime
    {
        get
        {
            return gameState.preparationTime;
        }
    }

    public float LevelTime
    {
        get
        {
            return gameState.levelTime;
        }
    }

    public int Score
    {
        get
        {
            return gameState.score;
        }
    }

    public GameState GameState
    {
        get
        {
            return gameState;
        }
    }



    protected override void Initialize()
    {
        base.Initialize();

        // create a game state if a debug game state isn't selected
        if (debugGameState != null)
        {
            //gameState = Instantiate(debugGameState);
            gameState = debugGameState;
        }
        else
        {
            gameState = ScriptableObject.CreateInstance<GameState>();
        }
    }

    // change score
    public void ChangeScore(int points)
    {
        gameState.score += points;
        OnScoreChanged?.Invoke(gameState.score);
    }

    // register a method to on change score event
    public void RegisterOnChangeScore(Action<int> action)
    {
        OnScoreChanged += action;
        OnScoreChanged?.Invoke(gameState.score);
    }

    // unregister a method to on change score event
    public void UnregisterOnChangeScore(Action<int> action)
    {
        OnScoreChanged?.Invoke(gameState.score);
        OnScoreChanged -= action;
    }
}
