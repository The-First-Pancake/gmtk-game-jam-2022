using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PoolResults {
    public int [] pips = new int[Enum.GetNames(typeof(PipType)).Length];
}

public class DicePoolManager : MonoBehaviour
{
    public List<Dice> diceInPool;
    public PoolResults poolResults;
    // Start is called before the first frame update
    void Start()
    {
        GetComponentsInChildren<Dice>(diceInPool);
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
                foreach (Pip pip in upFace) {
                    poolResults.pips[((int)pip.piptype)] += 1;
                }
            }
        }
        return poolResults;
    }
}
