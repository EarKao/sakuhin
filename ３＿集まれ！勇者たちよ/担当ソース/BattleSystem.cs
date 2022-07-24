using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { 
    Start, 
    PlayerTurn,
    EnemyTurn,
    Won,
    Lost
}


public class BattleSystem : MonoBehaviour
{
    public BattleState battleState;

    #region Buffs References
    List<Character> buffTarget = new List<Character>();    //A single target Character for the buff
    List<List<Character>> buffHostileList = new List<List<Character>>();  //A list of the Hostiles' party in a list
    List<List<Character>> buffAlliesList = new List<List<Character>> ();   //A list of the Allies' party in a list

    List<int> buffTurnBuffed = new List<int>();   //Turn it got buffed
    List<int> buffTurns = new List<int>();    //Turns the buff will last
    List<int> buffAmount = new List<int>();   //The Amount (+)
    List<float> buffMultiplier = new List<float>();   //The Multiplier (*)
    List<float> buffAnimationLength = new List<float>();   //Animation Length for Effects
    List<float> buffStatPower = new List<float>();   //The Buffers Stat Values
    List<bool> buffIsAll = new List<bool>();   //Is the Buff to Multiple Targets
    List<bool> buffIsEnemy = new List<bool>();   //Is the Buff to Friendly or Enemy
    List<BuffType> buffTypes = new List<BuffType>();   //Buff Types
    int buffCount = 0;   //The Total Number of Buffs
	#endregion



	public List<Character> playerC;
    public List<Character> currentEnemyC;
        
    public List<Character> Wave1EnemyC; //Reference Wave 1 2 3 GO instead
    public List<Character> Wave2EnemyC;
    public List<Character> Wave3EnemyC;

    private Character selectedPlayerC = null;   // The player character the Player selected
    private Character selectedEnemyC = null;    // The enemy character the Player selected
    private Skill selectedSkill = null;
    private Character targetedPlayerC = null;   // The player character the Player targeted

    public List<Button> SkillButton;
    public List<Text> SkillButtonText;

    private int currentWaveNum = 1;
    private int currentTurn = 1;

    public Text dialogueText;

    private void Start()
	{
        battleState = BattleState.Start;
        SetupBattle();

        StartCoroutine(GetMouseClickCharacter()); // Starts a neverending code, that checks once every frame (60fps)

	    void SetupBattle()
	    {
            ButtonControls(false, false, false);
        
            currentEnemyC = Wave1EnemyC;
            Wave1EnemyC[0].transform.parent.gameObject.SetActive(true);

            battleState = BattleState.PlayerTurn;
			StartCoroutine(PlayerTurn());
		}
    }

	private void Update()
	{
		if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
		{
            SceneManager.LoadScene("Main_Scene");
		}
	}

	IEnumerator GetMouseClickCharacter()
    {
        while (true) {
        if (Input.GetKeyDown(KeyCode.Mouse0) && battleState == BattleState.PlayerTurn) {

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero); // Casts a ray from the mouse to the screen
                if (hit)
                {
                    Character selectedHit = hit.collider.GetComponent<Character>(); // Return SelectedHit here. Everything belows goes to another method

                    if (selectedHit.tag == "Player")
                    {
                        if (selectedHit.currentMoves != 0 && selectedSkill == null) // Select a Character if no Skill is selected
                        {
                            selectedPlayerC = selectedHit;
                            bool isThirdSkillActive = false;
                            if (selectedPlayerC.currentMana == selectedPlayerC.maxMana) isThirdSkillActive = true;
                            ButtonControls(true, true, isThirdSkillActive);
                            SetSkillText(selectedPlayerC);
                            
                        }
                        else if (selectedSkill != null && selectedSkill.skillType == SkillType.Heal) //Cast a Skill if a Skill is selected and is a Healing Spell
                        {
                            targetedPlayerC = selectedHit;

                            ButtonControls(false, false, false);

                            List<float> skillList = selectedSkill.BuffInfo();
                            selectedSkill.CastSkill(targetedPlayerC, currentEnemyC, playerC);
                            if (selectedSkill.hasBuff) 
                                StartCoroutine(AddBuff(skillList, selectedSkill.buff_IsAll, selectedSkill.buff_IsEnemy, 
                                    selectedSkill.buffType, targetedPlayerC, currentEnemyC, playerC));

                            selectedPlayerC.currentMoves--;
                            selectedPlayerC = null; selectedEnemyC = null; selectedSkill = null;

                            //Does not check for CheckDead()
                            //Does not check for EndBattle()
                            CheckEndTurn(playerC);

                            yield return new WaitForSeconds(1f);
                        }
                    }
                    else if (selectedHit.tag == "Enemy")
                    {
                        //Does nothing if no Skill is selected
                        if (selectedSkill != null && selectedSkill.skillType != SkillType.Heal) //Cast a Skill if a Skill is selected and it's not a Healing Spell
                        {
                            selectedEnemyC = selectedHit;

                            ButtonControls(false, false, false);

                            List<float> skillList = selectedSkill.BuffInfo();

                            selectedSkill.CastSkill(selectedEnemyC, currentEnemyC, playerC);
                            if (selectedSkill.hasBuff) 
                                StartCoroutine(AddBuff(skillList, selectedSkill.buff_IsAll, selectedSkill.buff_IsEnemy, 
                                    selectedSkill.buffType, selectedEnemyC, currentEnemyC, playerC));

                            selectedPlayerC.currentMoves--;
                            selectedPlayerC = null; selectedEnemyC = null; selectedSkill = null;

                            CheckDead();
                            StartCoroutine(CheckEndBattle());
                            CheckEndTurn(playerC);

                            yield return new WaitForSeconds(0f);
                        }
                    }
                }
            }
        yield return null;
        }
    }

    IEnumerator PlayerTurn()
	{
        foreach(Character playerCharacter in playerC.ToArray())
		{
            playerCharacter.currentMoves = playerCharacter.maxMoves;
            // Grey out Skill Depleted Player Characters
        }

        yield return new WaitForSeconds(0f);
    }
    
    IEnumerator EnemyTurn()
	{
        yield return new WaitForSeconds(1f);

        foreach (Character enemyCharacter in currentEnemyC.ToArray())
        { enemyCharacter.currentMoves = enemyCharacter.maxMoves; }

        foreach (Character enemy in currentEnemyC.ToArray())
		{
            while (enemy.currentMoves != 0 && battleState == BattleState.EnemyTurn)
			{
                List<float> skillList = enemy.skills[0].BuffInfo();
                enemy.skills[0].CastSkill(playerC[0], playerC, currentEnemyC);
                if (enemy.skills[0].hasBuff)
                    StartCoroutine(AddBuff(skillList, selectedSkill.buff_IsAll, selectedSkill.buff_IsEnemy, 
                        selectedSkill.buffType, targetedPlayerC, currentEnemyC, playerC));

                enemy.currentMoves--;
                yield return new WaitForSeconds(1.5f);
                CheckDead();
                StartCoroutine(CheckEndBattle());
				CheckEndTurn(currentEnemyC);
			}
		}
	}

    IEnumerator AddBuff(List<float> _listBI, bool _isAll, bool _isEnemy, BuffType _buffType, Character _tar, List<Character> _hosL, List<Character> _allL)
	{
        buffTurnBuffed.Add(currentTurn); //Set the turn the target(s) is buffed

        buffTurns.Add((int)_listBI[0]); //Set how long the buff should last
        buffAmount.Add((int)_listBI[1]); // + amount
        buffMultiplier.Add(_listBI[2]); // * amount
        buffAnimationLength.Add(_listBI[3]); //Set the length for Animation
        buffStatPower.Add(_listBI[4]); // The stat used (AttackPower, DefensePower etc.)
        buffIsAll.Add(_isAll); //Is the buff towards all targets?
        buffIsEnemy.Add(_isEnemy); //Is the buff towards enemies?

		buffTypes.Add(_buffType); //Set the Buff Type (Health, Attack, Defense)

		buffTarget.Add(_tar); //Set the target for the buff
        buffHostileList.Add(_hosL); //Hostile Party, relative to the attacker
        buffAlliesList.Add(_allL); //Ally Party, relative to the attacker

        #region Debug.Logs
        //Debug.Log("buffTurnBuffed: " + buffTurnBuffed[0]);

        //Debug.Log("buffTurns: " + buffTurns[0]);
        //Debug.Log("buffAmount: " + buffAmount[0]);
        //Debug.Log("buffMultiplier: " + buffMultiplier[0]);
        //Debug.Log("buffAnimationLength: " + buffAnimationLength[0]);
        //Debug.Log("buffStatPower: " + buffStatPower[0]);
        //Debug.Log("buffIsAll: " + buffIsAll[0]);
        //Debug.Log("buffIsEnemy: " + buffIsEnemy[0]);

        //Debug.Log("buffTypes: " + buffTypes[0]);

        //Debug.Log("buffTarget: " + buffTarget[0]);
        //Debug.Log("buffHostileList: " + buffHostileList[0].Count);
        //Debug.Log("buffAlliesList: " + buffAlliesList[0].Count);
        #endregion

        switch (buffTypes[buffCount])
		{
			case BuffType.AttackPowerUp:
			{
				if (buffIsEnemy[buffCount]) //The buff is towards hostiles
				{
					if (buffIsAll[buffCount]) //The buff is towards all hostiles
					{
						if (buffHostileList[buffCount].Count != 0) //Check hostile Count
						{
							foreach (Character c in buffHostileList[buffCount]) //Increase Attack Power
							{
								c.attackPower *= buffMultiplier[buffCount];
								c.attackPower += buffAmount[buffCount];
							}
							//Effects
							yield return new WaitForSeconds(buffAnimationLength[buffCount]);
						}
					} else //The buff is towards one hostile
					{
						if (buffTarget[buffCount] != null) //Increase Attack Power
						{
							buffTarget[buffCount].attackPower *= buffMultiplier[buffCount];
							buffTarget[buffCount].attackPower += buffAmount[buffCount];
						}
					}
				} else //The buff is towards allies
				{
					if (buffIsAll[buffCount]) //The buff is towards all allies
					{
						if (buffAlliesList[buffCount].Count != 0)
						{
							foreach (Character c in buffAlliesList[buffCount]) //Increase Attack Power
							{
								c.attackPower *= buffMultiplier[buffCount];
								c.attackPower += buffAmount[buffCount];
							}
							//Effects
							yield return new WaitForSeconds(buffAnimationLength[buffCount]);
						}
					} else //The buff is towards one ally
					{
						if (buffTarget[buffCount] != null) //Increase Attack Power
						{
							buffTarget[buffCount].attackPower *= buffMultiplier[buffCount];
							buffTarget[buffCount].attackPower += buffAmount[buffCount];
						}
					}
				}
				break;
			}
			case BuffType.AttackPowerDown:
			{
				if (buffIsEnemy[buffCount]) //The debuff is towards hostiles
				{
					if (buffIsAll[buffCount]) //The debuff is towards all hostiles
					{
						if (buffHostileList[buffCount].Count != 0) //Check hostile Count
						{
							foreach (Character c in buffHostileList[buffCount]) //Decrease Attack Power
							{
								c.attackPower /= buffMultiplier[buffCount];
								c.attackPower -= buffAmount[buffCount];
							}
							//Effects
							yield return new WaitForSeconds(buffAnimationLength[buffCount]);
						}
					} else //The debuff is towards one hostile
					{
						if (buffTarget[buffCount] != null) //Decrease Attack Power
						{
							buffTarget[buffCount].attackPower /= buffMultiplier[buffCount];
							buffTarget[buffCount].attackPower -= buffAmount[buffCount];
						}
					}
				} else //The debuff is towards allies
				{
					if (buffIsAll[buffCount]) //The debuff is towards all allies
					{
						if (buffAlliesList[buffCount].Count != 0)
						{
							foreach (Character c in buffAlliesList[buffCount]) //Decrease Attack Power
							{
								c.attackPower /= buffMultiplier[buffCount];
								c.attackPower -= buffAmount[buffCount];
							}
							//Effects
							yield return new WaitForSeconds(buffAnimationLength[buffCount]);
						}
					} else //The debuff is towards one ally
					{
						if (buffTarget[buffCount] != null) //Decrease Attack Power
						{
							buffTarget[buffCount].attackPower /= buffMultiplier[buffCount];
							buffTarget[buffCount].attackPower -= buffAmount[buffCount];
						}
					}
				}
				break;
			}
			case BuffType.DefensePowerUp:
			{
				if (buffIsEnemy[buffCount]) //The buff is towards hostiles
				{
					if (buffIsAll[buffCount]) //The buff is towards all hostiles
					{
						if (buffHostileList[buffCount].Count != 0) //Check hostile Count
						{
							foreach (Character c in buffHostileList[buffCount]) //Increase Defense Power
							{
								c.defensePower *= buffMultiplier[buffCount];
								c.defensePower += buffAmount[buffCount];
							}
							//Effects
							yield return new WaitForSeconds(buffAnimationLength[buffCount]);
						}
					} else //The buff is towards one hostile
					{
						if (buffTarget[buffCount] != null) //Increase Defense Power
						{
							buffTarget[buffCount].defensePower *= buffMultiplier[buffCount];
							buffTarget[buffCount].defensePower += buffAmount[buffCount];
						}
					}
				} else //The buff is towards allies
				{
					if (buffIsAll[buffCount]) //The buff is towards all allies
					{
						if (buffAlliesList[buffCount].Count != 0)
						{
							foreach (Character c in buffAlliesList[buffCount]) //Increase Defense Power
							{
								c.defensePower *= buffMultiplier[buffCount];
								c.defensePower += buffAmount[buffCount];
							}
							//Effects
							yield return new WaitForSeconds(buffAnimationLength[buffCount]);
						}
					} else //The buff is towards one ally
					{
						if (buffTarget[buffCount] != null) //Increase Defense Power
						{
							buffTarget[buffCount].defensePower *= buffMultiplier[buffCount];
							buffTarget[buffCount].defensePower += buffAmount[buffCount];
						}
					}
				}
				break;
			}
			case BuffType.DefensePowerDown:
			{
				if (buffIsEnemy[buffCount]) //The debuff is towards hostiles
				{
					if (buffIsAll[buffCount]) //The debuff is towards all hostiles
					{
						if (buffHostileList[buffCount].Count != 0) //Check hostile Count
						{
							foreach (Character c in buffHostileList[buffCount]) //Decrease Defense Power
							{
								c.defensePower /= buffMultiplier[buffCount];
								c.defensePower -= buffAmount[buffCount];
							}
							//Effects
							yield return new WaitForSeconds(buffAnimationLength[buffCount]);
						}
					} else //The debuff is towards one hostile
					{
						if (buffTarget[buffCount] != null) //Decrease Defense Power
						{
							buffTarget[buffCount].defensePower /= buffMultiplier[buffCount];
							buffTarget[buffCount].defensePower -= buffAmount[buffCount];
						}
					}
				} else //The debuff is towards allies
				{
					if (buffIsAll[buffCount]) //The debuff is towards all allies
					{
						if (buffAlliesList[buffCount].Count != 0)
						{
							foreach (Character c in buffAlliesList[buffCount]) //Decrease Defense Power
							{
								c.defensePower /= buffMultiplier[buffCount];
								c.defensePower -= buffAmount[buffCount];
							}
							//Effects
							yield return new WaitForSeconds(buffAnimationLength[buffCount]);
						}
					} else //The debuff is towards one ally
					{
						if (buffTarget[buffCount] != null) //Decrease Defense Power
						{
							buffTarget[buffCount].defensePower /= buffMultiplier[buffCount];
							buffTarget[buffCount].defensePower -= buffAmount[buffCount];
						}
					}
				}
				break;
			}
		}

        buffCount++; //Increase the total buffCount by 1
    }

    IEnumerator ExecuteBuffs() //Called at the end of the Player's Turn
	{
        yield return new WaitForSeconds(1f);

		for (int i = 0; i < buffCount; i++)
		{
            if (buffTypes[i] == BuffType.Heal) //The buff increases characters current hp
            {
                if (currentTurn - buffTurnBuffed[i] < buffTurns[i]) //The turns the buff is active
                {
                    if (buffIsEnemy[i]) //The buff is towards hostiles
                    {
                        if (buffIsAll[i]) //The buff is towards all hostiles
                        {
                            if (buffHostileList[i].Count != 0) //Check hostile Count
                            {
                                foreach (Character c in buffHostileList[i].ToArray()) //Adds hp
                                {
                                    c.currentHP += buffStatPower[i] * buffMultiplier[i] + buffAmount[i];
                                    c.currentHP = Mathf.Min(c.currentHP, c.maxHp);
                                    c.hpBar.UpdateUISliders(c.currentHP, c.maxHp, c.currentMana, c.maxMana);
                                }
                                //Effects
                                yield return new WaitForSeconds(buffAnimationLength[i]);
                            }
                        } else //The buff is towards one hostile
                        {
                            if (buffTarget[i] != null) //Adds hp
                            {
                                buffTarget[i].currentHP += buffStatPower[i] * buffMultiplier[i] + buffAmount[i];
                                buffTarget[i].currentHP = Mathf.Min(buffTarget[i].currentHP, buffTarget[i].maxHp);
                                buffTarget[i].hpBar.UpdateUISliders(buffTarget[i].currentHP, buffTarget[i].maxHp, buffTarget[i].currentMana, buffTarget[i].maxMana);
                            }
                        }
                    } else //The buff is towards allies
                    {
                        if (buffIsAll[i]) //The buff is towards all allies
                        {
                            if (buffAlliesList[i].Count != 0)
                            {
                                foreach (Character c in buffAlliesList[i].ToArray()) //Adds hp
                                {
                                    c.currentHP += buffStatPower[i] * buffMultiplier[i] + buffAmount[i];
                                    //Debug.Log("Healing Buff healed " + (buffStatPower[i] * buffMultiplier[i] + buffAmount[i]) + " to " + c.name);
                                    c.currentHP = Mathf.Min(c.currentHP, c.maxHp);
                                    c.hpBar.UpdateUISliders(c.currentHP, c.maxHp, c.currentMana, c.maxMana);
                                }
                                //Effects
                                yield return new WaitForSeconds(buffAnimationLength[i]);
                            }
                        } else //The buff is towards one ally
                        {
                            if (buffTarget[i] != null) //Adds hp
                            {
                                buffTarget[i].currentHP += buffStatPower[i] * buffMultiplier[i] + buffAmount[i];
                                buffTarget[i].currentHP = Mathf.Min(buffTarget[i].currentHP, buffTarget[i].maxHp);
                                buffTarget[i].hpBar.UpdateUISliders(buffTarget[i].currentHP, buffTarget[i].maxHp, buffTarget[i].currentMana, buffTarget[i].maxMana);
                            }
                        }
                    }
                }
            }
            else if (buffTypes[i] == BuffType.Damage) //The debuff increases characters current hp
            {
                if (currentTurn - buffTurnBuffed[i] < buffTurns[i]) //The turns the debuff is active
                {
                    if (buffIsEnemy[i]) //The debuff is towards hostiles
                    {
                        if (buffIsAll[i]) //The debuff is towards all hostiles
                        {
                            if (buffHostileList[i].Count != 0) //Check hostile Count
                            {
                                foreach (Character c in buffHostileList[i].ToArray()) //Reduce hp
                                {
                                    c.currentHP -= buffStatPower[i] * buffMultiplier[i] + buffAmount[i];
                                    c.hpBar.UpdateUISliders(c.currentHP, c.maxHp, c.currentMana, c.maxMana);
                                    CheckDead();
                                    StartCoroutine(CheckEndBattle());
                                }
                                //Effects
                                yield return new WaitForSeconds(buffAnimationLength[i]);
                            }
                        } else //The debuff is towards one hostile
                        {
                            if (buffTarget[i] != null) //Reduce hp
                            {
                                buffTarget[i].currentHP -= buffStatPower[i] * buffMultiplier[i] + buffAmount[i];
                                buffTarget[i].hpBar.UpdateUISliders(buffTarget[i].currentHP, buffTarget[i].maxHp, buffTarget[i].currentMana, buffTarget[i].maxMana);
                                CheckDead();
                                StartCoroutine(CheckEndBattle());
                            }
                        }
                        CheckDead();
                    } else //The debuff is towards allies
                    {
                        if (buffIsAll[i]) //The debuff is towards all allies
                        {
                            if (buffAlliesList[i].Count != 0)
                            {
                                foreach (Character c in buffAlliesList[i].ToArray()) //Reduce hp
                                {
                                    c.currentHP -= buffStatPower[i] * buffMultiplier[i] + buffAmount[i];
                                    c.hpBar.UpdateUISliders(c.currentHP, c.maxHp, c.currentMana, c.maxMana);
                                    CheckDead();
                                    StartCoroutine(CheckEndBattle());
                                }
                                //Effects
                                yield return new WaitForSeconds(buffAnimationLength[i]);
                            }
                        } else //The debuff is towards one ally
                        {
                            if (buffTarget[i] != null) //Reduce hp
                            {
                                buffTarget[i].currentHP -= buffStatPower[i] * buffMultiplier[i] + buffAmount[i];
                                buffTarget[i].hpBar.UpdateUISliders(buffTarget[i].currentHP, buffTarget[i].maxHp, buffTarget[i].currentMana, buffTarget[i].maxMana);
                                CheckDead();
                                StartCoroutine(CheckEndBattle());
                            }
                        }
                        CheckDead();
                    }
                }
            }
            else if (buffTypes[i] == BuffType.AttackPowerUp)
			{
                if (currentTurn - buffTurnBuffed[i] == buffTurns[i] - 1) //The turn the buff should be reverted
                {
                    if (buffIsEnemy[i]) //The buff is towards hostiles
                    {
                        if (buffIsAll[i]) //The buff is towards all hostiles
                        {
                            if (buffHostileList[i].Count != 0) //Check hostile Count
                            {
                                foreach (Character c in buffHostileList[i].ToArray()) //Reduce added amount
                                {
                                    c.attackPower -= buffAmount[i];
                                    c.attackPower /= buffMultiplier[i];
                                }
                                //Effects
                                yield return new WaitForSeconds(buffAnimationLength[i]);
                            }
                        } else //The buff is towards one hostile
                        {
                            if (buffTarget[i] != null) //Reduce added amount
                            {
                                buffTarget[i].attackPower -= buffAmount[i];
                                buffTarget[i].attackPower /= buffMultiplier[i];
                            }
                        }
                    } else //The buff is towards allies
                    {
                        if (buffIsAll[i]) //The buff is towards all allies
                        {
                            if (buffAlliesList[i].Count != 0)
                            {
                                foreach (Character c in buffAlliesList[i].ToArray()) //Reduce added amount
                                {
                                    c.attackPower -= buffAmount[i];
                                    c.attackPower /= buffMultiplier[i];
                                }
                                //Effects
                                yield return new WaitForSeconds(buffAnimationLength[i]);
                            }
                        } else //The buff is towards one ally
                        {
                            if (buffTarget[i] != null) //Reduce added amount
                            {
                                buffTarget[i].attackPower -= buffAmount[i];
                                buffTarget[i].attackPower /= buffMultiplier[i];
                            }
                        }
                    }
                }
            }
            else if (buffTypes[i] == BuffType.AttackPowerDown) //The debuff decrease characters Attack Power
            {
                if (currentTurn - buffTurnBuffed[i] == buffTurns[i] - 1) //The turn the debuff should be reverted
                {
                    if (buffIsEnemy[i]) //The debuff is towards hostiles
                    {
                        if (buffIsAll[i]) //The debuff is towards all hostiles
                        {
                            if (buffHostileList[i].Count != 0) //Check hostile Count
                            {
                                foreach (Character c in buffHostileList[i].ToArray()) //Add back reduced amount
                                {
                                    c.attackPower += buffAmount[i];
                                    c.attackPower *= buffMultiplier[i];
                                }
                                //Effects
                                yield return new WaitForSeconds(buffAnimationLength[i]);
                            }
                        } else //The debuff is towards one hostile
                        {
                            if (buffTarget[i] != null) //Add back reduced amount
                            {
                                buffTarget[i].attackPower += buffAmount[i];
                                buffTarget[i].attackPower *= buffMultiplier[i];
                            }
                        }
                    } else //The debuff is towards allies
                    {
                        if (buffIsAll[i]) //The debuff is towards all allies
                        {
                            if (buffAlliesList[i].Count != 0)
                            {
                                foreach (Character c in buffAlliesList[i].ToArray()) //Add back reduced amount
                                {
                                    c.attackPower += buffAmount[i];
                                    c.attackPower *= buffMultiplier[i];
                                }
                                //Effects
                                yield return new WaitForSeconds(buffAnimationLength[i]);
                            }
                        } else //The debuff is towards one ally
                        {
                            if (buffTarget[i] != null) //Add back reduced amount
                            {
                                buffTarget[i].attackPower += buffAmount[i];
                                buffTarget[i].attackPower *= buffMultiplier[i];
                            }
                        }
                    }
                }
            }
            else if (buffTypes[i] == BuffType.DefensePowerUp)
			{
                {
                    if (currentTurn - buffTurnBuffed[i] == buffTurns[i] - 1) //The turn the buff should be reverted
                    {
                        if (buffIsEnemy[i]) //The buff is towards hostiles
                        {
                            if (buffIsAll[i]) //The buff is towards all hostiles
                            {
                                if (buffHostileList[i].Count != 0) //Check hostile Count
                                {
                                    foreach (Character c in buffHostileList[i].ToArray()) //Reduce added amount
                                    {
                                        c.defensePower -= buffAmount[i];
                                        c.defensePower /= buffMultiplier[i];
                                    }
                                    //Effects
                                    yield return new WaitForSeconds(buffAnimationLength[i]);
                                }
                            } else //The buff is towards one hostile
                            {
                                if (buffTarget[i] != null) //Reduce added amount
                                {
                                    buffTarget[i].defensePower -= buffAmount[i];
                                    buffTarget[i].defensePower /= buffMultiplier[i];
                                }
                            }
                        } else //The buff is towards allies
                        {
                            if (buffIsAll[i]) //The buff is towards all allies
                            {
                                if (buffAlliesList[i].Count != 0)
                                {
                                    foreach (Character c in buffAlliesList[i].ToArray()) //Reduce added amount
                                    {
                                        c.defensePower -= buffAmount[i];
                                        c.defensePower /= buffMultiplier[i];
                                    }
                                    //Effects
                                    yield return new WaitForSeconds(buffAnimationLength[i]);
                                }
                            } else //The buff is towards one ally
                            {
                                if (buffTarget[i] != null) //Reduce added amount
                                {
                                    buffTarget[i].defensePower -= buffAmount[i];
                                    buffTarget[i].defensePower /= buffMultiplier[i];
                                }
                            }
                        }
                    }
                }
            }
            else if (buffTypes[i] == BuffType.DefensePowerDown)
			{
                if (currentTurn - buffTurnBuffed[i] == buffTurns[i] - 1) //The turn the debuff should be reverted
                {
                    if (buffIsEnemy[i]) //The debuff is towards hostiles
                    {
                        if (buffIsAll[i]) //The debuff is towards all hostiles
                        {
                            if (buffHostileList[i].Count != 0) //Check hostile Count
                            {
                                foreach (Character c in buffHostileList[i].ToArray()) //Add back reduced amount
                                {
                                    c.defensePower += buffAmount[i];
                                    c.defensePower *= buffMultiplier[i];
                                }
                                //Effects
                                yield return new WaitForSeconds(buffAnimationLength[i]);
                            }
                        } else //The debuff is towards one hostile
                        {
                            if (buffTarget[i] != null) //Add back reduced amount
                            {
                                buffTarget[i].defensePower += buffAmount[i];
                                buffTarget[i].defensePower *= buffMultiplier[i];
                            }
                        }
                    } else //The debuff is towards allies
                    {
                        if (buffIsAll[i]) //The debuff is towards all allies
                        {
                            if (buffAlliesList[i].Count != 0)
                            {
                                foreach (Character c in buffAlliesList[i].ToArray()) //Add back reduced amount
                                {
                                    c.defensePower += buffAmount[i];
                                    c.defensePower *= buffMultiplier[i];
                                }
                                //Effects
                                yield return new WaitForSeconds(buffAnimationLength[i]);
                            }
                        } else //The debuff is towards one ally
                        {
                            if (buffTarget[i] != null) //Add back reduced amount
                            {
                                buffTarget[i].defensePower += buffAmount[i];
                                buffTarget[i].defensePower *= buffMultiplier[i];
                            }
                        }
                    }
                }
            }
		}
        StartCoroutine(EnemyTurn());
    }



	#region Checker Functions
	IEnumerator CheckEndBattle()
	{
        
        if (playerC.Count == 0)
		{
            battleState = BattleState.Lost;
        } else if (currentEnemyC.Count == 0)
		{
            battleState = BattleState.Won;
		}

        yield return new WaitForSeconds(1f);

        if (battleState == BattleState.Won)
		{
            Debug.Log("That Wave was " + currentWaveNum);
            switch (currentWaveNum)
            {
                case 1:
                    currentWaveNum++;
                    currentEnemyC = Wave2EnemyC;
                    Wave2EnemyC[0].transform.parent.gameObject.SetActive(true);
                    Wave2EnemyC[0].transform.parent.GetComponent<Transform>().position = new Vector3(0, 0, 0);
                    StartCoroutine(PlayerTurn());
                    battleState = BattleState.PlayerTurn;
                    break;
                case 2:
                    currentWaveNum++;
                    currentEnemyC = Wave3EnemyC;
                    Wave3EnemyC[0].transform.parent.gameObject.SetActive(true);
                    Wave3EnemyC[0].transform.parent.GetComponent<Transform>().position = new Vector3(0, 0, 0);
                    StartCoroutine(PlayerTurn());
                    battleState = BattleState.PlayerTurn;
                    break;
                case 3:
                    yield return new WaitForSeconds(1f);
                    SceneManager.LoadScene("Main_Scene");
                    break;
                default: break;
            }
            Debug.Log("This Wave is " + currentWaveNum);
        } else if (battleState == BattleState.Lost)
		{
            dialogueText.text = "ïâÇØÇƒÇµÇ‹Ç¡ÇΩÅBÅBÅB";
            SceneManager.LoadScene("Main_Scene");
		}
        ButtonControls(false, false, false);
    }

	void CheckEndTurn(List<Character> characters) //Checks if turn Ends for current team (Player or Enemy)
    {
        float totalMoves = 0;
        foreach (Character character in characters)
		{
            totalMoves += character.currentMoves;
		}

        if (totalMoves == 0)
		{
            if (battleState == BattleState.EnemyTurn)
			{
                battleState = BattleState.PlayerTurn;
                currentTurn++;
                StartCoroutine(PlayerTurn());
			} else if (battleState == BattleState.PlayerTurn)
			{
                battleState = BattleState.EnemyTurn;
                StartCoroutine(ExecuteBuffs()); //Buffs are called here
			}
		}
	}

    void CheckDead()
	{
        foreach (Character c in playerC.ToArray()) {
            if(c.IsDead()) {
                c.gameObject.SetActive(false);
            }
            playerC.RemoveAll(c => c.IsDead());
        }
        foreach (Character c in currentEnemyC.ToArray()) {
            if(c.IsDead()) {
                c.gameObject.SetActive(false);
            }
            currentEnemyC.RemoveAll(c => c.IsDead());
        }
    }
	#endregion



	#region Set Functions
	public void OnSkill_1_Button()
	{
        selectedSkill = selectedPlayerC.skills[0];
    }

    public void OnSkill_2_Button()
    {
        selectedSkill = selectedPlayerC.skills[1];
    }

    public void OnSkill_3_Button()
	{
        selectedSkill = selectedPlayerC.skills[2];
    }

    private void ButtonControls(bool sk1b, bool sk2b, bool sk3b)
	{
        if (sk1b) SkillButton[0].interactable = true; else SkillButton[0].interactable = false;
        if (sk2b) SkillButton[1].interactable = true; else SkillButton[1].interactable = false;
        if (sk3b) SkillButton[2].interactable = true; else SkillButton[2].interactable = false;
    }

    private void SetSkillText(Character selectedCharacter)
	{
        SkillButtonText[0].text = selectedCharacter.skills[0].skillName;
        SkillButton[0].GetComponent<Image>().sprite = selectedCharacter.skills[0].skillSprite;
        SkillButtonText[1].text = selectedCharacter.skills[1].skillName; ;
        SkillButton[1].GetComponent<Image>().sprite = selectedCharacter.skills[1].skillSprite;
        SkillButtonText[2].text = selectedCharacter.skills[2].skillName; ;
        SkillButton[2].GetComponent<Image>().sprite = selectedCharacter.skills[2].skillSprite;
    }
    #endregion

}