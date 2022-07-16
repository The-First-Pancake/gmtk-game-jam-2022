using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{
    public FaceType faceType = FaceType.pip;
    public Dice dice; //TODO make this plug in on construct
    [SerializeField] private List<Pip> pips;
    public Transform socket;

    [Header("Child Objects")]
    public GameObject pipLayout;
    public GameObject plate;
    public GameObject isopod;
    
    public void configureFace(FaceType faceType)
    {
        this.faceType = faceType;
        if (faceType == FaceType.pip)
        {
            pipLayout.SetActive(true);
            plate.SetActive(true);
            isopod.SetActive(false);
            generatePips();
        }
        if (faceType == FaceType.isopod)
        {
            pipLayout.SetActive(false);
            plate.SetActive(false);
            isopod.SetActive(true);
            pips.Clear();
        }
    }

    public void attatchFace(Dice dice, Transform socket)
    {
        this.socket = socket;
        this.dice = dice;

        transform.parent = socket;
        transform.position = socket.position;
        transform.rotation = socket.rotation;
    }
    public void generatePips()
    {
        if (faceType != FaceType.pip) { Debug.LogWarning("tried to generate pips on face that isn't a pip face"); return; }
        for (int i = 0; i < pipLayout.transform.childCount; i++) {DestroyImmediate(pipLayout.transform.GetChild(0).gameObject); } //Anakin in the jedi temple (kill all the children)
        foreach (Pip pip in pips)
        {
            UnityEngine.UI.Image newPip = Instantiate(GameManager.instance.pipPrefab, pipLayout.transform).GetComponent<UnityEngine.UI.Image>();
            newPip.sprite = pip.sprite;
            newPip.color = pip.color;
        }
    }
    public void addPip(PipType type)
    {
        //TODO check iff too many pips
        if(faceType != FaceType.pip) { Debug.LogWarning("tried to add pip to face that isn't a pip face"); return; }
        pips.Add(GameManager.instance.getPipOfType(type));
        
    }
}
public enum FaceType
{
    pip,
    isopod,
}
