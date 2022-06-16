using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;�@�@�@//�{�^�����g�p����̂�UI
using UnityEngine.SceneManagement;//SceneManager���g�p����SceneManagement��ǉ�

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