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
        if (Movement.IsTouchingLayers(playerLayer))�@
        {
            TutorialText.enabled = true;
            TutorialBox.enabled = true;
            TutorialText.text = "A��D�ł��ǂ�����";
        } else if (Jump.IsTouchingLayers(playerLayer))
        {
            TutorialText.enabled = true;
            TutorialBox.enabled = true;
            TutorialText.text = "Space�ŃW�����v";
        } else if (Sliding.IsTouchingLayers(playerLayer))
        {
            TutorialText.enabled = true;
            TutorialBox.enabled = true;
            TutorialText.text = "A��D�������Ȃ���S�ŃX���C�h���܂�";
        } else if (FallingToDeath.IsTouchingLayers(playerLayer))
        {
            TutorialText.enabled = true;
            TutorialBox.enabled = true;
            TutorialText.text = "���߂񂪂��ɂ�����Ƃ��ɂ܂�";
        } else if (FallingReturn.IsTouchingLayers(playerLayer))
        {
            TutorialText.enabled = true;
            TutorialBox.enabled = true;
            TutorialText.text = "�ӂ��̂΂����̓X�^�[�g�ɂ��ǂ�܂����A�`���[�g���A���ɂ͂��܂���";
        } else if (Hook.IsTouchingLayers(playerLayer))
        {
            TutorialText.enabled = true;
            TutorialBox.enabled = true;
            TutorialText.text = "�Ђ���N���b�N�Ńt�b�N������";
        } else if (RunAndHook.IsTouchingLayers(playerLayer))
        {
            TutorialText.enabled = true;
            TutorialBox.enabled = true;
            TutorialText.text = "A��D�������Ȃ���t�b�N";
        } else if (Timer.IsTouchingLayers(playerLayer))
        {
            TutorialText.enabled = true;
            TutorialBox.enabled = true;
            TutorialText.text = "�X�e�[�W�̂����񂹂����񂪂���܂�";
        } else if (Enemy.IsTouchingLayers(playerLayer))
        {
            TutorialText.enabled = true;
            TutorialBox.enabled = true;
            TutorialText.text = "�Ă��ƂԂ���Ƃ����񂪂ւ�܂�";
        } else if (Spikes.IsTouchingLayers(playerLayer))
        {
            TutorialText.enabled = true;
            TutorialBox.enabled = true;
            TutorialText.text = "���傤�����Ԃ�����܂��A�Ԃ���Ƃ����񂪂ւ�܂�";
        } else if (Goal.IsTouchingLayers(playerLayer))
        {
            TutorialText.enabled = true;
            TutorialBox.enabled = true;
            TutorialText.text = "�S�[���ɂƂ����Ⴍ�ƃN���A";
        } else { 
            TutorialText.enabled = false; TutorialBox.enabled = false; }
    }
}
