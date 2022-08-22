using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//メインシーンハンドラー
public class MainHandler : MonoBehaviour
{
	[Header("Main Menu")]
	public GameObject mainMenuGO;
	
	[Header("Stage Selection")]
	public GameObject stageSelectionGO;

	[Header("Character Selection")]
	public GameObject characterSelectionGO;
	
	[Header("Character Team Edit")]
	public GameObject characterTeamEditGO;

	private string selectedStage;
	private AudioSource mainMenuBGM;
	private AudioSource stageSelectBGM;

	private void Awake()
	{
		SetUpReference();

		mainMenuBGM.Play();

		mainMenuGO.SetActive(true);
		stageSelectionGO.SetActive(false);
		characterSelectionGO.SetActive(false);
		characterTeamEditGO.SetActive(false);

		void SetUpReference(){
			mainMenuBGM = GetComponents<AudioSource>()[0];
			stageSelectBGM = GetComponents<AudioSource>()[1];
		}
	}

	/// <summary>
	/// Main Screen
	/// </summary>

	public void OnAdventureButton()
	{
		mainMenuBGM.Stop();
		stageSelectBGM.Play();

		mainMenuGO.SetActive(false);
		stageSelectionGO.SetActive(true);
	}

	/// <summary>
	/// Stage Selection
	/// </summary>

	public void OnReturnToMainMenu()
	{
		stageSelectBGM.Stop();
		mainMenuBGM.Play();

		stageSelectionGO.SetActive(false);
		mainMenuGO.SetActive(true);
	}

	public void OnStage1_Button()
	{
		characterSelectionGO.SetActive(true);
		selectedStage = "BattleS1_Scene";
	}

	public void OnStage2_Button()
	{
		characterSelectionGO.SetActive(true);
		selectedStage = "BattleS2_Scene";
	}

	public void OnStage5_Button()
	{
		characterSelectionGO.SetActive(true);
		selectedStage = "BattleS3_Scene";
	}

	/// <summary>
	/// Character Selection
	/// </summary>

	public void OnReturnToStageSelection()
	{
		characterSelectionGO.SetActive(false);
	}

	public void OnStartStageButton()
	{
		SceneManager.LoadScene(selectedStage);
	}

	public void OnTeamEditButton()
	{
		characterTeamEditGO.SetActive(true);
	}

	/// <summary>
	/// Character Team Edit
	/// </summary>

	public void OnReturnToCharacterSelection()
	{
		characterTeamEditGO.SetActive(false);
	}
}
