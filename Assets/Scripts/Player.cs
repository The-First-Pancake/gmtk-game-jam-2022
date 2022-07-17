using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int maxHealth = 5;
    public int health = 5;
    public DicePoolManager dicePool;
    public int deathScene;
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

    public void Damage() {
        health--;
        if (health <= 0) {
            Die();
        }
    }

    void Die() {
        SceneManager.LoadScene(deathScene);
    }
}
