using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;�@�@�@//�{�^�����g�p����̂�UI
using UnityEngine.SceneManagement;//SceneManager���g�p����SceneManagement��ǉ�

public class GameoverYES : MonoBehaviour
{

    // �{�^�����N���b�N�����Scene�wMein Scene�x�Ɉړ�
    public void ButtonClick()
    {
        //3�b���Call�֐������s����
        Invoke("Call", 0.5f);
    }

    void Call()
    {
        SceneManager.LoadScene("Main Scene");
    }
}