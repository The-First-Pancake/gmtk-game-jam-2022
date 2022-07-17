using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TMP_Text textmeshPro = gameObject.GetComponent<TMP_Text>();
        textmeshPro.characterSpacing += 0.01f;
    }
}
