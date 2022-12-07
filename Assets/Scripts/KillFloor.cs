using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFloor : NetworkBehaviour
{
    
    public Transform spawn;

    public GameObject[] players;

    public static int playerNumber;

    public static int playerGanhou;

    public void Awake()
    {
        spawn = GameObject.FindGameObjectWithTag("Respawn").transform;
        playerNumber = 0;
        playerGanhou = 0;
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (isServer == false)
            return;
        players = GameObject.FindGameObjectsWithTag("Player");
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.position = spawn.transform.position;
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
            }

            PlayerMovAdv mov = other.gameObject.GetComponent<PlayerMovAdv>();
            if (mov != null)
            {
                mov.contagemQueda++;
            }
        }      
    }

    public void Winner()
    {
        int amigao = -1;
        
        foreach (var player in players)
        {
            PlayerMovAdv mov = player.GetComponent<PlayerMovAdv>();
            if (amigao == -1)
            {

                amigao = mov.contagemQueda;
                playerGanhou = mov.numero;
            }
            else
            {
                if (amigao < mov.contagemQueda)
                {
                    amigao = mov.contagemQueda;
                    playerGanhou = mov.numero;
                }
            }    
        }
        
    }
}
