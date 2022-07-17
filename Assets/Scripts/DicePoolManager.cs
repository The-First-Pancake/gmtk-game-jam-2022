using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PoolResults {
    public int [] pips = new int[Enum.GetNames(typeof(PipType)).Length];
    public int gun_combat;
    public int lightning_combat;
    public int fire_combat;

    public int best_combat;
}

public class DicePoolManager : MonoBehaviour
{
    public List<Dice> diceInPool;
    public List<DiceRoller> diceRollers;
    public PoolResults poolResults;
    // Start is called before the first frame update
    void Start()
    {
        GetComponentsInChildren<Dice>(diceInPool);
        GetComponentsInChildren<DiceRoller>(diceRollers);
    }

    // Update is called once per frame
    void Update()
    {
        poolResults = GetPoolResults();
    }

    void AddDieToPool(GameObject die) {
        // add die to children
        die.transform.parent = transform;
        diceInPool.Add(die.GetComponent<Dice>());
    }

    void RemoveDieFromPool(GameObject die) {
        die.transform.parent = null;
        diceInPool.Remove(die.GetComponent<Dice>());
    }

    public PoolResults GetPoolResults() {
        PoolResults poolResults = new PoolResults();
        foreach (Dice die in diceInPool) {
            Face upFace = die.getUpFace();
            if (upFace != null) {
                foreach (Pip pip in upFace.pips) {
                    poolResults.pips[((int)pip.type)] += 1;
                }
            }
        }
        poolResults.gun_combat = poolResults.pips[((int)PipType.Bullet)] * poolResults.pips[((int)PipType.Gun)];
        poolResults.lightning_combat = poolResults.pips[((int)PipType.Energy)] * poolResults.pips[((int)PipType.Energy)];
        poolResults.fire_combat = poolResults.pips[((int)PipType.Fire)];

        poolResults.best_combat = Math.Max(poolResults.gun_combat, Math.Max(poolResults.lightning_combat, poolResults.fire_combat));

        return poolResults;
    }
}
