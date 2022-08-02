using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKey)
		{
            SceneManager.LoadScene("Main_Scene");
		}
    }
}