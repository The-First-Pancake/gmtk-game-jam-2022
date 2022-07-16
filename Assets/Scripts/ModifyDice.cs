using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyDice : MonoBehaviour
{
    public Transform diceSelectionAreaAnchor;

    float diceSpacing = 2.5f;
    float dicerotateSpeed = 60;
    public DicePoolManager dicePoolManager;


    void Start()
    {
        StartCoroutine(SelectDiceSocket());
    }

    public IEnumerator SelectDiceSocket(System.Action<Transform> selectedSocket = null)
    {
        yield return null; //wait a frame in case this is happening right away (TODO remove this)

        selectedSocket = null;

        //freeze dice
        foreach (Dice dice in dicePoolManager.diceInPool)
        {
            dice.diceBody.GetComponent<Rigidbody>().isKinematic = true;
            dice.transform.rotation = Quaternion.Euler(45, 0, 45);
        }

        //move all dice into display
        int diceCount = dicePoolManager.diceInPool.Count;
        float span = (diceCount - 1) * diceSpacing;
        float startX = (-span / 2);
        for (int i = 0; i < diceCount; i++)
        {
            float newX = startX + diceSpacing * i;
            dicePoolManager.diceInPool[i].transform.position = diceSelectionAreaAnchor.position + new Vector3(newX, 0, 0);
        }



        while(selectedSocket == null)
        {
            foreach (Dice dice in dicePoolManager.diceInPool)
            {
                dice.transform.Rotate(new Vector3(1, 0, 1) * dicerotateSpeed * Time.deltaTime);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = GameManager.instance.camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] raycastHits = Physics.RaycastAll(ray, 100f);
                foreach (RaycastHit raycastHit in raycastHits)
                {
                    Dice hitDie = dicePoolManager.diceInPool.Find(x => raycastHit.transform == x.diceBody.transform);
                    if (hitDie)
                    {
                        Transform hitsocket = hitDie.sockets.Find(x => raycastHit.transform == x);
                        selectedSocket.Invoke(hitsocket);
                        break;
                    }
                }
            }
            yield return null;
        }
    }

    

}
