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
        if (Time.time - timeLastHit < 0.25f) return;
        ContactPoint contact = collision.GetContact(0);
        if (collision.transform.CompareTag("Player"))
        {
            timeLastHit = Time.time;
            GameManager.Instance.InstantiateDecal(GameManager.Instance.decalBloodPrefab, contact.point, -contact.normal, collision.rigidbody);
        }

        Collider[] cs = Physics.OverlapSphere(contact.point, 0.5f, ~0, QueryTriggerInteraction.Collide);
        foreach (Collider c in cs)
            if (c.GetComponent<FragileProp>() != null) c.GetComponent<FragileProp>().Drop();
    }
}
