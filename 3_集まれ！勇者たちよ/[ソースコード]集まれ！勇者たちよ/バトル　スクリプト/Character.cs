using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public new string name;

    public float attackPower;
    public float healingPower;
    public float defensePower;

    public float maxMoves = 1;
    public float currentMoves; //currentMoves = moves in Start()

    public float maxHp;
    public float currentHP;
    public float maxMana;
    public float currentMana;

    [HideInInspector] public Skill[] skills;
    [HideInInspector] public HealthBar hpBar;
    
	private void Start()
	{
        skills = GetComponents<Skill>();
        hpBar = GetComponentInChildren<HealthBar>();

        hpBar.UpdateUISliders(currentHP, maxHp, currentMana, maxMana);

        currentMoves = maxMoves;
    }

    public bool IsDead()
	{
        if (currentHP <= 0)
        {return true;} else {return false;}
	}
}