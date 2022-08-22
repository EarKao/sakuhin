using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//メインシーン　アニメーション
public class GameAnimationHandler : MonoBehaviour
{
    public GameObject whiteScreenGO;
    public GameObject criticalHitAnimationGO;
    public GameObject ReadySetGoGO;

    private void Start()
    {
        StartupActives();
        StartCoroutine(StartAnimation());

        void StartupActives()
        {
            whiteScreenGO.SetActive(true);

            criticalHitAnimationGO.SetActive(false);
            ReadySetGoGO.SetActive(false);
        }
    }

	IEnumerator StartAnimation()
	{
        yield return new WaitForSeconds(0.25f);
        ReadySetGoGO.GetComponent<TextMeshProUGUI>().text = "Ready?";
        ReadySetGoGO.SetActive(true);
        yield return new WaitForSeconds(1f);
        ReadySetGoGO.GetComponent<TextMeshProUGUI>().text = "GO!!!";
        ReadySetGoGO.GetComponent<TextMeshProUGUI>().fontSize = 140f;
        yield return new WaitForSeconds(0.75f);
        ReadySetGoGO.SetActive(false);



    }
}
