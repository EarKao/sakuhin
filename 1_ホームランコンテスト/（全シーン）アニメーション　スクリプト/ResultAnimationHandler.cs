using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���U���g�V�[���@�A�j���[�V����
public class ResultAnimationHandler : MonoBehaviour
{
    public GameObject whiteScreenGO;

    private void Start()
    {
        StartupActives();

        void StartupActives()
        {
            whiteScreenGO.SetActive(true);
        }
    }
}