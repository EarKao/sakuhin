using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Animator playerAnimator;

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