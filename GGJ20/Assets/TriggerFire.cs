using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFire : MonoBehaviour
{
    public bool isFireActive = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFireActive == false) return;


    }

    private void OnTriggerEnter(Collider other)
    {
        //GameManager.Instance.InstantiateDecal(prefabDecal, collisionEvents[0].intersection, -collisionEvents[0].normal, other.GetComponent<Rigidbody>());
        //if (other.GetComponent<Cake>() == null) return;
        //other.GetComponent<Cake>().EnableClandles();
    }
}
