using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSigns : MonoBehaviour
{
    [SerializeField] Text TutorialText;
    [SerializeField] Image TutorialBox;
    [SerializeField] private LayerMask playerLayer;
    
    [SerializeField] CompositeCollider2D Movement;
    [SerializeField] CompositeCollider2D Jump;
    [SerializeField] CompositeCollider2D Sliding;
    [SerializeField] CompositeCollider2D FallingToDeath;
    [SerializeField] CompositeCollider2D FallingReturn;
    [SerializeField] CompositeCollider2D Hook;
    [SerializeField] CompositeCollider2D RunAndHook;
    [SerializeField] CompositeCollider2D Timer;
    [SerializeField] CompositeCollider2D Enemy;
    [SerializeField] CompositeCollider2D Spikes;
    [SerializeField] CompositeCollider2D Goal;

    private void Start()
    {
        TutorialText.enabled = false;
        TutorialBox.enabled = false;
    }

    void FixedUpdate()
    {
        if (Movement.IsTouchingLayers(playerLayer))　
        {
            TutorialText.enabled = true;
            TutorialBox.enabled = true;
            TutorialText.text = "AとDでいどうする";
        } else if (Jump.IsTouchingLayers(playerLayer))
        {
            TutorialText.enabled = true;
            TutorialBox.enabled = true;
            TutorialText.text = "Spaceでジャンプ";
        } else if (Sliding.IsTouchingLayers(playerLayer))
        {
            TutorialText.enabled = true;
            TutorialBox.enabled = true;
            TutorialText.text = "AやDをおしながらSでスライドします";
        } else if (FallingToDeath.IsTouchingLayers(playerLayer))
        {
            TutorialText.enabled = true;
            TutorialBox.enabled = true;
            TutorialText.text = "がめんがいにおちるとしにます";
        } else if (FallingReturn.IsTouchingLayers(playerLayer))
        {
            TutorialText.enabled = true;
            TutorialBox.enabled = true;
            TutorialText.text = "ふつうのばあいはスタートにもどりますが、チュートリアルにはしません";
        } else if (Hook.IsTouchingLayers(playerLayer))
        {
            TutorialText.enabled = true;
            TutorialBox.enabled = true;
            TutorialText.text = "ひだりクリックでフックをだす";
        } else if (RunAndHook.IsTouchingLayers(playerLayer))
        {
            TutorialText.enabled = true;
            TutorialBox.enabled = true;
            TutorialText.text = "AやDをおしながらフック";
        } else if (Timer.IsTouchingLayers(playerLayer))
        {
            TutorialText.enabled = true;
            TutorialBox.enabled = true;
            TutorialText.text = "ステージのじかんせいげんがあります";
        } else if (Enemy.IsTouchingLayers(playerLayer))
        {
            TutorialText.enabled = true;
            TutorialBox.enabled = true;
            TutorialText.text = "てきとぶつかるとじかんがへります";
        } else if (Spikes.IsTouchingLayers(playerLayer))
        {
            TutorialText.enabled = true;
            TutorialBox.enabled = true;
            TutorialText.text = "しょうがいぶつもあります、ぶつかるとじかんがへります";
        } else if (Goal.IsTouchingLayers(playerLayer))
        {
            TutorialText.enabled = true;
            TutorialBox.enabled = true;
            TutorialText.text = "ゴールにとうちゃくとクリア";
        } else { 
            TutorialText.enabled = false; TutorialBox.enabled = false; }
    }
}
