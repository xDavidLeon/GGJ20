using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dangerous : MonoBehaviour
{
    public float timeLastHit = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Time.time - timeLastHit < 0.5f) return;
        if (collision.transform.CompareTag("Player"))
        {
            timeLastHit = Time.time;
            ContactPoint contact = collision.GetContact(0);
            GameManager.Instance.InstantiateDecalBlood(contact.point, -contact.normal, collision.rigidbody);
        }
    }
}
