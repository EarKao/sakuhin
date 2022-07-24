using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleButtons : MonoBehaviour
{
    [SerializeField] Button StartButton;
    [SerializeField] Button TutorialButton;
    [SerializeField] Button QuitButton;


    public void ClickedStart()
	{
		Invoke("Start_Main", 1f);
	}

    public void ClickedTutorial()
	{
		Invoke("Start_Tutorial", 0f);
	}

    public void ClickedQuit()
	{
		Invoke("Start_QuitGame", 0f);
	}

	void Start_Main()
	{
		SceneManager.LoadScene("Main Scene");
	}

	void Start_Tutorial()
	{
		SceneManager.LoadScene("Tutorial Scene");
	}

	void Start_QuitGame()
	{
		Application.Quit();
	}
}
