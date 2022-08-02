using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutofStageColliders : MonoBehaviour
{
	[SerializeField] private float returnX = 0, returnY = 0;

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.CompareTag("Player"))
		{
			collider.transform.position = new Vector2(returnX, returnY);
			collider.attachedRigidbody.velocity = new Vector2(0f, 0f);
		}
	}
}