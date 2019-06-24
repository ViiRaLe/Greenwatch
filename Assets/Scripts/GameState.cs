using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game State", menuName = "Green Watch/GameState Asset")]
public class GameState : ScriptableObject
{
    public int score; //// removable ////
    public int playerCount;
    public float levelTime;
    public float preparationTime;

    public Gender[] gender;



    public GameState()
    {
        score = 0;
        playerCount = 1;
        levelTime = 180;
        preparationTime = 6;
        gender = new Gender[] { Gender.Boy, Gender.Girl };
    }



    // set player count
    public void SetPlayerCount(int number)
    {
        Debug.Assert(number > 0);
        playerCount = number;
        gender = new Gender[playerCount];
    }

    // set selected gender
    public void SetGender(int number, Gender genderValue)
    {
        gender[number - 1] = genderValue;
    }

    // reset score
    public void ResetScore()
    {
        score = 0;
    }
}

public enum Gender
{
    Girl = 0, Boy = 1
}
