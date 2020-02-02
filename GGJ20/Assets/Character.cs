using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Rigidbody rbHead;
    private Transform pointOfInterest;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rbHead)
        {
            if (pointOfInterest)
                rbHead.transform.LookAt(pointOfInterest);
        }
    }

    public void SetPointOfInterest(Transform g)
    {
        pointOfInterest = g;
    }
}
