using UnityEngine;

public class Character : MonoBehaviour
{
    public Sprite m_sprite;
    public int characterID;
    public string characterName;

    public float timingSpeed; // Range: 0 ~ 10 (0 = Extremely Fast, 10 = Extremely Slow)
    [Range(0f, 1000f)]
    public float timingLocation; //Location of the Slider in Timing Phase. Range: 0 ~ 500 (Middle is SliderWidth / 2 (250))
    [Range(0f, 1000f)]
    public float timingWidth;   //Width of the Slider in Timing Phase. Range from 5 ~ SliderWidth (500)
    public float timingScoreSet;  //Score displayed that's right on Critical Point

    public char[] mash_keys = new char[0];
    public float[] mash_waitTime;

    public int powerLevel;
    public int battingLevel;
    [TextArea(1, 2)]
    public string techniqueText;

    public Vector3 rankingAnchoredPosition;
    public Vector2 rankingSizeDelta;

    public Vector3 gameAnchoredPosition;
    public Vector2 gameSizeDelta;

    public Vector3 gameTransformPos;
    public Vector3 gameTransformScale;
}
