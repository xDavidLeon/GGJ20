using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragileProp : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        float m = collision.relativeVelocity.magnitude;
        if (m > 5.0f)
        {
            Drop();
            //Debug.Log(m);
        }
    }

    public void Drop()
    {
        transform.SetParent(null);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Collider>().isTrigger = false;
    }
}
