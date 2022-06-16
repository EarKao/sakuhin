using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GoalColliders : MonoBehaviour
{
	[SerializeField] Timer timer;

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.CompareTag("Player"))
		{
			SceneManager.sceneLoaded += SceneLoaded;
			SceneManager.LoadScene("Clear Scene");
		}
	}

	// イベントハンドラー（イベント発生時に動かしたい処理）
	void SceneLoaded(Scene nextScene, LoadSceneMode mode)
	{
		float r_second = timer.secondsLeft % 60; // 85 % 60 = 25
		float r_minute = (timer.secondsLeft - (timer.secondsLeft % 60)) / 60;

		Text clearTime = GameObject.Find("time01").GetComponent<Text>();

		clearTime.text = r_minute.ToString() + ":" + r_second.ToString();

		SceneManager.sceneLoaded -= SceneLoaded;
	}
}
