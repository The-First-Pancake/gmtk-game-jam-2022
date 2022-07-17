using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyDice : MonoBehaviour
{
    private Transform zoomPoint;

    float objectSpacing = 2.5f;
    float dicerotateSpeed = 60;
    public DicePoolManager dicePoolManager;
    Transform clickedSocket = null;

    
    void Start()
    {
        zoomPoint = Camera.main.transform.Find("DiceZoomPoint");
        StartCoroutine(SelectDiceSocketFromPool());
    }

    public void ChooseAFace()
    {

        int choices = 3;
        List<Face> faceOptions = new List<Face>();
        for (int i = 0; i < choices; i++)
        {
            Face face1 = Instantiate(GameManager.instance.getFacePrefab_OfSize(6)).GetComponent<Face>();
            face1.configureFace(FaceType.pip);
            faceOptions.Add(face1);


        }

    }


    public void moveTransformsTo_ZoomPoint(List<Transform> objects)
    {
        int count = objects.Count;
        float span = (count - 1) * objectSpacing;
        float startX = (-span / 2);
        for (int i = 0; i < count; i++)
        {
            float newX = startX + objectSpacing * i;
            objects[i].position = zoomPoint.position + new Vector3(newX, 0, 0);
        }
    }

    public IEnumerator SelectDiceSocketFromPool(System.Action<Transform> selectedSocket = null)
    {
        yield return null; //wait a frame in case this is happening right away (TODO remove this)
        clickedSocket = null;

        
        //freeze dice
        foreach (Dice dice in dicePoolManager.diceInPool)
        {
            
            dice.diceBody.GetComponent<Rigidbody>().isKinematic = true;
            dice.transform.rotation = Quaternion.Euler(45, 0, 45);
        }

        //make list of transforms so we can align them on the zoom point
        List<Transform> diceTransformList = new List<Transform>();
        foreach (Dice dice in dicePoolManager.diceInPool) { diceTransformList.Add(dice.transform); }

        //Move all thee transforms where they need to go
        moveTransformsTo_ZoomPoint(diceTransformList);



        while (clickedSocket == null)
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

        foreach (Dice dice in dicePoolManager.diceInPool)
        {
            dice.diceBody.GetComponent<Rigidbody>().isKinematic = false;
            dice.transform.position = GameManager.instance.diceHomePoint.position;
            yield return new WaitForSeconds(.1f);
        }

        Debug.Log(clickedSocket);
    }

    

}
