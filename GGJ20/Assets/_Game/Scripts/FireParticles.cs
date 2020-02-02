using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireParticles : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    public GameObject prefabDecal;

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

        GameManager.Instance.InstantiateDecal(prefabDecal, collisionEvents[0].intersection, -collisionEvents[0].normal, other.GetComponent<Rigidbody>());
    }
}
