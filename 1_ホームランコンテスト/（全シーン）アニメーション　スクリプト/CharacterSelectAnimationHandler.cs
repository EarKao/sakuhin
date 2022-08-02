using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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