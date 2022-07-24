using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameoverNO : MonoBehaviour
{
    public void ButtonClick()
    {
        Invoke("Call", 0.5f);
    }

    void Call()
    {
        SceneManager.LoadScene("Title Scene");
    }
}