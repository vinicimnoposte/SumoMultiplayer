using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsChooseSkin : MonoBehaviour
{
    public GameObject player;
    public void EscolheAzul()
    {
        player.GetComponent<ChooseSkin>().ChooseAzul();
    }

    public void EscolheVermelho()
    {
        player.GetComponent<ChooseSkin>().ChooseVermelho();
    }

    public void EscolheRaijin()
    {
        player.GetComponent<ChooseSkin>().ChooseRaijin();
    }

    public void EscolheFujin()
    {
        player.GetComponent<ChooseSkin>().ChooseFujin();
    }
}

