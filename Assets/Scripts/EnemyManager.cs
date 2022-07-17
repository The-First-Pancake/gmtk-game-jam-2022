using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    private GameObject Enemy;
    public GameObject DeathParticles;
    public GameObject EnemyPrefab;
    private ParticleSystem DeathParticlesSystem;


    // Start is called before the first frame update
    void Start()
    {
        Enemy = GameObject.FindGameObjectWithTag("Enemy");
        DeathParticlesSystem = DeathParticles.GetComponent<ParticleSystem>();
    }

    public void KillEnemy() {
        Destroy(Enemy);
        DeathParticlesSystem.Play();
        Debug.Log("Invoking NewEnemy()");
        Invoke("NewEnemy", 2f);
    }

    public void NewEnemy() {
        Debug.Log("Instantiaing new enemy");
        Instantiate(EnemyPrefab, new Vector3(260, 9, 100), Quaternion.Euler(0, -90, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
