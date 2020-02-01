using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentMotor : RobotComponent
{
    [Header("Motor")]
    public HingeJoint motorJoint;
    public float power = 100.0f;

    public void FixedUpdate()
    {
        if (motorJoint == null) return;

        float targetVelocity = power * input;
        var motor = motorJoint.motor;
        motor.targetVelocity = targetVelocity;

        motorJoint.motor = motor;
    }
}
