using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
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
    public UnityEvent OnEncounterStart;
    private int encounterIndex = 0;
    private bool loadNextEncounter = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (loadNextEncounter) {
            if (encounterIndex < encounterPlan.Count) {
                loadNextEncounter = false;
                currentEncounter = encounterPlan[encounterIndex];
                OnEncounterStart.Invoke();
                encounterIndex++;
            } else {
                SceneManager.LoadScene(4);
            }
        }
        
    }

    public void LoadNextEncounter() {
        loadNextEncounter = true;
    }
}
