using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceSearchManager : MonoBehaviour
{
    public Transform FaceRewardDisplayPt;
    public ModifyDice modifyDice;

    public void Start()
    {
        modifyDice = GameManager.instance.player.gameObject.GetComponentInChildren<ModifyDice>();
    }
}
