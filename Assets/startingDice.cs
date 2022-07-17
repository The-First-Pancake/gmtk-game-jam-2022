using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startingDice : MonoBehaviour
{
    public GameObject dicePrefab;

    [SerializeField]
    public List<_Dice> starting_Dice = new List<_Dice>();


    public void Awake()
    {
        foreach (_Dice dice in starting_Dice)
        {
            Dice newDice = Instantiate(dicePrefab.GetComponent<Dice>(), transform);
            newDice.generateDieBody();
            foreach (_Face face in dice._faces)
            {
                Face newFace = Instantiate(GameManager.instance.getFacePrefab_OfSize(newDice.dieSize)).GetComponent<Face>();
                foreach (PipType pip in face._pips)
                {
                    newFace.addPip(pip);
                }
                newFace.configureFace(face.faceType);
                newDice.AttatchFace(newFace, newDice.firstOpenSocket());
            }
            newDice.sendHome();
        }

    }

    [System.Serializable]
    public class _Dice
    {
        public List<_Face> _faces = new List<_Face>();
    }
    [System.Serializable]
    public class _Face
    {
        public FaceType faceType = FaceType.pip;
        public List<PipType> _pips = new List<PipType>();
    }

}
