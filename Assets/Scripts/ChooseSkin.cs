using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseSkin : NetworkBehaviour
{
    public GameObject azul, vermelho, raijin, fujin;

    [SyncVar]
    public bool isAzul = false;
    [SyncVar]
    public bool isVermelho = false;
    [SyncVar]
    public bool isRaijin = false;
    [SyncVar]
    public bool isFujin = false;

    private PlayerMovAdv mov;

    // Start is called before the first frame update
    void Start()
    {
        mov = GetComponent<PlayerMovAdv>();

        azul.SetActive(true);
        vermelho.SetActive(false);
        raijin.SetActive(false);
        fujin.SetActive(false);

        if (isAzul == true)
        {
            azul.SetActive(true);
            vermelho.SetActive(false);
            raijin.SetActive(false);
            fujin.SetActive(false);
            //mov.anim=azul.GetComponent<Animator>();
        }
        if (isVermelho == true)
        {
            azul.SetActive(false);
            vermelho.SetActive(true);
            raijin.SetActive(false);
            fujin.SetActive(false);
            //mov.anim = vermelho.GetComponent<Animator>();
        }
        if (isRaijin == true)
        {
            azul.SetActive(false);
            vermelho.SetActive(false);
            raijin.SetActive(true);
            fujin.SetActive(false);
            //mov.anim = raijin.GetComponent<Animator>();
        }
        if (isFujin == true)
        {
            azul.SetActive(false);
            vermelho.SetActive(false);
            raijin.SetActive(false);
            fujin.SetActive(true);
            //mov.anim = fujin.GetComponent<Animator>();
        }
    }
    [Command]
    void CMD_ChooseAzul()
    {
        RPC_ChooseAzul();
    }
    [ClientRpc]
    void RPC_ChooseAzul()
    {
        azul.SetActive(true);
        vermelho.SetActive(false);
        raijin.SetActive(false);
        fujin.SetActive(false);

        isAzul = true;
        isVermelho = false;
        isRaijin = false;
        isFujin = false;
        mov.activeAnimator = azul.GetComponent<Animator>();
    }
    public void ChooseAzul()
    {
        CMD_ChooseAzul();
    }
    [Command]
    void CMD_ChooseVermelho()
    {
        RPC_ChooseVermelho();
    }
    [ClientRpc]
    void RPC_ChooseVermelho()
    {
        azul.SetActive(false);
        vermelho.SetActive(true);
        raijin.SetActive(false);
        fujin.SetActive(false);

        isAzul = false;
        isVermelho = true;
        isRaijin = false;
        isFujin = false;
    }
    public void ChooseVermelho()
    {
        CMD_ChooseVermelho();
    }
    [Command]
    void CMD_ChooseRaijin()
    {
        RPC_ChooseRaijin();
    }
    [ClientRpc]
    void RPC_ChooseRaijin()
    {
        azul.SetActive(false);
        vermelho.SetActive(false);
        raijin.SetActive(true);
        fujin.SetActive(false);

        isAzul = false;
        isVermelho = false;
        isRaijin = true;
        isFujin = false;
    }
    public void ChooseRaijin()
    {
        CMD_ChooseRaijin();
    }
    [Command]
    void CMD_ChooseFujin()
    {
        RPC_ChooseFujin();
    }
    [ClientRpc]
    void RPC_ChooseFujin()
    {
        azul.SetActive(false);
        vermelho.SetActive(false);
        raijin.SetActive(false);
        fujin.SetActive(true);

        isAzul = false;
        isVermelho = false;
        isRaijin = false;
        isFujin = true;
    }
    public void ChooseFujin()
    {
        CMD_ChooseFujin();
    }
}

