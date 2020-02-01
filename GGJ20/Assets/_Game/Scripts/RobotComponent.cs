using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotComponent : MonoBehaviour
{
    [Header("Robot Component")]
    [Range(-1.0f, 1.0f)]
    public float input = 0.0f;

    public Rigidbody chassis;

    public virtual void Reset()
    {

    }
}
