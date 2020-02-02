using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentFlamethrower : RobotComponent
{
    public ParticleSystem ps;

    private void Awake()
    {
    }

    void Update()
    {
        if (input > 0.0f)
        {
            ps.Play();
        }
        else ps.Stop();
    }
}
