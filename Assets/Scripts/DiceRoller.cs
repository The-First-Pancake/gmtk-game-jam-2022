using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    public enum DiceState {
        DISABLED,
        SETTLED,
        PICKED_UP,
        ROLLING,
    }

    public float followSpeed;
    public float floatToHeight;
    public float shakeFactor;
    public int averagedOver = 20;

    private Rigidbody rb;
    private Camera camera;
    private DiceState diceState = DiceState.DISABLED;
    private float debouncedVelocity = 0;
    private int debounceCounter = 0;
    private bool emitSettled = false;

    private AudioSource diceSound;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        camera = GameManager.instance.camera;
        diceSound = GetComponent<AudioSource>();
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
                    Debug.Log("Entered Rolling");
                    diceState = DiceState.ROLLING;
                }
                break;
            case DiceState.ROLLING:
                if (CheckSettled()) {
                    emitSettled = true;
                    rb.isKinematic = true;
                    Debug.Log("Entered Settled");
                    diceState = DiceState.SETTLED;
                }
                break;
            case DiceState.SETTLED:
                if (CheckPickup()) {
                    rb.isKinematic = false;
                    Debug.Log("Entered Picked Up");
                    diceState = DiceState.PICKED_UP;
                }
                break;
        }
    }

    void OnCollisionEnter(Collision collision) {
        diceSound.Play();
    }

    bool CheckDrop() {
        return Input.GetMouseButtonDown(1);
    }

    bool CheckPickup() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] raycastHits = Physics.RaycastAll(ray, 100f);
            foreach (RaycastHit raycastHit in raycastHits) {
                if (raycastHit.transform == transform) {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CheckSettled() {
        debounceCounter = (debounceCounter + 1) % averagedOver;
        debouncedVelocity += rb.velocity.magnitude;
        if (debounceCounter == (averagedOver - 1)) {
            float averagedVelocity = debouncedVelocity / averagedOver;
            debouncedVelocity = 0;
            return averagedVelocity <= 0.0001;
        }
        return false;
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
        rb.AddForceAtPosition(Shake(), Shake());
    }

    public void SetEnabled(bool enabled) {
        if (enabled) {
            rb.isKinematic = false;
            diceState = DiceState.PICKED_UP;
        } else {
            diceState = DiceState.DISABLED;
        }
    }

    public bool IsSettled() {
        return diceState == DiceState.SETTLED;
    }

    public bool IsJustSettled() {
        if (emitSettled) {
            emitSettled = false;
            return true;
        }
        return false;
    }
}
