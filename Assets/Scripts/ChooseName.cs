using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChooseName : MonoBehaviour
{
    public GameObject player;
    public TMP_Text chooseNameText;

    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<PlayerMovAdv>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NameConfirm()
    {
        PlayerMovAdv varPlayerMovement = player.GetComponent<PlayerMovAdv>();
        //CMD_NameConfirm(varPlayerMovement);
        varPlayerMovement.CMD_NameConfirm(varPlayerMovement, chooseNameText.text);
        Debug.Log("ACFS");
        player.GetComponent<PlayerMovAdv>().enabled = true;
        gameObject.SetActive(false);
    }
}
