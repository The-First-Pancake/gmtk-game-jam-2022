using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmoRandomRotation : MonoBehaviour
{
    private Rigidbody rb;
     void Awake(){
          rb = gameObject.GetComponent<Rigidbody>();
     }
  
     private void Start() {
         rb.AddTorque(new Vector3(Random.Range(0,10), Random.Range(0,10), Random.Range(0,10)));
         rb.AddForce(new Vector3(0, 0, 100));
     }
}
