using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circuit : MonoBehaviour
{
    public List<ControlInput> controlInputs;
    public List<RobotComponent> robotComponents;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < controlInputs.Count; i++)
        {
            ControlInput ci = controlInputs[i];
            if (ci == null) continue;

            if (robotComponents.Count < i) continue;
            RobotComponent c = robotComponents[i];

            if (c == null) continue;

            c.input = ci.input;
        }

    }

    public void SetComponentInput(int id, float x)
    {
        robotComponents[id].input = x;

    }
}
