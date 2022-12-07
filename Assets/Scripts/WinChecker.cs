using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinChecker : MonoBehaviour
{
    public GameObject[] winScreens;
    

    void Start()
    {
        winScreens[KillFloor.playerGanhou-1].SetActive(true);
    }


}
