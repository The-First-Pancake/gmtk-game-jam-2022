using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 5;
    public int health = 5;
    public DicePoolManager dicePool;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        dicePool = GetComponentInChildren<DicePoolManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
