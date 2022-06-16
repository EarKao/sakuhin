using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimationHandler : MonoBehaviour
{
    [SerializeField] GameObject whiteScreenGO;
    [SerializeField] GameObject logoImageGO;
    [SerializeField] GameObject HomerunContestStringGO;
    [SerializeField] GameObject characterImageGO;

    [SerializeField] GameObject startImage;
    [SerializeField] GameObject rankingImage;

    [SerializeField] GameObject startTextGO;
    [SerializeField] GameObject rankingTextGO;

    public AudioSource batHittingSE;

    void Start()
    {
        StartupActives();
        StartCoroutine(StartAnimation());

        void StartupActives()
		{
            logoImageGO.SetActive(false);
            characterImageGO.SetActive(false);
            whiteScreenGO.SetActive(true);
            HomerunContestStringGO.SetActive(false);

            startTextGO.SetActive(false);
            rankingTextGO.SetActive(false);

            startImage.SetActive(false);
            rankingImage.SetActive(false);
        }
    }

	IEnumerator StartAnimation()
	{
        yield return new WaitForSeconds(0.5f);
        logoImageGO.SetActive(true);
        
        yield return new WaitForSeconds(2f);
        characterImageGO.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        batHittingSE.Play();

        yield return new WaitForSeconds(0.4f);
        HomerunContestStringGO.SetActive(true);

        yield return new WaitForSeconds(1f);
        //startTextGO.SetActive(true);
        startImage.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        //rankingTextGO.SetActive(true);
        rankingImage.SetActive(true);

        yield return new WaitForSeconds(0.75f);
        GetComponentInParent<TitleHandler>().SetButtonAnimation();
    }
}
