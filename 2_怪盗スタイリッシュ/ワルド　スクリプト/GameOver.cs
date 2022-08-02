using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] Button gameoverYes;
    [SerializeField] Button gameoverNo;

	private void Start()
	{
		gameoverYes.onClick.AddListener(ClickedYes);
		gameoverNo.onClick.AddListener(ClickedNo);
	}
	
	void ClickedYes()
	{
		SceneManager.LoadScene("Main Scene");
	}

	void ClickedNo()
	{
		SceneManager.LoadScene("Title Scene");
	}
}