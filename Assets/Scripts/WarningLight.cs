using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningLight : MonoBehaviour
{
    public GameObject warningLight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        warningLight.transform.RotateAround(warningLight.transform.position, warningLight.transform.up, 1);
    }
}
