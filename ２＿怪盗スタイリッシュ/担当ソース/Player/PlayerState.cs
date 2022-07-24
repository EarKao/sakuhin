using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    [SerializeField] float invincibleTime = 2f;

    [SerializeField] Timer timer;
    [SerializeField] Text countdownTimerUI;

    public PlayerMovement p_Controller;
    public float flashRate = 0.2f;

    private State p_state;

    void Awake()
    {
        p_state = State.Normal;
    }
    public void TakeDamage(float _damageTaken = 3)
    {
        if (p_state == State.Normal)
        {
            StartCoroutine(TriggerFlash());
            StartCoroutine(TriggerInvincibility(_damageTaken));
        }
    }

    IEnumerator TriggerInvincibility(float _damageTaken)
    {
        p_state = State.Invincible;
        timer.secondsLeft -= _damageTaken;
        countdownTimerUI.text = timer.secondsLeft.ToString();
        yield return new WaitForSeconds(invincibleTime);
        p_state = State.Normal;
    }

    IEnumerator TriggerFlash()
    {
        for (float time = invincibleTime; time >= 0; time -= (flashRate * 2))
        {
            Debug.Log("Once");
            p_Controller.spriteRenderer.color = new Color(
                p_Controller.spriteRenderer.color.r,
                p_Controller.spriteRenderer.color.g,
                p_Controller.spriteRenderer.color.b,
                0);
            yield return new WaitForSeconds(flashRate);
            p_Controller.spriteRenderer.color = new Color(
                p_Controller.spriteRenderer.color.r,
                p_Controller.spriteRenderer.color.g,
                p_Controller.spriteRenderer.color.b,
                255);
            yield return new WaitForSeconds(flashRate);
        }
    }

enum State
    {
        Normal,
        Invincible,
    }
}
