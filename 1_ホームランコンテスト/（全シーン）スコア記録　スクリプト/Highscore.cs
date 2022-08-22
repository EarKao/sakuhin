using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//保存するデータ
[System.Serializable]
public class Highscore
{
    public int characterID;
    public int distance;

    public Highscore(int _characterID, int _distance)
	{
        characterID = _characterID;
        distance = _distance;
	}
}
