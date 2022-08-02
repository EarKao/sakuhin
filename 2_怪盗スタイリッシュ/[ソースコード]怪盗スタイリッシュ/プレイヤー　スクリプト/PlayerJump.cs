using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [HideInInspector] public bool isJumping = false;
    public float jumpForce = 6f;

    [Header("PlayerController Script Reference")]
    public PlayerMovement pC;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && pC.isGrounded && !isJumping)
        {
            PerformJump();
        }
    }

    private void PerformJump()
    {
        isJumping = true;

        pC.animator.SetBool("isJumping", true);

        pC.rigidbody2d.AddForce(Vector2.up * jumpForce * 100f, ForceMode2D.Force);

        StartCoroutine("Onlanding");
    }

    IEnumerator Onlanding()
	{
        float waitTime = 0.1f;

        do
        {
            yield return new WaitForSeconds(waitTime);
            if (pC.isGrounded)
			{
                pC.animator.SetBool("isJumping", false);
                isJumping = false;
            }
        } while (!pC.isGrounded);
	}
}