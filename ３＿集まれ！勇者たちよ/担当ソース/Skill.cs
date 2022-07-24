using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
	Damage, 
	Heal
}
public enum BuffType
{
	Heal, 
	Damage, 
	AttackPowerUp,
	AttackPowerDown,
	DefensePowerUp,
	DefensePowerDown
}
public enum BuffTarget
{
	Target, 
	AllFriendly, 
	AllHostile
}
public enum BuffStats
{
	None,
	AttackPower,
	HealingPower,
	DefensePower,
	CurrentHP,
	MaxHP
}

public class Skill : MonoBehaviour
{
	// Add Effects for Attacking and Healing
	[TextArea(1, 2)]
	public string skillName;
	public Sprite skillSprite;
	public SkillType skillType;
	public float manaRecoverAmount;
	public float manaCost;

	public bool isAll;	// Handles Damage Dealing Process and Buffing Process

	[Header("Percentage Multiplier where 1 is 100%")]
	public float damageModifier;	// Character Base Damage x Damage Modifier x defenseModifier
	public float defenseModifier;	//// = Final Damage Amount (Basically don't ignore X% of armor)
	public float healingModifier;   // Character Base Healing x Healing Modifier = Final Healing Amount

	public bool hasBuff;
	public bool buff_IsAll;
	public bool buff_IsEnemy;
	public int buff_Turns;  //Used in BuffInfo [0]
	public float buff_Amount; //Used in BuffInfo [1]
	public float buff_Multiplier; //Used in BuffInfo [2]
	public float buff_AnimationLength; //Used in BuffInfo [3]
	public BuffStats buffStats; //Used in BuffInfo [4]

	public BuffType buffType; //Directly checked

	/* Start Reference */
	private Character m_character;

	private void Start()
	{
		m_character = GetComponent<Character>();
	}

	public void CastSkill(Character _target, List<Character> _hostiles, List<Character> _friendlies) //Target, relative to the attacker
	{
		ExecuteSkill(_target, _hostiles, _friendlies);
		UseMana();
		RecoverMana();
		UpdateLocalCharacter();

		void ExecuteSkill(Character _target, List<Character> _hostiles, List<Character> _friendlies)
		{
			switch (skillType)
			{
				case SkillType.Damage:
				{
					float finalDamageAmount = Mathf.Clamp(m_character.attackPower * damageModifier - _target.defensePower, 0, 9999f);
					finalDamageAmount = Random.Range(finalDamageAmount * 0.95f, finalDamageAmount * 1.05f);

					if (isAll)
					{
						foreach (Character c in _hostiles)
						{
							c.currentHP -= Random.Range(finalDamageAmount * 0.95f, finalDamageAmount *1.05f);
							UpdateTargetSliders(c);
						}
					} else
					{
						_target.currentHP -= Random.Range(finalDamageAmount * 0.95f, finalDamageAmount * 1.05f); ;
						UpdateTargetSliders(_target);
					}
					break;
				}

				case SkillType.Heal:
				{
					float finalHealingAmount = m_character.healingPower * healingModifier;

					if (isAll)
					{
						foreach (Character c in _friendlies)
						{
							if (c.currentHP + finalHealingAmount >= c.maxHp)
								finalHealingAmount = c.maxHp - c.currentHP;

							c.currentHP += finalHealingAmount;
							UpdateTargetSliders(c);
						}
					} else
					{
						if (_target.currentHP + finalHealingAmount >= _target.maxHp)
						{
							finalHealingAmount = _target.maxHp - _target.currentHP;
						}
						_target.currentHP += finalHealingAmount;
						UpdateTargetSliders(_target);
					}
					break;
				}
			}
		}
		#region Local Functions
		void RecoverMana()
		{
			m_character.currentMana = Mathf.Min(m_character.maxMana, m_character.currentMana + manaRecoverAmount);
		}
		void UpdateTargetSliders(Character c)
		{
			c.hpBar.UpdateUISliders(c.currentHP, c.maxHp, c.currentMana, c.maxMana);
			Debug.Log(c.name);
		}
		void UpdateLocalCharacter()
		{
			m_character.hpBar.UpdateUISliders(m_character.currentHP, m_character.maxHp, m_character.currentMana, m_character.maxMana);
		}
		void UseMana()
		{
			m_character.currentMana -= manaCost;
		}
		#endregion
	}

	public List<float> BuffInfo()
	{
		List<float> bI = new List<float>();

		#region Turns (Index = 0)
		bI.Add(buff_Turns);
		#endregion

		#region Amount (Index = 1)
		bI.Add(buff_Amount);
		#endregion

		#region Multiplier (Index = 2)
		bI.Add(buff_Multiplier);
		#endregion

		#region Animation Length (Index = 3)
		bI.Add(buff_AnimationLength);
		#endregion

		#region Buffer Stat Power (Index = 4)
		switch (buffStats)
		{
			case BuffStats.AttackPower:
				bI.Add(m_character.attackPower); break;
			case BuffStats.HealingPower:
				bI.Add(m_character.healingPower); break;
			case BuffStats.DefensePower:
				bI.Add(m_character.defensePower); break;
			case BuffStats.MaxHP:
				bI.Add(m_character.maxHp); break;
			case BuffStats.CurrentHP:
				bI.Add(m_character.currentHP); break;
			case BuffStats.None:
				bI.Add(0); break;
		}
		#endregion

		return bI;
	}
}
