using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

public class Moderator : MonoBehaviourPunCallbacks
{
    public GameObject[] Players;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient) {
            Debug.Log("Moderator: You are not the MasterClient.");
        } else {
            Debug.Log("Moderator: You are the MasterClient!");
            getAllPlayers();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient) {

        }
    }

    // call this when game starts and players disconnect or reconnect
    void getAllPlayers() {
        Players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log("getAllPlayers: "+Players.Length);
    }
}
