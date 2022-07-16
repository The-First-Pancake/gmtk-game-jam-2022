using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    public Camera camera;
    public float followSpeed;
    public float floatToHeight;
    public float shakeFactor;

    private Rigidbody rb;
    private bool pickedUp = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckPickup()) {
            FollowMouse();
        }
    }

    bool CheckPickup() {
        if (pickedUp) {
            if (Input.GetMouseButtonDown(1)) {
                ThrowDice();
                pickedUp = false;
            }
        } else {
            if (Input.GetMouseButtonDown(0)) {
                Ray ray = camera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -camera.transform.position.y));
                RaycastHit[] raycastHits = Physics.RaycastAll(ray, 100f);
                foreach (RaycastHit raycastHit in raycastHits) {
                    if (raycastHit.transform == transform) {
                        pickedUp = true;
                    }
                }
             }
        }
        return pickedUp;
    }

    void FollowMouse()
    {
        Vector3 adjustedMouse = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.transform.position.y));
        adjustedMouse.y = floatToHeight;
        Vector3 diff = adjustedMouse - transform.position;

        if (diff.magnitude > 1.5) {
            diff = followSpeed*diff;
            rb.velocity = diff;
        }
        rb.AddForceAtPosition(Shake(), Shake());
    }

    Vector3 Shake()
    {
        // Shake is more severe with bottom down. Nose dive is faster, but more stable
        Vector3 randomdir = UnityEngine.Random.insideUnitSphere;
        return randomdir * shakeFactor;
    }

    void ThrowDice()
    {

    }
}
