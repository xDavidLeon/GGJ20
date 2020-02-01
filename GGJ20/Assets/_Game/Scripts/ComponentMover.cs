using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentMover : RobotComponent
{
    [Header("Motor")]
    public Rigidbody rb;
    public float power = 100.0f;

    public Vector3 anchorMin;
    public Vector3 anchorMax;

    public Vector3 moveDirection = Vector3.up;

    private Vector3 startConnectedAnchor;

    private void Start()
    {
        //startConnectedAnchor = motorJoint.connectedAnchor;
    }

    public override void Reset()
    {
        base.Reset();
        //motorJoint.connectedAnchor = startConnectedAnchor;
    }

    public void FixedUpdate()
    {
        if (rb == null) return;

        Vector3 targetPosition = rb.transform.position + moveDirection * Time.deltaTime * input * power;
        rb.MovePosition(targetPosition);

        //Vector3 targetAnchor = input < 0 ? anchorMin : anchorMax;
        //motorJoint.connectedAnchor = Vector3.MoveTowards(motorJoint.connectedAnchor, targetAnchor, Time.deltaTime * Mathf.Abs(input) * power);
    }
}
