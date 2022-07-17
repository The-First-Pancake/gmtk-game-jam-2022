using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flare : MonoBehaviour
{

    public Light flareRed;
    public Light flareWhite;

    public int multiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int intensity = Random.Range(1 * multiplier, 2 * multiplier);
        flareRed.intensity = intensity;
        flareWhite.intensity = intensity;
    }
}
