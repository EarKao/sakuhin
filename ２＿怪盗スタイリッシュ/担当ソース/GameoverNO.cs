using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;　　　//ボタンを使用するのでUI
using UnityEngine.SceneManagement;//SceneManagerを使用ためSceneManagementを追加

public class GameoverNO : MonoBehaviour
{

    // ボタンをクリックするとScene『Mein Scene』に移動
    public void ButtonClick()
    {
        //3秒後にCall関数を実行する
        Invoke("Call", 0.5f);
    }

    void Call()
    {
        SceneManager.LoadScene("Title Scene");
    }
}