using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class PlayerNameTag : NetworkBehaviour
{
    public TMP_Text gamertag; // this is the UI that floats above the head of players.

    [SyncVar(hook = "DisplayPlayerName")] public string playerDisplayName;

    // Start is called before the first frame update
    void Start()
    {

        // the "NAMEKEY" can be whatever you want but in my project, this is where
        // i store the name of my player. it is set in the menu of the game
        if (isLocalPlayer) { CmdSendName(PlayerPrefs.GetString("NAMEKEY")); }
    }

    [Command]
    public void CmdSendName(string playerName)
    {
        playerDisplayName = playerName;

        // no rpc needed because the magic of SyncVar.
        // a syncvar listen to data that is on the server
        // thus, we change data on the server and ALL clients update the name with the use of the hook.
    }

    public void DisplayPlayerName(string oldName, string newName)
    {
        Debug.Log("Player changed name from " + oldName + " to " + newName);

        gamertag.text = newName;
    }
}
