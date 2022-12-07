using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using UnityEngine.SceneManagement;

public class TimerManager : NetworkBehaviour
{
    [SyncVar]
    public float timeLeft;
    public bool timerOn;
    public TMP_Text timerText;

    public Image spriteRenderer;
    public Sprite hushImage;



    // Update is called once per frame
    
    void Update()
    {
        if(timerOn)
        {
            if(timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                
                updateTimer(timeLeft);
                if (timeLeft < 30f)
                {
                    ChangeSprite();
                }
            }
            
            else
            {
                timeLeft = 0;
                timerOn = false;
                ChangeScene();
            }
        }
    }
    [ClientRpc]
    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }


    [ClientRpc]    
    public void ChangeScene()
    {
        SceneManager.LoadScene("MatchEnd", LoadSceneMode.Single);
    }
    public void StartMatch()
    {
        timerOn = true;
    }

    [ClientRpc]
    public void ChangeSprite()
    {
        spriteRenderer.sprite = hushImage;
    }

}
