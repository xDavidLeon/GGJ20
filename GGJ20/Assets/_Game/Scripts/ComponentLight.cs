using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentLight : RobotComponent
{
    public Light light;

    public float maxIntensity = 0.2f;

    private void Update()
    {
        light.intensity = Mathf.Lerp(0.0f, maxIntensity, Mathf.Clamp01(input));
    }
}
