using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkLiquid : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    public GameObject prefabDecalMilk;

    // Start is called before the first frame update
    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        if (numCollisionEvents == 0) return;

        GameManager.Instance.InstantiateDecal(prefabDecalMilk, collisionEvents[0].intersection, -collisionEvents[0].normal, other.GetComponent<Rigidbody>());

        if (other.GetComponent<Bowl>() == null) return;
        other.GetComponent<Bowl>().AddLiquid();



        //Rigidbody rb = other.GetComponent<Rigidbody>();
        //int i = 0;

        //while (i < numCollisionEvents)
        //{
        //    if (rb)
        //    {
        //        Vector3 pos = collisionEvents[i].intersection;
        //        Vector3 force = collisionEvents[i].velocity * 10;
        //        rb.AddForce(force);
        //    }
        //    i++;
        //}
    }
}
