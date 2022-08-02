using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObjects : MonoBehaviour
{
    [SerializeField] private PlayerState p_state;
    [SerializeField] private AudioSource audio;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            audio.Play();
            p_state.TakeDamage();
        }
    }
}