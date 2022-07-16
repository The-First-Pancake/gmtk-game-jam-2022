using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CombatState {
    NOT_IN_COMBAT,
    ROLLING,
    RESOLUTION,
    END_COMBAT,
}

public class CombatManager : MonoBehaviour
{

    public CombatState state = CombatState.NOT_IN_COMBAT;
    public int numRerollsAllowed;

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
                break;
            case CombatState.RESOLUTION:
                break;
            case CombatState.END_COMBAT:
                break;
        }
    }

    void StartCombat() {
        if (state == CombatState.NOT_IN_COMBAT) {
            state = CombatState.ROLLING;
            foreach (DiceRoller diceRoller in GameManager.instance.player.dicePool.diceRollers) {
                
            }
        }
    }
}
