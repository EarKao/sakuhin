using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultHandler : MonoBehaviour
{
	[SerializeField] Image characterImage;
	[SerializeField] Text characterNameText;
    [SerializeField] TextMeshProUGUI resultsTextLeft;
	[SerializeField] TextMeshProUGUI resultsTextRight;

    [SerializeField] TextMeshProUGUI retryText;
    [SerializeField] TextMeshProUGUI yesText;
	[SerializeField] TextMeshProUGUI noText;

	[SerializeField] Transform selectionBar;

    [HideInInspector] public int mashingScore;
    [HideInInspector] public int timingScore;
    [HideInInspector] public int distanceScore;

	private ResultAnimationHandler rAH;

    private int selectedTextNUm = 0;

	private void Start()
	{
		SetupReference();
		SetSelectedWord();

		StartCoroutine(StartResults());

		void SetupReference()
		{
			rAH = GetComponentInChildren<ResultAnimationHandler>();
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("PS4_DPadVertical") != 0)
		{
			if (selectedTextNUm == 0) selectedTextNUm = 1; else selectedTextNUm = 0;
			SetSelectedWord();
		}
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button2))
		{
			StartCoroutine(StartRetry());
		}
	}


	IEnumerator StartResults()
	{
		SetResultTexts(false, "", "");
		SetRetryFunctions(false);

		characterImage.sprite = CharacterSelectHandler.selectedCharacter.m_sprite;
		characterImage.rectTransform.anchoredPosition = CharacterSelectHandler.selectedCharacter.rankingAnchoredPosition;
		characterImage.rectTransform.sizeDelta = CharacterSelectHandler.selectedCharacter.rankingSizeDelta;

		characterNameText.text = CharacterSelectHandler.selectedCharacter.characterName;
		yield return new WaitForSeconds(1.5f);

		SetResultTexts(true, "˜A‘Å\n", mashingScore.ToString() + "\n");
		yield return new WaitForSeconds(1f);

		SetResultTexts(true, "”ò‹——£\n", distanceScore.ToString() + "km\n");
		yield return new WaitForSeconds(1.5f);

		SetResultTexts(true, "ƒ‰ƒ“ƒLƒ“ƒO\n", (getRankingScore()).ToString() + "ˆÊ\n");
		yield return new WaitForSeconds(1f);

		int characterID = CharacterSelectHandler.selectedCharacter.characterID;
		Highscore hs = new Highscore(characterID, distanceScore);
		SaveSystem.SaveData(hs);

		SetRetryFunctions(true);

		int getRankingScore()
		{
			List<Highscore> hsL = SaveSystem.LoadScore();
			if (hsL != null)
			{
				Highscore rankingHs = new Highscore(CharacterSelectHandler.selectedCharacter.characterID, distanceScore);
				hsL.Add(rankingHs);
				hsL.Sort((s1, s2) => s2.distance.CompareTo(s1.distance));
				return hsL.IndexOf(rankingHs) + 1;
			} else
			{
				return 1;
			}
		}
	}

	IEnumerator StartRetry()
	{
		rAH.whiteScreenGO.GetComponent<Animator>().Play("Whiteground_Exit");
		yield return new WaitForSeconds(1f);

		if (selectedTextNUm == 0) SceneManager.LoadScene("CharacterSelect_Scene");
		else SceneManager.LoadScene("Title_Scene");
	}

	#region Set Functions

	void SetSelectedWord()
	{
		if (selectedTextNUm == 0)
		{
			yesText.fontStyle = FontStyles.Underline;
			noText.fontStyle = FontStyles.Normal;
		} else
		{
			yesText.fontStyle = FontStyles.Normal;
			noText.fontStyle = FontStyles.Underline;
		}
	}

	void SetSelectionBar(Transform _transform)
	{
		selectionBar.transform.position = _transform.position;
	}

	void SetResultTexts(bool _addToText, string _rL, string _rR)
	{
		if (_addToText)
		{
			resultsTextLeft.text += _rL;
			resultsTextRight.text += _rR;
		} else
		{
			resultsTextLeft.text = _rL;
			resultsTextRight.text = _rR;
		}
	}

	void SetRetryFunctions(bool _b)
	{
		retryText.gameObject.SetActive(_b);
		yesText.gameObject.SetActive(_b);
		noText.gameObject.SetActive(_b);
	}

	#endregion
}
