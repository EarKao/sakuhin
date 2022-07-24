using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("プレイヤー基本操作")]
    public float moveSpeed = 8f;
    public bool canMove = true;

    [Header("Basic References")]
    public BoxCollider2D bodyCollider;
    public BoxCollider2D slideCollider;
    public BoxCollider2D bc_isGrounded;
    public bool isGrounded;
    public Animator animator;
    [SerializeField] private LayerMask groundLayer;

    /* Awake References */
    [HideInInspector] public Rigidbody2D rigidbody2d;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    void Awake()
    {
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }

	void FixedUpdate()
    {
        Move();
        GroundCheck();
    }

    private void Move()
    {
        if (!slideCollider.enabled && canMove)
        {
            if (Input.GetKey(KeyCode.D)) // Move Right
            {
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                spriteRenderer.flipX = false;

                animator.SetBool("isDashing", true);
            }
            else if (Input.GetKey(KeyCode.A)) // Move Left
            {
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                spriteRenderer.flipX = true;

                animator.SetBool("isDashing", true);
            }
            else
            {
                animator.SetBool("isDashing", false);
            }
        }
    }

    void GroundCheck()
    {
        if (bc_isGrounded.IsTouchingLayers(groundLayer))
        {
            isGrounded = true;
        } else
        {
            isGrounded = false;
        }
    }
}