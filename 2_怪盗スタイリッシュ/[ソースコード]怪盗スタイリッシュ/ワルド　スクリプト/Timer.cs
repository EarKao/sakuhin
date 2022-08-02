using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] Text countdownTimerUI;
    [SerializeField] public float secondsLeft = 100;
    [SerializeField] public float timeLeftEffector = 1.0f; // The lesser the number the slower the timer
    [SerializeField] private float bgmSpeed = 1.5f;
    [SerializeField] private string LoadSceneName;


    [SerializeField] public float firstColorChange = 30f;
    [SerializeField] public float secondColorChange = 5f;

    [SerializeField] private AudioSource bgm;

    private float deltaTime = 1;
    private bool isTimerActive;

    
    void Start()
    {
        isTimerActive = true;
        countdownTimerUI.text = secondsLeft.ToString();
    }
    
    void Update()
    {
        TimerCountdown();
    }

    void TimerCountdown()
    {
        if (isTimerActive)
        {
            //           0.16 per frame * 1.0 (Default)
            deltaTime -= Time.deltaTime * timeLeftEffector;

            if (deltaTime <= 0)
            {
                deltaTime = 1;
                secondsLeft -= 1; //31

                ColorChange(secondsLeft);

                countdownTimerUI.text = secondsLeft.ToString();

                StageFailChecker();
            }
        }
    }

    void StageFailChecker()
    {
        if (secondsLeft <= 0)
        {
            SceneManager.LoadScene(LoadSceneName);
        }
    }

    void ColorChange(float _secondsLeft)
    {
        if (_secondsLeft <= secondColorChange) // If less than 10, change color to Red
        {
            countdownTimerUI.color = new Color(255, 0, 0);

        } else if (_secondsLeft <= firstColorChange) // If less than 30, change color to Orange
        {
            countdownTimerUI.color = new Color(255, 255, 0);
            bgm.pitch = bgmSpeed;
        }
    }
}