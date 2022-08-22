using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//キャラクター選択シーン　アニメーション
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