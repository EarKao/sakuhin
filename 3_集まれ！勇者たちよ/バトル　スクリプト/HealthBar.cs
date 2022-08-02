using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Vector3 offset;
    private Slider hpSlider;
    private Slider mpSlider;
	private Text hpText;
	private Text mpText;
	[SerializeField] private bool hasMana;

	private void Awake()
	{
		hpSlider = transform.GetComponentsInChildren<Slider>()[0];
		hpText = transform.GetComponentsInChildren<Text>()[0];

		if (hasMana)
		{
			mpSlider = transform.GetComponentsInChildren<Slider>()[1];
			mpText = transform.GetComponentsInChildren<Text>()[1];
		}
	}

	public void UpdateUISliders(float currentHealth, float maxHealth, float currentMana, float maxMana)
	{
		hpSlider.minValue = 0;
		hpSlider.maxValue = (int)maxHealth;
		hpSlider.value = (int)currentHealth;

		hpSlider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);

		hpText.text = hpSlider.value + " / " + hpSlider.maxValue;

		if (hasMana)
		{
			mpSlider.minValue = 0;
			mpSlider.maxValue = (int)maxMana;
			mpSlider.value = (int)currentMana;

			Vector3 mpOffset = new Vector3(0, -0.25f, 0);
			mpSlider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset + mpOffset);

			mpText.text = mpSlider.value + " / " + mpSlider.maxValue;
		}
	}
}
