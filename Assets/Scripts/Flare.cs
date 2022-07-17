using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flare : MonoBehaviour
{

    public Light flareRed;
    public Light flareWhite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int intensity = Random.Range(75, 150);
        flareRed.intensity = intensity;
        flareWhite.intensity = intensity;
    }
}
