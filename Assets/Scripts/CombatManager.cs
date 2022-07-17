using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatState {
    NOT_IN_COMBAT,
    ROLLING,
    WAIT_FOR_SETTLING,
    RESOLUTION,
    END_COMBAT,
}

public class CombatManager : MonoBehaviour
{

    public CombatState state = CombatState.NOT_IN_COMBAT;
    public int numRerollsAllowed = 3;
    public int numRollsLeft;

    // Start is called before the first frame update
    void Start()
    {
        
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
                        break;
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
                break;
            case CombatState.END_COMBAT:
                break;
        }
    }

    [ContextMenu("Start Combat")]
    public void StartCombat() {
        if (state == CombatState.NOT_IN_COMBAT) {
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
