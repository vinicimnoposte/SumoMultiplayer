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
            foreach (GameObject player in players)
            {
                player.gameObject.transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y, spawn.transform.position.z);
            }
        }      
    }
}
