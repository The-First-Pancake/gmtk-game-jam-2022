using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public enum CombatState {
    NOT_IN_COMBAT,
    ROLLING,
    WAIT_FOR_SETTLING,
    CHECK_SEARCH,
    WAIT_FOR_SEARCH_SELECT,
    RESOLUTION,
    WAIT_FOR_RESOLUTION_SELECT,
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

    public UnityEvent OnCombatStart;
    public UnityEvent OnSearch;
    public UnityEvent OnKillAlien;
    public UnityEvent OnSneak;
    public UnityEvent OnPlayerHit;
    public UnityEvent OnCombatEnd;
    private PoolResults dicePoolResults;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndCombat();
        }

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
                    // Check loot
                    dicePoolResults = GameManager.instance.player.dicePool.GetPoolResults();
                    state = CombatState.CHECK_SEARCH;
                }
                break;
            case CombatState.CHECK_SEARCH:
                if (dicePoolResults.pips[((int)PipType.Search)] >= combatThresholds.search) {
                    Debug.Log("Got Lunch");
                    OnSearch.Invoke();
                    state = CombatState.WAIT_FOR_SEARCH_SELECT;
                } else {
                    state = CombatState.RESOLUTION;
                }
                break;
            case CombatState.WAIT_FOR_SEARCH_SELECT:
                break;
            case CombatState.RESOLUTION:
                // dicePoolResults = GameManager.instance.player.dicePool.GetPoolResults();
                if (dicePoolResults.best_combat >= combatThresholds.combat) {
                    Debug.Log("Killed Alien");
                    OnKillAlien.Invoke();
                    // Call victory reward / kill animation function
                    state = CombatState.WAIT_FOR_RESOLUTION_SELECT;
                } else if (dicePoolResults.pips[((int)PipType.Evade)] >= combatThresholds.evade) {
                    Debug.Log("Snuck Past");
                    OnSneak.Invoke();
                    // Call sneak animation
                    state = CombatState.END_COMBAT;
                } else {
                    Debug.Log("Got Hit");
                    OnPlayerHit.Invoke();
                    // Call harm
                    state = CombatState.WAIT_FOR_RESOLUTION_SELECT;
                }
                break;
            case CombatState.WAIT_FOR_RESOLUTION_SELECT:
                break;
            case CombatState.END_COMBAT:
                // Nothing to do yet, but I bet there will be
                OnCombatEnd.Invoke();
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

    private void StartCombat(CombatThresholds thresholds) {
        if (state == CombatState.NOT_IN_COMBAT) {
            OnCombatStart.Invoke();
            combatThresholds = thresholds;
            numRollsLeft = numRerollsAllowed;
            foreach (DiceRoller diceRoller in GameManager.instance.player.dicePool.diceRollers) {
                diceRoller.SetEnabled(true);
            }
            state = CombatState.ROLLING;
        }
    }

    public void StartCombatFromEncounter() {
        StartCombat(GameManager.instance.encounterPlanner.currentEncounter.thresholds);
    }

    public void EndCombat() {
        if (state == CombatState.ROLLING || state == CombatState.WAIT_FOR_SETTLING) {
            foreach (DiceRoller diceRoller in GameManager.instance.player.dicePool.diceRollers) {
                diceRoller.SetEnabled(false);
            }
            // Check loot
            dicePoolResults = GameManager.instance.player.dicePool.GetPoolResults();
            state = CombatState.CHECK_SEARCH;
        }
    }

    public void EndSelect() {
        if (state == CombatState.WAIT_FOR_SEARCH_SELECT) {
            state = CombatState.RESOLUTION;
        } else if (state == CombatState.WAIT_FOR_RESOLUTION_SELECT) {
            state = CombatState.END_COMBAT;
        }
    }
}
