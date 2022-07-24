using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum GameState {
    Start, Mashing, Timing, End
}

public class GameHandler : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI topText;
	[SerializeField] Text bottomRText;
	[SerializeField] TextMeshProUGUI bottomLText;

	[SerializeField] GameObject PlayerGO;

	[SerializeField] Rigidbody2D sandbagRB;
	[SerializeField] float sandbagMass;
	private int sandbadStartingPosX;

	[Header("[ Start Game ]")]
	[SerializeField] float gameStartTime;

	[Header("[ Mashing ]")]
	[SerializeField] char[] mashingKeys;
	[SerializeField] List<KeyCode> mashingKeysController = new List<KeyCode>(); // WASD △□XO 3012
	[SerializeField] float[] mashingWaitTime;
	[SerializeField] AudioSource mashingAudio;
	[SerializeField] Animator buttonAnimator;

	//Mashing: Button
	int keyNum = -1;
	char currentKey;
	int finalMashingScore;

	[Header("[ Timing ]")]
	[SerializeField] Slider timingSlider; //For Reference Only
	[SerializeField] RectTransform timingCriticalZoneRT;
	[SerializeField] RectTransform YellowZone;
	[SerializeField] Vector3 criticalPercentage;
	[SerializeField] GameObject timingTipsGO;
	[SerializeField] AudioSource cutinAudio;

	bool inSliderLoop = true;
	int finalTimingScore;

	[Header("[ End Game ]")]
	[SerializeField] float speedupMul;
	[SerializeField] AudioSource sandbagFlyAudio;
	private bool speedup = false;

	[SerializeField] Image background;
	[SerializeField] Sprite[] bgImages;

	private GameAnimationHandler gAH;
	
	GameState gameState = GameState.Start;

	private void Start()
	{
		Setup();
		SetupReference();
		StartCoroutine(StartGame());

		void Setup()
		{
			PlayerGO.GetComponent<SpriteRenderer>().sprite = CharacterSelectHandler.selectedCharacter.m_sprite;
			PlayerGO.transform.position = CharacterSelectHandler.selectedCharacter.gameTransformPos;
			PlayerGO.transform.localScale = CharacterSelectHandler.selectedCharacter.gameTransformScale;

			timingTipsGO.SetActive(false);

			sandbadStartingPosX = (int)sandbagRB.transform.position.x; //Sets the Original Position of the Sandbag so that it's Starting Position does not affect the Final Score
		}

		void SetupReference()
		{
			gAH = GetComponentInChildren<GameAnimationHandler>();
		}
	}
	private void Update()
	{
		if (gameState == GameState.Mashing)
		{
			if (currentKey == mashingKeys[keyNum] && Input.GetKeyDown(mashingKeys[keyNum].ToString()))
			{
				finalMashingScore++;
				buttonAnimator.Play("ButtonPressed");
				mashingAudio.Play();
			} else
			if (currentKey == mashingKeys[keyNum] && Input.GetKeyDown(mashingKeysController[keyNum]))
			{
				finalMashingScore++;
				buttonAnimator.Play("ButtonPressed");
				mashingAudio.Play();
				Debug.Log(mashingKeysController[keyNum]);
			}
		}

		if (gameState == GameState.Timing)
		{
			if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button2))
			{
				inSliderLoop = false;
			}
		}

		if (gameState == GameState.End)
        {
			if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button2))
            {
				if (!speedup)
				{
					speedup = true;
					Time.timeScale = speedupMul;
					SetBottomRText("\n\n\n\n〇で早送り▶▶▶");
					Debug.Log("Time is now " + speedupMul + " times faster");
				} else
                {
					speedup = false;
					Time.timeScale = 1;
					SetBottomRText("\n\n\n\n〇で早送り▶");
					Debug.Log("Time is now at normal speed");
                }
            }
        }
	}

	#region Game Phases
	IEnumerator StartGame()
	{
		SetTopText("");
		SetBottomLText("");
		SetBottomRText("");

		timingSlider.gameObject.SetActive(false);
		buttonAnimator.gameObject.SetActive(false);

		yield return new WaitForSeconds(gameStartTime);        // Initial wait for Load Scene

		StartCoroutine(StartMashing());

		gameState = GameState.Mashing;
	}

	IEnumerator StartMashing()
	{
		buttonAnimator.gameObject.SetActive(true);
		
		mashingKeys = CharacterSelectHandler.selectedCharacter.mash_keys;
		mashingWaitTime = CharacterSelectHandler.selectedCharacter.mash_waitTime;

		for (int i = 0; i < mashingKeys.Length; i++) // Sets up KeyCode for controller beforehand
		{
			switch (mashingKeys[i])
			{
				case 'w': mashingKeysController.Add(KeyCode.Joystick1Button3); break; // △
				case 'a': mashingKeysController.Add(KeyCode.Joystick1Button0); break; // □
				case 's': mashingKeysController.Add(KeyCode.Joystick1Button1); break; // X
				case 'd': mashingKeysController.Add(KeyCode.Joystick1Button2); break; // O
			}
		}


		StartCoroutine(mashingCountdown());

		for (int i = 0; i < mashingKeys.Length; i++)
		{
			keyNum = i;
			currentKey = mashingKeys[i];
			
			SetBottomRText(GetControllerSymbols(currentKey) + "を連打！");
			while (true)
			{
				yield return new WaitForSeconds(0.1f);
				mashingWaitTime[i] -= 0.1f;

				if (mashingWaitTime[i] <= 0) break;
			}

			char GetControllerSymbols(char letter)
			{
				char symbol = '-';
				switch (letter)
				{
					case 'w': symbol = '△'; break;
					case 'a': symbol = '☐'; break;
					case 's': symbol = '✕'; break;
					case 'd': symbol = '〇'; break;
				}
				return symbol;
			}
		}

		IEnumerator mashingCountdown()
		{
			float mashingCDtime = 0;

			foreach (float t in mashingWaitTime) mashingCDtime += t;

			while (true)
			{
				SetBottomLText(mashingCDtime.ToString("0.0"));
				yield return new WaitForSeconds(0.1f);
				mashingCDtime -= 0.1f;

				if (mashingCDtime <= 0)
				{
					SetBottomLText("");
					break;
				}
			}
		}

		buttonAnimator.gameObject.SetActive(false);
		keyNum = -1; // Key Number -1 means invalid

		SetTopText(finalMashingScore.ToString());
		SetBottomRText("");

		StartCoroutine(StartTiming());
		gameState = GameState.Timing;
	}

	IEnumerator StartTiming()
	{
		yield return new WaitForSeconds(0.5f);

		//Setup Character Statistics
		float characterSpeedLevel = CharacterSelectHandler.selectedCharacter.timingSpeed / 1000;
		timingCriticalZoneRT.anchoredPosition = new Vector2(CharacterSelectHandler.selectedCharacter.timingLocation, timingCriticalZoneRT.anchoredPosition.y);
		timingCriticalZoneRT.sizeDelta = new Vector2(CharacterSelectHandler.selectedCharacter.timingWidth, timingCriticalZoneRT.sizeDelta.y);

		//Setup YellowStone Park
		YellowZone.anchoredPosition = timingCriticalZoneRT.anchoredPosition;
		YellowZone.sizeDelta = new Vector2(timingCriticalZoneRT.sizeDelta.x + 100, YellowZone.sizeDelta.y);

		timingTipsGO.gameObject.SetActive(true);
		timingSlider.gameObject.SetActive(true);

		bool forward = true;

		while (inSliderLoop)
		{
			AddValue(1);
			yield return new WaitForSeconds(characterSpeedLevel);

			void AddValue(int _value)
			{
				if (forward) timingSlider.value += _value; else timingSlider.value -= _value;
				
				if (timingSlider.value >= 100) forward = false; 
				else if (timingSlider.value <= 0) forward = true;
			}
		}
		timingSlider.gameObject.SetActive(false);
		timingTipsGO.gameObject.SetActive(false);

		finalTimingScore = (int)GetSliderScore();
		yield return new WaitForSeconds(0.5f);

		StartCoroutine(EndGame());
		gameState = GameState.End;

		float GetSliderScore()
		{
			float sliderValue = timingSlider.value;
			float finalScore;

			//Physical Postions
			float scoreMul = timingSlider.maxValue / timingSlider.GetComponent<RectTransform>().sizeDelta.x; // 1000 / 500 = 2
			float CzPosX = timingCriticalZoneRT.anchoredPosition.x; // The location of the Critical Zone (Currently 250)
			float CzWidth = timingCriticalZoneRT.sizeDelta.x; // 100
			float XsDistance = 10;  // x1 to x2  or  x3 to x4, vice versa
			Debug.Log("scoreMul: " + scoreMul + "     CzPosX: " + CzPosX + "     CzWidth: " + CzWidth);

			float s25 = CharacterSelectHandler.selectedCharacter.timingScoreSet * criticalPercentage.x;
			float s15 = CharacterSelectHandler.selectedCharacter.timingScoreSet * criticalPercentage.y;
			float s10 = CharacterSelectHandler.selectedCharacter.timingScoreSet * criticalPercentage.z;
			Debug.Log("s10: " + s10 + "     s15: " + s15 + "     s25: " + s25);

			//float xMin = 0; Useless, use it in else
			float x1 = scoreMul * CzPosX - (scoreMul * CzWidth / 2) - XsDistance;
			float x2 = scoreMul * CzPosX - (scoreMul * CzWidth / 2);
			float xMid = scoreMul * CzPosX;
			float x3 = scoreMul * CzPosX + (scoreMul * CzWidth / 2);
			float x4 = scoreMul * CzPosX + (scoreMul * CzWidth / 2) + XsDistance;
			//float xMax = 1000; Useless, use it in else
			Debug.Log("x1: " + x1 + "     x2: " + x2 + "     xMid: " + xMid + "     x3: " + x3 + "     x4: " + x4);

			if (x2 <= sliderValue && sliderValue <= x3)
			{
				finalScore = s25 - (Mathf.Abs(sliderValue - xMid) * ((s25 - s15) / 100));
				PlayCutin();
				Debug.Log("The Slider Value is in the CriticalZone (Between x2 and x3)");
			} else if (x1 <= sliderValue && sliderValue <= x4)
			{
				finalScore = (sliderValue - x1) * ((s15 - s10) / 100) + s10;
				
				Debug.Log("The Slider Value is in the YellowZone, but not the CriticalZone (Between x1 and x4, but not x2 and x3)");
			} else
			{
				finalScore = s10;
				Debug.Log("The Slider Value is in the White Zone (Not between x1 and x4, nor x2 and x3)");
			}
			return finalScore;

			void PlayCutin()
			{
				gAH.criticalHitAnimationGO.GetComponentsInChildren<Image>()[2].sprite = CharacterSelectHandler.selectedCharacter.m_sprite;
				gAH.criticalHitAnimationGO.GetComponentsInChildren<RectTransform>()[3].anchoredPosition = CharacterSelectHandler.selectedCharacter.gameAnchoredPosition;
				gAH.criticalHitAnimationGO.GetComponentsInChildren<RectTransform>()[3].sizeDelta = CharacterSelectHandler.selectedCharacter.gameSizeDelta;

				gAH.criticalHitAnimationGO.SetActive(true);
				cutinAudio.Play();
			}
		}
	}

	IEnumerator EndGame()
	{
		yield return new WaitForSeconds(0.5f);

		float finalScore = finalMashingScore + finalTimingScore;
		Debug.Log("FinalMashingScore: " + finalMashingScore + "   FinalTimingScore: " + finalTimingScore + "   FinalScore: " + finalScore);

		yield return new WaitForSeconds(0.5f);

		PlayerGO.transform.position = CharacterSelectHandler.selectedCharacter.gameTransformPos;
		PlayerGO.transform.localScale = CharacterSelectHandler.selectedCharacter.gameTransformScale;


		PlayerGO.GetComponent<Animator>().enabled = true;
		int cID = CharacterSelectHandler.selectedCharacter.characterID;
		PlayerGO.GetComponent<Animator>().Play("Hit_" + cID);
		
		yield return new WaitForSeconds(0.5f);

		sandbagRB.mass = sandbagMass;

		sandbagFlyAudio.Play();
		sandbagRB.AddTorque(finalScore * 5, ForceMode2D.Impulse);
		sandbagRB.AddForce(new Vector2(finalScore * 4f, finalScore * 5f), ForceMode2D.Impulse);
		SetBottomRText("\n\n\n\n〇で早送り▶");
		bottomRText.fontSize = 48;
		yield return new WaitForSeconds(0.5f);

		int finalDistanceScore = (int)sandbagRB.transform.position.x;
		while (true)
		{
			finalDistanceScore = (int)sandbagRB.transform.position.x - sandbadStartingPosX;

			SetTopText(finalDistanceScore + "km");    // Updates UI for kilometers traveled

			CheckBackground();
			if (sandbagRB.velocity.x == 0) break;
			yield return new WaitForEndOfFrame();

			void CheckBackground()
            {
				if (sandbagRB.transform.position.y >= 1700) ChangeBackgroundInto(bgImages[7]);
				else if (sandbagRB.transform.position.y >= 1500) ChangeBackgroundInto(bgImages[6]);
				else if (sandbagRB.transform.position.y >= 1250) ChangeBackgroundInto(bgImages[5]);
				else if (sandbagRB.transform.position.y >= 1000) ChangeBackgroundInto(bgImages[4]);
				else if (sandbagRB.transform.position.y >= 700) ChangeBackgroundInto(bgImages[3]);
				else if (sandbagRB.transform.position.y >= 300) ChangeBackgroundInto(bgImages[2]);
				else if (sandbagRB.transform.position.y >= 150) ChangeBackgroundInto(bgImages[1]);
				else if (sandbagRB.transform.position.y >= 0) ChangeBackgroundInto(bgImages[0]);

				void ChangeBackgroundInto(Sprite sprite)
                {
					background.sprite = sprite;
				}
            }
		}

		yield return new WaitForSeconds(0.25f);
		Time.timeScale = 1;

		SceneManager.sceneLoaded += SceneLoaded;
		SceneManager.LoadScene("Result_Scene");

		void SceneLoaded(Scene nextScene, LoadSceneMode mode)
		{
			ResultHandler resultHandler = GameObject.Find("ResultHandler").GetComponent<ResultHandler>();
			resultHandler.mashingScore = finalMashingScore;
			resultHandler.timingScore = finalTimingScore;
			resultHandler.distanceScore = finalDistanceScore;

			SceneManager.sceneLoaded -= SceneLoaded;
		}
	}

	#endregion

	#region Set Functions

	void SetTopText(string _text)
	{
		topText.text = _text;
	}

	void SetBottomRText(string _text)
	{
		bottomRText.text = _text;
	}

	void SetBottomLText(string _text)
	{
		bottomLText.text = _text;
	}
	#endregion
}