using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CombatState {
    NOT_IN_COMBAT,
    ROLLING,
    WAIT_FOR_SETTLING,
    RESOLUTION,
    END_COMBAT,
}

[Serializable]
public struct CombatThresholds {
    public int combat;
    public int evade;
    public int search;
}

public class CombatManager : MonoBehaviour
{

    public CombatState state = CombatState.NOT_IN_COMBAT;
    public int numRerollsAllowed = 3;
    public int numRollsLeft;
    public CombatThresholds combatThresholds;

    private GameObject[] enemies;
    private ParticleSystem enemyDeathParticle;

    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyDeathParticle = GameObject.FindGameObjectsWithTag("Enemy Death Particle")[0].GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case CombatState.NOT_IN_COMBAT:
                break;
            case CombatState.ROLLING:
                if (Input.GetMouseButtonDown(1)) {
                    numRollsLeft -= 1;
                    state = CombatState.WAIT_FOR_SETTLING;
                }
                break;
            case CombatState.WAIT_FOR_SETTLING:
                foreach (DiceRoller diceRoller in GameManager.instance.player.dicePool.diceRollers) {
                    if (!diceRoller.IsSettled()) {
                        return;
                    }
                }
                if (numRollsLeft > 0) {
                    state = CombatState.ROLLING;
                } else {
                    foreach (DiceRoller diceRoller in GameManager.instance.player.dicePool.diceRollers) {
                        diceRoller.SetEnabled(false);
                    }
                    state = CombatState.RESOLUTION;
                }
                break;
            case CombatState.RESOLUTION:
                PoolResults dicePoolResults = GameManager.instance.player.dicePool.GetPoolResults();
                // Check loot
                if (dicePoolResults.pips[((int)PipType.Search)] >= combatThresholds.search) {
                    Debug.Log("Got Lunch");
                    // Call loot reward
                }
                if (dicePoolResults.best_combat >= combatThresholds.combat) {
                    Debug.Log("Killed Alien");
                    Destroy(enemies[0]);
                    enemyDeathParticle.Play(true);
                    // Call victory reward / kill animation function
                } else if (dicePoolResults.pips[((int)PipType.Evade)] >= combatThresholds.evade) {
                    Debug.Log("Snuck Past");
                    // Call sneak animation
                } else {
                    Debug.Log("Got Hit");
                    // Call harm
                }
                state = CombatState.END_COMBAT;
                break;
            case CombatState.END_COMBAT:
                // Nothing to do yet, but I bet there will be
                state = CombatState.NOT_IN_COMBAT;
                break;
        }
    }

    [ContextMenu("Start Combat")]
    public void TestCombat() {
        CombatThresholds testCombat = new CombatThresholds();
        testCombat.combat = 2;
        testCombat.evade = 3;
        testCombat.search = 2;
        StartCombat(testCombat);
    }

    public void StartCombat(CombatThresholds thresholds) {
        if (state == CombatState.NOT_IN_COMBAT) {
            combatThresholds = thresholds;
            numRollsLeft = numRerollsAllowed;
            foreach (DiceRoller diceRoller in GameManager.instance.player.dicePool.diceRollers) {
                diceRoller.SetEnabled(true);
            }
            state = CombatState.ROLLING;
        }
    }

    public void EndCombat() {
        if (state == CombatState.ROLLING || state == CombatState.WAIT_FOR_SETTLING) {
            foreach (DiceRoller diceRoller in GameManager.instance.player.dicePool.diceRollers) {
                diceRoller.SetEnabled(false);
            }
            state = CombatState.RESOLUTION;
        }
    }
}
