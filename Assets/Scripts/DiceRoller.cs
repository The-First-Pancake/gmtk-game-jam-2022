using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    public enum DiceState {
        SETTLED,
        PICKED_UP,
        ROLLING,
    }

    public float followSpeed;
    public float floatToHeight;
    public float shakeFactor;

    private Rigidbody rb;
    private Camera camera;
    private DiceState diceState = DiceState.ROLLING;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        camera = GameManager.instance.camera;
    }

    // Update is called once per frame
    void Update()
    {
        switch (diceState)
        {
            case DiceState.PICKED_UP:
                FollowMouse();
                if (CheckDrop()) {
                    ThrowDice();
                    diceState = DiceState.ROLLING;
                }
                break;
            case DiceState.ROLLING:
                if (CheckSettled()) {
                    rb.isKinematic = true;
                    diceState = DiceState.SETTLED;
                }
                break;
            case DiceState.SETTLED:
                if (CheckPickup()) {
                    rb.isKinematic = false;
                    diceState = DiceState.PICKED_UP;
                }
                break;
        }
        if (CheckPickup()) {
            FollowMouse();
        }
    }

    bool CheckDrop() {
        return Input.GetMouseButtonDown(1);
    }

    bool CheckPickup() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = camera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -camera.transform.position.y));
            RaycastHit[] raycastHits = Physics.RaycastAll(ray, 100f);
            foreach (RaycastHit raycastHit in raycastHits) {
                if (raycastHit.transform == transform) {
                    return true;
                }
            }
        }
        return false;
    }

    bool CheckSettled() {
        return (rb.velocity.magnitude < 0.001);
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
        rb.AddForceAtPosition(Shake(), diff.magnitude*Shake());
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
