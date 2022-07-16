using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketAligner : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
        float crosshairSize = .35f;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - transform.forward);
        Gizmos.DrawLine(transform.position - transform.right * crosshairSize, transform.position + transform.right * crosshairSize);
        Gizmos.DrawLine(transform.position - transform.right * crosshairSize - transform.up * crosshairSize, transform.position + transform.right * crosshairSize + transform.up * crosshairSize);
        Gizmos.DrawLine(transform.position - transform.up * crosshairSize, transform.position + transform.up* crosshairSize);
        Gizmos.DrawLine(transform.position - transform.up * crosshairSize + transform.right * crosshairSize, transform.position + transform.up * crosshairSize - transform.right * crosshairSize);
    }
}
