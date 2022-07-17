using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyDisplay : MonoBehaviour
{
    public GameObject canvasObject;
    public TextMeshProUGUI combatText;
    public TextMeshProUGUI evasionText;
    public TextMeshProUGUI searchText;
    public Transform searchPlatform;
    // Start is called before the first frame update
    void Start()
    {
        canvasObject.GetComponent<Canvas>().worldCamera = GameManager.instance.camera;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.combatManager.state != CombatState.NOT_IN_COMBAT) {
            canvasObject.SetActive(true);
            combatText.text = GameManager.instance.combatManager.combatThresholds.combat.ToString();
            evasionText.text = GameManager.instance.combatManager.combatThresholds.evade.ToString();
            searchText.text = GameManager.instance.combatManager.combatThresholds.search.ToString();
        } else {
            canvasObject.SetActive(false);   
        }
    }
}
