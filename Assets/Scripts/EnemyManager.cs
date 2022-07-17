using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{

    private GameObject Enemy;
    private GameObject DeathParticles;
    public GameObject EnemyPrefab;
    public GameObject GameScene;
    public GameObject EnemySceneMarker;
    private ParticleSystem DeathParticlesSystem;
    public UnityEvent OnNewEnemy;



    // Start is called before the first frame update
    void Start()
    {
        Enemy = GameObject.FindGameObjectWithTag("Enemy");
        DeathParticles = GameObject.FindGameObjectWithTag("Enemy Death Particle");
        DeathParticlesSystem = DeathParticles.GetComponent<ParticleSystem>();
    }

    public void KillEnemy() {
        Destroy(Enemy);
        DeathParticlesSystem.Play();
    }

    public void NewEnemy() {
        Debug.Log("Instantiaing new enemy");
        Instantiate(GameManager.instance.encounterPlanner.currentEncounter.enemyPrefab, GameScene.transform);
        Enemy = GameObject.FindGameObjectWithTag("Enemy");
        OnNewEnemy.Invoke();
    }

    private void EnemySlideIntoView() {
        if (Enemy != null && (Enemy.transform.position.x > EnemySceneMarker.transform.position.x)) {
            float speedOffset = (Enemy.transform.position.x / EnemySceneMarker.transform.position.x) * 0.1f;
            Enemy.transform.Translate(Vector3.forward * speedOffset);
        }
    }

    // Update is called once per frame
    void Update()
    {
        EnemySlideIntoView();
    }
}
