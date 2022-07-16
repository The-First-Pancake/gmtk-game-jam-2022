using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultsDisplay : MonoBehaviour
{
    public List<TextMeshProUGUI> childTexts;
    public GameObject heartPrefab;
    public GameObject healtbar;
    private List<GameObject> hearts = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < GameManager.instance.player.health; i++) {
            GameObject newheart = Instantiate(heartPrefab, healtbar.transform);
            hearts.Add(newheart);
        }
    }

    // Update is called once per frame
    void Update()
    {
        PoolResults results = GameManager.instance.player.dicePool.GetPoolResults();
        for (int i = 0; i < results.pips.Length; i++) {
            childTexts[i].text = results.pips[i].ToString();
        }
        if (hearts.Count > GameManager.instance.player.health) {
            Destroy(hearts[0]);
            hearts.RemoveAt(0);
        }
    }
}
