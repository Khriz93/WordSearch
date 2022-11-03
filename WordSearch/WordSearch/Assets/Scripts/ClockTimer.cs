using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockTimer : MonoBehaviour
{
    public GameData currentGameData;
    public Text clockText;
    public GameOverScreen gameOverScreen;
    public GameObject continueGameAfterAdsButton;
    public float timeLeft;
    private float minutes;
    private float seconds;
    private float oneSecondDown;
    public bool timeFinish;
    public bool stopTimer;



    // Start is called before the first frame update
    void Start()
    {
        stopTimer = false;
        timeFinish = false;
        timeLeft = currentGameData.selectedBoardData.timeInSeconds;
        oneSecondDown = timeLeft - 1f;

        GameEvents.OnBoardCompleted += StopTimer;
        GameEvents.OnUnlockNextCategory += StopTimer;
    }

    private void OnDisable()
    {
        GameEvents.OnBoardCompleted -= StopTimer;
        GameEvents.OnUnlockNextCategory -= StopTimer;
    }

    public void StopTimer()
    {
        stopTimer = true;
    }

    private void Update()
    {
        if (stopTimer == false)
        {
            timeLeft -= Time.deltaTime;

            if (timeLeft <= oneSecondDown)
            {
                oneSecondDown = timeLeft - 1f;
            }
        }
    }

    private void OnGUI()
    {
        if (timeFinish == false)
        {
            if (timeLeft > 0)
            {
                minutes = Mathf.Floor(timeLeft / 60);
                seconds = Mathf.RoundToInt(timeLeft % 60);

                clockText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
            }
            else
            {
                stopTimer = true;
                ActivateGameOverGUI();
            }
        }
    }

    private void ActivateGameOverGUI()
    {
        GameEvents.GameOverMethod();
        SoundManager.instance.SilienceBackgroundMusic(true);
        timeFinish = true;
    }


}
