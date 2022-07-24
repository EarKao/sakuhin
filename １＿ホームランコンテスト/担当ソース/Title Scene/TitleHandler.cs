using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class TitleHandler : MonoBehaviour
{
	[SerializeField] Animator whitebackgroundAnimator;
	[SerializeField] Animator[] buttonAnimator;
	private int selectedTextNum = 0;

	[SerializeField] AudioSource titleBGM;
	[SerializeField] AudioSource onSelectionChangeSE;
	[SerializeField] AudioSource onSelectionEnterSE;

	private TitleAnimationHandler tAH;


	private void Start()
	{
		SetButtonAnimation();
		SetupReference();

		void SetupReference()
		{
			tAH = GetComponent<TitleAnimationHandler>();
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("PS4_DPadVertical") != 0)
		{
			if (selectedTextNum == 0) selectedTextNum = 1; else selectedTextNum = 0;
			onSelectionChangeSE.Play();
			SetButtonAnimation();
		}

		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button2))
		{
			onSelectionEnterSE.Play();
			if (selectedTextNum == 0) StartCoroutine(LoadMainScene());
			if (selectedTextNum == 1) StartCoroutine(LoadRankingScene());
		}
	}

	IEnumerator LoadMainScene()
	{
		whitebackgroundAnimator.Play("Whiteground_Exit");
		StartCoroutine(LowerMusic());
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene("CharacterSelect_Scene");

		IEnumerator LowerMusic()
		{
			while (true)
			{
				titleBGM.volume -= 0.05f;
				yield return new WaitForSeconds(0.1f);
			}
		}
	}

	IEnumerator LoadRankingScene()
	{
		whitebackgroundAnimator.Play("Whiteground_Exit");
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene("Ranking_Scene");
	}

	public void SetButtonAnimation()
	{
		if (selectedTextNum == 0)
		{
			buttonAnimator[0].Play("Blinking");
			buttonAnimator[1].Play("NotBlinking");
		} else
		{
			buttonAnimator[1].Play("Blinking");
			buttonAnimator[0].Play("NotBlinking");
		}
	}
}
