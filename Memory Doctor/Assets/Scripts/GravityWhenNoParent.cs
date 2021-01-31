using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWhenNoParent : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    void Update()
    {
        rb.isKinematic = transform.parent == null;
        rb.useGravity = transform.parent == null;
        if (transform.parent != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
