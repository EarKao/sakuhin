using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����L���O�V�[��
public class RankingAnimationHandler : MonoBehaviour
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
