using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultsDisplay : MonoBehaviour
{
    public List<TextMeshProUGUI> childTexts;
    public TextMeshProUGUI gunCombat;
    public TextMeshProUGUI lightningCombat;
    public TextMeshProUGUI fireCombat;
    public GameObject heartPrefab;
    public GameObject healtbar;
    private List<GameObject> hearts = new List<GameObject>();
    public GameObject rerollPrefab;
    public GameObject rerollBar;
    private List<GameObject> rerolls = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < GameManager.instance.player.health; i++) {
            GameObject newheart = Instantiate(heartPrefab, healtbar.transform);
            hearts.Add(newheart);
        }

        for (int i = 0; i < GameManager.instance.combatManager.numRollsLeft; i++) {
            GameObject newReroll = Instantiate(rerollPrefab, rerollBar.transform);
            rerolls.Add(newReroll);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        PoolResults results = GameManager.instance.player.dicePool.GetPoolResults();
        gunCombat.text = results.gun_combat.ToString();
        lightningCombat.text = results.lightning_combat.ToString();
        fireCombat.text = results.fire_combat.ToString();

        for (int i = 0; i < results.pips.Length; i++) {
            childTexts[i].text = results.pips[i].ToString();
        }


        if (hearts.Count > GameManager.instance.player.health) {
            Destroy(hearts[0]);
            hearts.RemoveAt(0);
        }

        if (rerolls.Count > GameManager.instance.combatManager.numRollsLeft) {
            Destroy(rerolls[0]);
            rerolls.RemoveAt(0);
        }
        if (rerolls.Count < GameManager.instance.combatManager.numRollsLeft) {
            GameObject newReroll = Instantiate(rerollPrefab, rerollBar.transform);
            rerolls.Add(newReroll);
        }
    }
}
