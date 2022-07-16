using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    
    public int dieSize = 6;
    public List<diceSizePrefabs> dicePrefabsBySize = new List<diceSizePrefabs>();
    private List<Transform> sockets = new List<Transform>();
    public List<Face> faces;
    public GameObject diceBody;
    public void Start()
    {
        generateDie();
    }
    public void Update()
    {
        Debug.Log(getUpFace());
    }
    [ContextMenu("Generate Model")]
    public void generateDie()
    {
        if (diceBody) { DestroyImmediate(diceBody); } //if already exists, destro the old one


        diceSizePrefabs dicePrefabs = dicePrefabsBySize.Find(x => x.size == this.dieSize);
        if(dicePrefabs == null) { Debug.LogWarning("Tried to generate die of size " + this.dieSize + ". Could not find prefabs of that size"); return;}

        diceBody = Instantiate(dicePrefabs.shapePrefab, transform);
        sockets.Clear();
        foreach(Transform child in diceBody.transform)
        {
            if (child.name.ToLower().Contains("socket"))
            {
                sockets.Add(child);
            }
        }
        
        if(faces.Count >= sockets.Count){ Debug.LogWarning("Too many faces on die " + this.name); return; }

        int i = 0;
        foreach(Face face in faces)
        {
            face.generateFace(sockets[i], dicePrefabs.facePrefab);
            i++;
        }
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
        Debug.Log(bestSocket);
        return faces.Find(x => x.socket == bestSocket);
    }
}

[Serializable]
public class Face
{ 
    public Dice dice; //TODO make this plug in on construct
    public List<Pip> pips;
    public GameObject gameObject;
    public Transform socket;


    public void generateFace(Transform socket, GameObject prefab)
    {
        this.socket = socket;
        gameObject = MonoBehaviour.Instantiate(prefab, socket.position, socket.rotation, socket);
        addPips();

    }
    public void addPips()
    {
        Debug.Log(Resources.Load("Pips/PipPrefab"));
        GameObject pipPrefab = (GameObject)Resources.Load("Pips/PipPrefab");
        Transform pipLayoutGroup = gameObject.transform.Find("Canvas").Find("PipLayout");
        foreach (Transform child in pipLayoutGroup)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Pip pip in pips)
        {
            UnityEngine.UI.Image newPip = MonoBehaviour.Instantiate(pipPrefab, pipLayoutGroup).GetComponent<UnityEngine.UI.Image>();
            newPip.sprite = pip.sprite;
            newPip.color = pip.color;
        }
    }
}

[Serializable]
public class diceSizePrefabs
{
    public int size;
    public GameObject shapePrefab;
    public GameObject facePrefab;
}

