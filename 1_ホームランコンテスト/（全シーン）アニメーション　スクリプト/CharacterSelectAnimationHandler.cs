using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�L�����N�^�[�I���V�[���@�A�j���[�V����
public class CharacterSelectAnimationHandler : MonoBehaviour
{
    [SerializeField] GameObject whiteScreenGO;

	private void Start()
	{
        StartupActives();

        void StartupActives()
        {
            whiteScreenGO.SetActive(true);
        }
    }
}