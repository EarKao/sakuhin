using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤースライディング
public class PlayerSlide : MonoBehaviour
{
	[HideInInspector] public bool isSliding = false;
    public float slideSpeed = 5f;
	public float slideTime = 0.8f;

    [Header("PlayerMovement Script Reference")]
	public PlayerMovement p_Movement;

    private void Start()
    {
		p_Movement.slideCollider.enabled = false;
    }

    private void Update()
	{
        if (Input.GetKeyDown(KeyCode.S) && p_Movement.isGrounded && !isSliding)
		{
            PerformSlide();
		}
	}

	private void PerformSlide()
	{
		isSliding = true;

		p_Movement.animator.SetBool("isSliding", true);

		p_Movement.bodyCollider.enabled = false;
		p_Movement.slideCollider.enabled = true;

		if (p_Movement.spriteRenderer.flipX == true)
		{
			p_Movement.rigidbody2d.AddForce(Vector2.left * slideSpeed * 50f);
		} else if (p_Movement.spriteRenderer.flipX == false)
		{
			p_Movement.rigidbody2d.AddForce((Vector2.right) * slideSpeed * 50f);
		}

		StartCoroutine("StopSliding");
	}

    IEnumerator StopSliding()
    {
        yield return new WaitForSeconds(slideTime);
		p_Movement.animator.Play("Player_Idle");
		p_Movement.animator.SetBool("isSliding", false);
		p_Movement.bodyCollider.enabled = true;
		p_Movement.slideCollider.enabled = false;
        isSliding = false;
    }
}
