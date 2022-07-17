using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyDice : MonoBehaviour
{
    private Transform diceZoomPoint;

    float diceSpacing = 2.5f;
    float dicerotateSpeed = 60;
    public DicePoolManager dicePoolManager;
    Transform clickedSocket = null;

    public
    void Start()
    {
        diceZoomPoint = Camera.main.transform.Find("DiceZoomPoint");
        StartCoroutine(SelectDiceSocket());
    }

    public IEnumerator SelectDiceSocket(System.Action<Transform> selectedSocket = null)
    {
        yield return null; //wait a frame in case this is happening right away (TODO remove this)

        clickedSocket = null;
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
            dicePoolManager.diceInPool[i].transform.position = diceZoomPoint.position + new Vector3(newX, 0, 0);
        }



        while(clickedSocket == null)
        {
            foreach (Dice dice in dicePoolManager.diceInPool)
            {
                dice.transform.Rotate(new Vector3(1, 1, Mathf.Sin(Time.time)) * dicerotateSpeed * Time.deltaTime);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = GameManager.instance.camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] raycastHits = Physics.RaycastAll(ray, 100f);
                foreach (RaycastHit raycastHit in raycastHits)
                {
                    foreach (Dice dice in dicePoolManager.diceInPool)
                    {
                        Transform hitsocket = dice.sockets.Find(x => raycastHit.transform == x);
                        if (hitsocket)
                        {
                            clickedSocket = hitsocket;
                            break;
                        }
                    }
                    if (clickedSocket) { break; }
                }
            }
            yield return null;
        }

        Debug.Log(clickedSocket);
    }

    

}
