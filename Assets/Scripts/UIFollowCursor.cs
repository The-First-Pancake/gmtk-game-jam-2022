using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFollowCursor : MonoBehaviour
{

    public RectTransform MovingObject;
    public Vector3 offset;
    public RectTransform BasisObject;

    // Update is called once per frame
    void Update()
    {
        MoveObject();
    }

    public void MoveObject() {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 screenOrigin = new Vector3(Screen.width/2, Screen.height/2, 0);
        Vector3 rotationVector = (screenOrigin - mousePosition) * 0.01f;
        MovingObject.rotation = Quaternion.Euler(new Vector3(rotationVector.y * -1, rotationVector.x, 0));
    }

}
