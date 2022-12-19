using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] rigidBodies;

    // Start is called before the first frame update
    void Start()
    {
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        DeactivateRagdoll();
    }

    public void DeactivateRagdoll() { 
        foreach(Rigidbody rigidBody in rigidBodies)
        {
            rigidBody.detectCollisions = false;
            rigidBody.isKinematic = true;
        }
    }

    public void ActivateRagdoll()
    {
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.detectCollisions = true;
            rigidBody.isKinematic = false;
        }
    }
}
