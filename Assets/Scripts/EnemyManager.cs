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
    public GameObject EnemySceneMarkerIn;
    public GameObject EnemySceneMarkerOut;
    private ParticleSystem DeathParticlesSystem;
    public UnityEvent OnNewEnemy;
    public Transform cam;
    private bool PlayerLost = false;
    public GameObject Flare;



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
        if (Enemy != null && (Enemy.transform.position.x > EnemySceneMarkerIn.transform.position.x)) {
            float speedOffset = (Enemy.transform.position.x / EnemySceneMarkerIn.transform.position.x) * 0.1f;
            Enemy.transform.Translate(Vector3.forward * speedOffset);
        }
    }

    private void EnemyRotateAway() {
        if (cam.InverseTransformPoint(Enemy.transform.position).z > -100 && cam.InverseTransformPoint(Enemy.transform.position).z <= 100) {
            Enemy.transform.Translate(new Vector3(0, 0, -1), Space.Self);
        }

        if (Enemy.transform.rotation.y < 90) {
            Enemy.transform.Rotate(Vector3.up, 1f);
        }
    }

    private void EnemySlideOutView() {
        if (Enemy != null && (Enemy.transform.position.x < EnemySceneMarkerOut.transform.position.x)) {
            float speedOffset = (EnemySceneMarkerOut.transform.position.x / Enemy.transform.position.x) * 0.1f;
            Enemy.transform.Translate(Vector3.back * speedOffset);
        } else if (Enemy != null && (Enemy.transform.position.x >= EnemySceneMarkerOut.transform.position.x)) {
            Destroy(Enemy);
            PlayerLost = false;
            
            Light[] flareLights = Flare.GetComponentsInChildren<Light>();
            ParticleSystem[] flareParticles = Flare.GetComponentsInChildren<ParticleSystem>();
            
            foreach (ParticleSystem flareParticle in flareParticles)
            {
                flareParticle.Play();
            }

            foreach (Light flareLight in flareLights)
            {
                flareLight.enabled = true;
            }

            Invoke("EndEvade", 1f);
        }
    }

    private void EndEvade () {
        GameManager.instance.combatManager.EndSelect();
    }

    public void PlayerEvade() {
        Light[] flareLights = Flare.GetComponentsInChildren<Light>();
        ParticleSystem[] flareParticles = Flare.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem flareParticle in flareParticles)
        {
            flareParticle.Stop();
        }
        foreach (Light flareLight in flareLights)
        {
            flareLight.enabled = false;
        }
        Invoke("PlayerLose", 1.5f);
    }

    public void PlayerLose() {
        Debug.Log("PlayerLose");
        PlayerLost = true;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(PlayerLost);
        if (PlayerLost){
            // EnemyRotateAway();
            EnemySlideOutView();
        } else {
            EnemySlideIntoView();
        }
    }
}
