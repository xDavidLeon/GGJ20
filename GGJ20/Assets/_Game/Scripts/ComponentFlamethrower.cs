using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentFlamethrower : RobotComponent
{
    public ParticleSystem ps;
    public BoxCollider triggerFire;

    private void Awake()
    {
    }

    void Update()
    {
        var e = ps.emission;
        if (input > 0.0f)
        {
            e.enabled = true;
            triggerFire.enabled = true;
        }
        else
        {
            e.enabled = false;
            triggerFire.enabled = false;
        }
    }
}
