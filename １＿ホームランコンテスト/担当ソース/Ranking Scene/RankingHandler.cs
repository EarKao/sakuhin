using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RankingHandler : MonoBehaviour
{
	[SerializeField] float scrollerSpeed;

	[SerializeField] RectTransform entryContainerRT;
	private float containerSizeX;
	[SerializeField] Transform entry;
    [SerializeField] ScrollRect scrollRect;
	[SerializeField] float templateHeight = 391f;

	[SerializeField] Animator whiteScreenAnimator;

	[SerializeField] Sprite[] characterIdImages;
	private readonly List<Highscore> highscoreList = SaveSystem.LoadScore();

	[SerializeField] bool debugMode;
	[SerializeField] int debugScrollerLength;
	[SerializeField] Vector2 debugDistance;

	private void Awake()
	{
		SetupRanking();

		containerSizeX = entryContainerRT.sizeDelta.x / templateHeight;

		void SetupRanking()
		{
			entry.gameObject.SetActive(false);

			if (highscoreList != null && !debugMode)  // Reads highscore from Save File
			{
				entryContainerRT.sizeDelta = new Vector2(highscoreList.Count * templateHeight, entryContainerRT.sizeDelta.y);
				
				highscoreList.Sort((s1, s2) => s2.distance.CompareTo(s1.distance)); // Sort List by distance

				for (int i = 0; i < highscoreList.Count; i++)
				{
					Instantiate(entry, entryContainerRT);

					RectTransform entryRectTransform = entry.GetComponent<RectTransform>();     // Find the RectTransform
					entryRectTransform.anchoredPosition = new Vector2(templateHeight * i, entryRectTransform.anchoredPosition.y);  // Set a new AnchoredPosition

					Image entrySprite = entry.GetComponentInChildren<Image>();                  // Find the Image
					entrySprite.sprite = CharacterSpriteFinder(highscoreList[i].characterID);   // Set a new Image

					TextMeshProUGUI rankingNum = entry.GetComponentsInChildren<TextMeshProUGUI>()[0]; // Find the 1st Text
					rankingNum.text = (i + 1).ToString();                       // Set the Ranking Number

					TextMeshProUGUI distanceFlew = entry.GetComponentsInChildren<TextMeshProUGUI>()[1];   // Find the 2nd Text
					distanceFlew.text = highscoreList[i].distance.ToString();       // Set the Distance Flew

					entry.gameObject.SetActive(true);
				}
			}
			if (debugMode)    // For Debugging
			{
				entryContainerRT.sizeDelta = new Vector2(debugScrollerLength * templateHeight, entryContainerRT.sizeDelta.y);

				/////////////

				List<int> randomDistance = new List<int>();
				for (int i = 0; i < debugScrollerLength; i++) randomDistance.Add((int)Random.Range(debugDistance.x, debugDistance.y));
				randomDistance.Sort((r1, r2) => r2.CompareTo(r1));

				/////////////

				for (int i = 0; i < debugScrollerLength; i++)
				{
					Instantiate(entry, entryContainerRT);

					RectTransform entryRectTransform = entry.GetComponent<RectTransform>();
					entryRectTransform.anchoredPosition = new Vector2(templateHeight * i, 0f);

					Image entrySprite = entry.GetComponentInChildren<Image>();
					entrySprite.sprite = CharacterSpriteFinder(Random.Range(0, 9));

					TextMeshProUGUI rankingNum = entry.GetComponentsInChildren<TextMeshProUGUI>()[0];
					rankingNum.text = (i + 1).ToString();

					TextMeshProUGUI distanceFlew = entry.GetComponentsInChildren<TextMeshProUGUI>()[1];
					distanceFlew.text = randomDistance[i].ToString();

					entry.gameObject.SetActive(true);
				}
			}
		}
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("PS4_DPadVertical") > 0) scrollRect.horizontalNormalizedPosition += (scrollerSpeed / 100) / containerSizeX;
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("PS4_DPadVertical") < 0) scrollRect.horizontalNormalizedPosition -= (scrollerSpeed / 100) / containerSizeX;
		if (Input.GetKey(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button2)) StartCoroutine(LoadTitleScene());
	}

	IEnumerator LoadTitleScene()
	{
		whiteScreenAnimator.Play("Whiteground_Exit");
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene("Title_Scene");
	}

	private Sprite CharacterSpriteFinder(int _characterID)	// There must be an easier way to do this :^(
	{
		for (int i = 0; i < characterIdImages.Length; i++)
		{
			if (_characterID == i)
			{
				return characterIdImages[i];
			}
		}
		return null;
	}

	/*
	#region Editor
	#if UNITY_EDITOR

	[CustomEditor(typeof(RankingHandler))]
	public class RankingHandlerEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			RankingHandler rankingHandler = (RankingHandler)target;
			DrawDetails(rankingHandler);

			bool debugBool = rankingHandler.debug;

			if (debugBool)
			{
				rankingHandler.
			}
		}

		private void DrawDetails(RankingHandler rh)
		{
			
		}
	}
	#endif
	#endregion*/
}