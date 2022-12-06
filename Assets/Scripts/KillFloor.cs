using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFloor : NetworkBehaviour
{
    
    public Transform spawn;

    public GameObject[] players;



    public void Awake()
    {
        spawn = GameObject.FindGameObjectWithTag("Respawn").transform;
    }
    
    public void OnTriggerEnter(Collider other)
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.position = spawn.transform.position;
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
            }
        }      
    }
}
