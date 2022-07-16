using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultsDisplay : MonoBehaviour
{
    public List<TextMeshProUGUI> childTexts;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PoolResults results = GameManager.instance.player.dicePool.GetPoolResults();
        for (int i = 0; i < results.pips.Length; i++) {
            childTexts[i].text = results.pips[i].ToString();
        }
    }
}
