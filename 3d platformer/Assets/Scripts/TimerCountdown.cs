using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerCountdown : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public int secondsLeft = 59;
    public bool takingAway = false;

    void Start()
    {
        textDisplay.GetComponent<TextMeshProUGUI>().text = "00:" + secondsLeft;
    }

    void Update()
    {
        if (takingAway == false && secondsLeft > 0)
            StartCoroutine(TimerTake());
    }

    IEnumerator TimerTake()
    {
        takingAway = true;
        yield return new WaitForSeconds(1);
        secondsLeft -= 1;
        textDisplay.GetComponent<TextMeshProUGUI>().text = "00:" + secondsLeft;
        takingAway = false;
    }
}