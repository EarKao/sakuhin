using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private bool movable;
    [SerializeField] private float speed;

    [SerializeField] private GameObject Camera;
    [SerializeField] private PlayerState p_state;
    [SerializeField] private LayerMask Ground;
    [SerializeField] private AudioSource audio;
    [SerializeField] private Rigidbody2D Enemy_rb;

    private Vector3 originalVector;
    private float distanceToPlayer;

    private SpriteRenderer spriteRenderer;

    private float EnemyDirection = -1; // By Default it goes left

    private void Start()
	{
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        originalVector = transform.position;
    }


    private void Update()
	{
        distanceToPlayer = Camera.transform.position.x - transform.position.x;

		if (originalVector.x < (Camera.transform.position.x - 20) || originalVector.x > (Camera.transform.position.x + 20))
		{
			transform.position = originalVector;
            Enemy_rb.velocity = new Vector2(0,0);

        } else if (movable)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(EnemyDirection, 0), 0.5f, Ground);
            
            if (hit) { EnemyDirection *= -1; }
            if (EnemyDirection == -1) { spriteRenderer.flipX = false; } else { spriteRenderer.flipX = true; }
            transform.position += Vector3.right * EnemyDirection * speed * Time.deltaTime;
        }
	}

	private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            audio.Play();
            p_state.TakeDamage();
        }
    }
}