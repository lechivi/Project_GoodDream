using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerRemainCtrl : MonoBehaviour
{
    [SerializeField] private float timeRemaining = 60f;
    [SerializeField] private TMP_Text timeText;
    
    public bool TimeOut;

    private bool isTimerRunning = false;

    private void Update()
    {
        if (this.isTimerRunning)
        {
            if (this.timeRemaining > 0)
            {
                this.timeRemaining -= Time.deltaTime;
                this.DisplayTimer(this.timeRemaining);
            }
            else
            {
                if (AudioManager.HasInstance)
                {
                    AudioManager.Instance.PlaySFX(AUDIO.SFX_TIMEOUT);
                }

                this.timeText.SetText("Time to sleep!");
                this.timeRemaining = 0;
                this.isTimerRunning = false;
                this.TimeOut = true;
            }
        }
    }
    private void DisplayTimer(float timerToDisplay)
    {
        timerToDisplay += 1;
        float minutes = Mathf.FloorToInt(timerToDisplay / 60);
        float seconds = Mathf.FloorToInt(timerToDisplay % 60);
        this.timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void RunTime()
    {
        this.isTimerRunning = true;
    }

    public void PauseTime()
    {
        this.isTimerRunning = false;
    }
}
