using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField]
    private Color endingColor;

    private float levelTimeTotal = 0;
    private float levelPreparationTime = 0;

    private bool ending = false;
    private bool gameStarted = false;
    private bool timerStarted = false;

    private float currentTime;
    private int minutes;
    private int seconds;

    public event Action OnCountdownEnd;
    public event Action OnPreparationEnd;
    public event Action OnGameEnd;
    public event Action<int, int> OnTimerUpdate;

    [Header("Sound"), SerializeField]
    private AudioClip endGameClip;
    [SerializeField]
    private bool endGameClipLoop = false;
    [SerializeField]
    private int endGameClipPriority = 128;
    [SerializeField]
    private float endGameClipVolume = 1;
    [SerializeField, Min(0.75f)]
    private float endGameClipPitchMin = 0.95f;
    [SerializeField,  Min(1)]
    private float endGameClipPitchMax = 1.05f;


    [Space, Header("Countdown Timer")]
    public Text startingTimer;
    public float countdownTimer;
    private float second;
    [SerializeField]
    private string startingText = "VIA!";



    public float CurrentTimePercentage
    {
        get
        {
            return (currentTime) / levelTimeTotal;
        }
    }



    private void Awake()
    {
        // initialize timer
        ResetTimer();
    }

    private void Update()
    {
        if (gameStarted == false)
        {
            if (countdownTimer > 0.5f)
            {
                InputManager.Instance.Activate(false);
                countdownTimer -= Time.deltaTime;
                second = countdownTimer % 60;
                startingTimer.text = string.Format("{0:0}", second);
            }
            else
            {
                startingTimer.text = startingText;
                countdownTimer -= Time.deltaTime;

                if (countdownTimer < -0.5f)
                {
                    OnCountdownEnd?.Invoke();
                    InputManager.Instance.Activate(true);
                    timerStarted = true;
                    gameStarted = true;
                }
            }
        }

        // if timer is started decrease it by time passed
        if (timerStarted == true)
        {
            //Destroy(startingTimer, 0.5f);

            if (levelPreparationTime > 0)
            {
                levelPreparationTime -= Time.deltaTime;
            }
            else
            {
                OnPreparationEnd?.Invoke();
            }

            currentTime -= Time.deltaTime;
            CalculateTime();
            OnTimerUpdate?.Invoke(minutes, seconds);

            // if countdown arrived to 0 send timer ended message and reset timer
            if (currentTime <= 0)
            {
                SoundManager.Instance.PlayAudioClip(endGameClip, false, endGameClipLoop, endGameClipPriority, endGameClipVolume,
                    UnityEngine.Random.Range(endGameClipPitchMin, endGameClipPitchMax));

                timerStarted = false;
                OnGameEnd?.Invoke();
            }

            if (levelTimeTotal - currentTime <= 0.5f)
            {
                startingTimer.gameObject.SetActive(false);
            }
            
        }
    }

    // reset countdown timer
    public void ResetTimer()
    {
        timerStarted = false;
        levelTimeTotal = GameManager.Instance.LevelTime;
        levelPreparationTime = GameManager.Instance.PreparationTime;
        currentTime = levelTimeTotal;
        CalculateTime();
    }

    // calculate minutes and seconds
    private void CalculateTime()
    {
        if (currentTime > 0)
        {
            minutes = Mathf.FloorToInt(currentTime / 60);
            seconds = (int)currentTime - (minutes * 60);
        }
        else
        {
            minutes = 0;
            seconds = 0;
            Destroy(startingTimer.gameObject);
        }
        
        if (currentTime <= 11f)
        {
            if (!ending)
            {
                ending = true;
                startingTimer.gameObject.SetActive(true);
                startingTimer.fontSize = startingTimer.fontSize * 1 / 3;
                startingTimer.color = endingColor;
            }

            
            startingTimer.text = seconds.ToString();
        }

    }


    // register a method to on game end event
    public void RegisterOnGameEnd(Action action)
    {
        OnGameEnd += action;
    }

    // unregister a method to on game end event
    public void UnregisterOnGameEnd(Action action)
    {
        OnGameEnd -= action;
    }


    // register a method to on countdown end event
    public void RegisterOnCountdownEnd(Action action)
    {
        OnCountdownEnd += action;
    }

    // unregister a method to on countdown end event
    public void UnregisterOnCountdownEnd(Action action)
    {
        OnCountdownEnd -= action;
    }


    // register a method to on game end event
    public void RegisterOnTimerUpdate(Action<int, int> action)
    {
        OnTimerUpdate += action;
        OnTimerUpdate?.Invoke(minutes, seconds);
    }

    // unregister a method to on game end event
    public void UnregisterOnTimerUpdate(Action<int, int> action)
    {
        OnTimerUpdate?.Invoke(minutes, seconds);
        OnTimerUpdate -= action;
    }
}
