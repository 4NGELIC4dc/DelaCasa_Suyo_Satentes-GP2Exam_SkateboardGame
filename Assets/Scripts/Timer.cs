using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    [SerializeField] GameOver gameOver;

    // Update is called once per frame
    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else
        {
            remainingTime = 0;
            TimesUp();
        }
        
        timerText.text = "Time: " + Mathf.CeilToInt(remainingTime).ToString();
    }

    void TimesUp()
    {
        if (gameOver != null)
        {
            gameOver.GameEnd();
        }
    }
}
