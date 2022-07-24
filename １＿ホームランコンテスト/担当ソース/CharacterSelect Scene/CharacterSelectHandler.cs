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

public class CharacterSelectHandler : MonoBehaviour
{
    public List<Character> characterList = new List<Character>();    // Static Variable

    [SerializeField] Animator whiteScreenAnimator;

    [SerializeField] Image cHighlighted;
    [SerializeField] Transform selectionBar;
    private Animator selectionBarAnimator;

    [SerializeField] Text cNameText;
    [SerializeField] Text cPowerStars;
    [SerializeField] Text cBattingStars;
    [SerializeField] Text cTechText;

    [SerializeField] float textAnimationSpeed;
    [SerializeField] float starsAnimationSpeed;

    [SerializeField] AudioSource characterSelectBGM;
    [SerializeField] AudioSource onSelectionChangeSE;
    [SerializeField] AudioSource onSelectionChooseSE;

    public static Character selectedCharacter; //Static Variable
    private int selectedCharacterNum = 4;
    private bool isSelectable = true;

    private void Start()
    {
        SetupReference();
        SetCharacter(characterList[selectedCharacterNum]); //Sets Default Character to the First Character

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

	#region Text Animation
	IEnumerator AnimateText(string _text, DialogueBox _dialogueBox) //Text Dialogue Box Only
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

    IEnumerator AnimateText(int _num, DialogueBox _dialogueBox) //Stars Dialogue Box Only
    {
        switch (_dialogueBox)
        {
            case DialogueBox.PowerStars:
                cPowerStars.text = "";
                for (int i = 0; i < _num; i++)
                {
                    yield return new WaitForSeconds(starsAnimationSpeed);
                    cPowerStars.text += "š";
                }
                break;
            case DialogueBox.BattingStars:
                cBattingStars.text = "";
                for (int i = 0; i < _num; i++)
                {
                    yield return new WaitForSeconds(starsAnimationSpeed);
                    cBattingStars.text += "š";
                }
                break;
            default: break;
        }
    }
	#endregion
}