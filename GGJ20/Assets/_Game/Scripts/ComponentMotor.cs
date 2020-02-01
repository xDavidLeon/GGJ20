using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentMotor : RobotComponent
{
    [Header("Motor")]
    public HingeJoint motorJoint;

    public float power = 100.0f;
    
    private Rigidbody motorRb;

    private void Awake()
    {
        motorRb = motorJoint.GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        if (motorJoint == null) return;

        var motor = motorJoint.motor;
        float targetVelocity = power * input;

        motor.targetVelocity = targetVelocity;

        motorJoint.motor = motor;
    }
}
