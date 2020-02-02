using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFire : MonoBehaviour
{
    public bool isFireActive = false;
    public LayerMask hitLayerMask;
    RaycastHit hit;
    public float radius = 1.0f;
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFireActive == false) return;

        Collider[] cs = Physics.OverlapSphere(transform.position, radius, hitLayerMask);
        if (cs.Length > 0)
        {
            Debug.DrawRay(transform.position, transform.forward, Color.green, 1.0f);

            foreach (Collider c in cs)
            {
                if (c == null) continue;
                if (c.GetComponent<Cake>() != null)
                c.GetComponent<Cake>().EnableClandles();
            }
        }

        if (Physics.SphereCast(transform.position, radius, transform.forward, out hit, 1.0f, hitLayerMask))
        {
            Debug.DrawRay(transform.position, transform.forward, Color.green, 1.0f);
            //GameManager.Instance.InstantiateDecal(prefabDecal, collisionEvents[0].intersection, -collisionEvents[0].normal, other.GetComponent<Rigidbody>());
            if (hit.collider.GetComponent<Cake>() != null) 
            hit.collider.GetComponent<Cake>().EnableClandles();
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward, Color.red, 1.0f);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //GameManager.Instance.InstantiateDecal(prefabDecal, collisionEvents[0].intersection, -collisionEvents[0].normal, other.GetComponent<Rigidbody>());
        //if (other.GetComponent<Cake>() == null) return;
        //other.GetComponent<Cake>().EnableClandles();
    }
}
