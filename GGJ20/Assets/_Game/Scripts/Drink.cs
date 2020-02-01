using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink : MonoBehaviour
{
    public ParticleSystem particlesLiquid;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Dot(transform.up, Vector3.up) < 0)
        {
            particlesLiquid.Play();
        }
        else particlesLiquid.Stop();
    }
}
