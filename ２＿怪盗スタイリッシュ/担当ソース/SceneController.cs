using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;　　　//ボタンを使用するのでUI
using UnityEngine.SceneManagement;//SceneManagerを使用ためSceneManagementを追加

public class SceneController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Animator playerAnimator;


    // ボタンをクリックするとScene『Mein Scene』に移動
    public void ButtonClick()
    {
        Invoke("Call", 1f);
        playerRb.velocity = new Vector2(15f, playerRb.velocity.y) ;
        playerAnimator.SetBool("isDashing", true);
    }

    void Call()
    {
        SceneManager.LoadScene("Main Scene");
    }
}