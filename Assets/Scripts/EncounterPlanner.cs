using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct Encounter {
    public CombatThresholds thresholds;
    public GameObject enemyPrefab;
    public bool hasDiceReward;
}

public class EncounterPlanner : MonoBehaviour
{
    public List<Encounter> encounterPlan;
    public Encounter currentEncounter;
    private int encounterIndex = 0;
    private bool loadNextEncounter = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (loadNextEncounter && encounterIndex < encounterPlan.Count) {
            loadNextEncounter = false;
            currentEncounter = encounterPlan[encounterIndex];
            GameManager.instance.combatManager.StartCombat(currentEncounter.thresholds);
            encounterIndex++;
        }
    }

    public void LoadNextEncounter() {
        loadNextEncounter = true;
    }
}
