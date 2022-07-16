using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBackgroundDice : MonoBehaviour
{
    public GameObject D6;
    public GameObject D8;
    public GameObject D12;
    public int zOffset;

    public Camera cam;

    Vector3 GetInitialPosition() {
        float isLeft = Mathf.Round(Random.value);
        float isTop = Mathf.Round(Random.value);
        
        int randomX = isLeft == 0f ? Random.Range(-10, 0) : Random.Range(Screen.width, Screen.width + 10);
        int randomY = isTop == 0f ? Random.Range(-10, 0) : Random.Range(Screen.height, Screen.height + 10);

        return new Vector3(randomX, randomY, zOffset);
    }

    GameObject GetRandomDie() {
        int random = Random.Range(0,2);

        switch (random) {
            case 0:
                return D6;
            case 1:
                return D8;
            case 2:
                return D12;
        }
        return D6;

    }

    GameObject InstantiateDie() {
        return Instantiate(GetRandomDie(), GetInitialPosition(), Random.rotation);
    }

    void Start() {
        InvokeRepeating("InstantiateDie", 5f, 5f);
    }
}
