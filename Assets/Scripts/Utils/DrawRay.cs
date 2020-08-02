using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRay : MonoBehaviour
{

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, transform.forward * 3, Color.red);
    }
}
