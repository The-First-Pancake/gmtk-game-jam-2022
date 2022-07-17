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
    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        dicePool = GetComponentInChildren<DicePoolManager>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage() {
        audio.Play();
        health--;
        if (health <= 0) {
            Die();
        }
    }

    void Die() {
        SceneManager.LoadScene(deathScene);
    }
}
