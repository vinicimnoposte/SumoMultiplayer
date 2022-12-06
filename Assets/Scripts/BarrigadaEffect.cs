using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrigadaEffect : MonoBehaviour
{

    private void OnTriggerEnter(Collider col)
    {
        
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerMovAdv mov;
            mov = col.gameObject.GetComponent<PlayerMovAdv>();
            if (mov != null)
            {
                Vector3 direction = (col.transform.position - transform.position).normalized;
                mov.Knockback(direction);
            }
        }
    }
}
