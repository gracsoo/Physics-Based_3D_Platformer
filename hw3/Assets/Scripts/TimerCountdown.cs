using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerCountdown : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public ThirdPersonController controller;
    public float timeLimit = 2;

    void Awake()
    {
       
    }
    
    void Update()
    {
        timeLimit -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(timeLimit / 60F);
        int seconds = Mathf.FloorToInt(timeLimit % 60F);
        textDisplay.text = minutes.ToString ("00") + ":" + seconds.ToString ("00");

        if(minutes == 0 && seconds == 0)
        {
            controller.timerRunning = false;
            textDisplay.enabled = false;
        }
    }
}
