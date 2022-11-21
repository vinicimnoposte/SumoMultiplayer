using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    
    public GameObject spawn;

    public void Awake()
    {
        if(spawn == null)
            spawn = GameObject.FindGameObjectWithTag("Respawn");
    }
    public void Spawn()
    {
        gameObject.transform.position = spawn.transform.position;
        print("spawnei");
    }
}
