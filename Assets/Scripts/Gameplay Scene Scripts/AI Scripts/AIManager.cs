using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    private GameTimer gameTimer;



    private void Awake()
    {
        gameTimer = FindObjectOfType(typeof(GameTimer)) as GameTimer;
    }

    private void OnEnable()
    {
        gameTimer.OnPreparationEnd += EnableChildren;
        gameTimer.OnGameEnd += DisableChildren;
    }

    private void OnDisable()
    {
        gameTimer.OnPreparationEnd -= EnableChildren;
        gameTimer.OnGameEnd -= DisableChildren;
    }

    private void EnableChildren()
    {
        for (int i = 0; i < transform.childCount; i += 1)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        gameTimer.OnPreparationEnd -= EnableChildren;
    }

    private void DisableChildren()
    {
        for (int i = 0; i < transform.childCount; i += 1)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        gameTimer.OnGameEnd -= DisableChildren;
    }
}
