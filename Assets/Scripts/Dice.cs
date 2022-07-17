using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    
    public int dieSize = 6;
    public List<Transform> sockets = new List<Transform>();
    public List<Face> faces;
    public GameObject diceBody;
    public void Start()
    {
        generateTestDie();
    }

    [ContextMenu("Generate Model")]
    public void generateDieBody()
    {
        if (diceBody) { DestroyImmediate(diceBody); } //if already exists, destroy the old one
        faces.Clear();

        GameObject diceBodyPrefab = GameManager.instance.getDiceBodyPrefab_OfSize(dieSize);
        if(diceBodyPrefab == null) { return;}

        diceBody = Instantiate(diceBodyPrefab, transform);
        sockets.Clear();
        foreach(Transform child in diceBody.transform)
        {
            if (child.name.ToLower().Contains("socket"))
            {
                sockets.Add(child);
            }
        }
    }
    [ContextMenu("Generate Test Die")]
    public void generateTestDie()
    {
        generateDieBody();

        Face face1 = Instantiate(GameManager.instance.getFacePrefab_OfSize(dieSize)).GetComponent<Face>();
        face1.addPip(PipType.Bullet);
        face1.addPip(PipType.Bullet);
        face1.addPip(PipType.Bullet);
        face1.configureFace(FaceType.pip);
        attatchFace(face1, firstOpenSocket());

        Face face2 = Instantiate(GameManager.instance.getFacePrefab_OfSize(dieSize)).GetComponent<Face>();
        face2.addPip(PipType.Energy);
        face2.addPip(PipType.Energy);
        face2.configureFace(FaceType.pip);
        attatchFace(face2, firstOpenSocket());

        Face face3 = Instantiate(GameManager.instance.getFacePrefab_OfSize(dieSize)).GetComponent<Face>();
        attatchFace(face3, firstOpenSocket());
        face3.configureFace(FaceType.isopod);
    }

    public Face? getUpFace()
    {
        float bestDot = 0;
        Transform bestSocket = null;
        foreach (Transform socket in sockets)
        {
            float dot = Vector3.Dot(-socket.forward, Vector3.up); //close to 1 if parallel, clost to -1 if antiparallel
            if (dot > bestDot) { bestSocket = socket ; bestDot = dot; }
        }
        return faces.Find(x => x.socket == bestSocket);
    }
    public void ChangeSize(bool increase)
    {
        //TODO. Do this
    }
    public Transform firstOpenSocket()
    {
        foreach (Transform socket in sockets)
        {
            if (socket.childCount == 0)
            {
                return socket;
            }
        }
        return null;
    }
    public void attatchFace(Face face, Transform socket)
    {
        //TODO handle if socket is already filled
        face.transform.parent = socket;
        face.transform.position = socket.position;
        face.transform.rotation = socket.rotation;

        face.dice = this;
        face.socket = socket;
        if (!faces.Contains(face)) { faces.Add(face); }
    }

}

[Serializable]
public class diceSizePrefabs
{
    public int size;
    public GameObject bodyPrefab;
    public GameObject facePrefab;
}

