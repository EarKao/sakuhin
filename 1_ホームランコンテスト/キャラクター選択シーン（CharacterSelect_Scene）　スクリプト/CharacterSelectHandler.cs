using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum DialogueBox
{
    Name, PowerStars, BattingStars, Technique
}

//キャラクター選択シーン
public class CharacterSelectHandler : MonoBehaviour
{
    public List<Character> characterList = new List<Character>();    // Static Variable

    [SerializeField] Animator whiteScreenAnimator;

    [SerializeField] Image cHighlighted;
    [SerializeField] Transform selectionBar;
    private Animator selectionBarAnimator;

    //キャラクターのテキスト
    [SerializeField] Text cNameText;
    [SerializeField] Text cPowerStars;
    [SerializeField] Text cBattingStars;
    [SerializeField] Text cTechText;

    //アニメーションの速度
    [SerializeField] float textAnimationSpeed;
    [SerializeField] float starsAnimationSpeed;

    //BGMとサウンド
    [SerializeField] AudioSource characterSelectBGM;
    [SerializeField] AudioSource onSelectionChangeSE;
    [SerializeField] AudioSource onSelectionChooseSE;

    public static Character selectedCharacter; //プレイヤー選択したキャラ　（スタティック変数）
    private int selectedCharacterNum = 4; //開始位置
    private bool isSelectable = true; //選択できるかのbool

    private void Start()
    {
        SetupReference();
        SetCharacter(characterList[selectedCharacterNum]); //開始キャラを設定する

		void SetupReference(){
            selectionBarAnimator = selectionBar.GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("PS4_DPadVertical") > 0 && isSelectable)
        {
            if (selectedCharacterNum == 8) selectedCharacterNum = 0;
            else selectedCharacterNum++;
            onSelectionChangeSE.Play();
            SetCharacter(characterList[selectedCharacterNum]);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("PS4_DPadVertical") < 0 && isSelectable)
        {
            if (selectedCharacterNum == 0) selectedCharacterNum = 8;
            else selectedCharacterNum--;
            onSelectionChangeSE.Play();
            SetCharacter(characterList[selectedCharacterNum]);
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button2) && isSelectable)
        {
            isSelectable = false;
            StartCoroutine(LoadMainScene());
        }
    }

    //MainSceneをロード
    IEnumerator LoadMainScene()
    {
        selectionBarAnimator.Play("SelectionImage_Selected");
        onSelectionChooseSE.Play();
        StartCoroutine(LowerMusic());
        yield return new WaitForSeconds(0.75f);
        whiteScreenAnimator.Play("Whiteground_Exit");
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("Main_Scene");

        IEnumerator LowerMusic()
        {
            while (true)
            {
                characterSelectBGM.volume -= 0.05f;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    //キャラ選択
    private void SetCharacter(Character targetC)
    {
        selectionBar.position = targetC.transform.position;

        cHighlighted.sprite = targetC.m_sprite;
        cHighlighted.rectTransform.anchoredPosition = targetC.GetComponentsInChildren<RectTransform>()[1].anchoredPosition;
        cHighlighted.rectTransform.sizeDelta = targetC.GetComponentsInChildren<RectTransform>()[1].sizeDelta;


        selectedCharacter = targetC;

        StopAllCoroutines();
		StartCoroutine(AnimateText(targetC.characterName, DialogueBox.Name));
        
        StartCoroutine(AnimateText(targetC.powerLevel, DialogueBox.PowerStars));
		StartCoroutine(AnimateText(targetC.battingLevel, DialogueBox.BattingStars));
		

		StartCoroutine(AnimateText(targetC.techniqueText, DialogueBox.Technique));
    }

    //テキストアニメーション
	#region Text Animation
	IEnumerator AnimateText(string _text, DialogueBox _dialogueBox) //テキスト　用
    {
        switch (_dialogueBox)
        {
            case DialogueBox.Name:
                cNameText.text = "";
                foreach (char letter in _text.ToCharArray())
                {
                    yield return new WaitForSeconds(textAnimationSpeed);
                    cNameText.text += letter;
                }
                break;
            case DialogueBox.Technique:
                cTechText.text = "";
                foreach (char letter in _text.ToCharArray())
                {
                    yield return new WaitForSeconds(textAnimationSpeed);
                    cTechText.text += letter;
                }
                break;
            default: break;
        }
    }

    IEnumerator AnimateText(int _num, DialogueBox _dialogueBox) //ほし★　用
    {
        switch (_dialogueBox)
        {
            case DialogueBox.PowerStars:
                cPowerStars.text = "";
                for (int i = 0; i < _num; i++)
                {
                    yield return new WaitForSeconds(starsAnimationSpeed);
                    cPowerStars.text += "★";
                }
                break;
            case DialogueBox.BattingStars:
                cBattingStars.text = "";
                for (int i = 0; i < _num; i++)
                {
                    yield return new WaitForSeconds(starsAnimationSpeed);
                    cBattingStars.text += "★";
                }
                break;
            default: break;
        }
    }
	#endregion
}