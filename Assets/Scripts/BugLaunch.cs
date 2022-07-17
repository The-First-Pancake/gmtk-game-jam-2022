using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugLaunch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(-1000, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
