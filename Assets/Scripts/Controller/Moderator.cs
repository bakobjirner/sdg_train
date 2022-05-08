using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

public class Moderator : MonoBehaviourPunCallbacks
{

    public PlayerController[] Players;
    public int murderers = 0;
    public int passengers = 0;
    public int security = 0;

    public GameObject[] Active_Events;

    public GameObject Event_Tunnel;

    private bool blockEvents = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine) {
            return;
        }
        if (!PhotonNetwork.IsMasterClient) {
            Debug.Log("Moderator: You are not the MasterClient.");
        } else {
            Debug.Log("Moderator: You are the MasterClient!");
        }
        getAllPlayers();
    }

    void Update() {
        if (PhotonNetwork.IsMasterClient && photonView.IsMine) {
            Active_Events = GameObject.FindGameObjectsWithTag("Event");
            if (!blockEvents && Active_Events.Length == 0) {
                blockEvents = true;
                photonView.RPC("fireEvent", RpcTarget.AllViaServer, "Event_Tunnel");
            }
        }
    }

    // call this whenever player count changes (game start, connect, disconnect)
    // track up to date players, their roles and interesting stats
    public void getAllPlayers() {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Player");
        Players = new PlayerController[allObjects.Length];
        for (int i = 0; i < allObjects.Length; i++) {
            Players[i] = allObjects[i].GetComponent<PlayerController>();
        }
        for (int i=0; i < Players.Length; i++) {
            if (Players[i].role != null) {
                switch (Players[i].role.getRole()) {
                    case "Murderer":
                        murderers++;
                        break;
                    case "Security":
                        security++;
                        break;
                    case "Passenger":
                        passengers++;
                        break;
                }
            }
        }
    }

    public void setRoles() {
        if (Players.Length <= 1) {
            return;
        }
        // first shuffle player order
        for (int i = 0; i < Players.Length; i++) {
            PlayerController temp = Players[i];
            int rIndex = Random.Range(0, Players.Length);
            Players[i] = Players[rIndex];
            Players[rIndex] = temp;
        }
        // then assign roles where currently unset
        for (int i = 0; i < Players.Length; i++) {
            if (Players[i].role == null) {
                if (murderers <= 0) {
                    Players[i].SetRole("Murderer");
                    murderers++;
                } else if (security <= 0) {
                    Players[i].SetRole("Security");
                    security++;
                } else {
                    Players[i].SetRole("Passenger");
                    passengers++;
                }
            }
        }
        PhotonView photonView = PhotonView.Get(this);
        // pun2 can't handle 2d arrays as parameters, AMAZIN
        // lets not use pun ever again
        // synchronizing player roles took me 3hours of debugging with this AMAZIN pun RPC implementation
        string[] roles = new string[Players.Length];
        string[] nicknames = new string[Players.Length];
        for (int i = 0; i < Players.Length; i++) {
            nicknames[i] = Players[i].photonView.Owner.NickName;
            roles[i] = Players[i].role.getRole();
        }
        photonView.RPC("SynchRoles", RpcTarget.AllViaServer, nicknames, roles);
    }

    [PunRPC]
    public void SynchRoles(string[] nicknames, string[] roles) {
        getAllPlayers();
        for (int i = 0; i < roles.Length; i++) {
            foreach (PlayerController player in Players) {
                if (player.photonView.Owner.NickName.Equals(nicknames[i])) {
                    Debug.Log("Moderator RPC: setting "+player.photonView.Owner.NickName+" to role "+roles[i]);
                    player.SetRole(roles[i]);
                    player.updateUI();
                }
            }
        }
        getAllPlayers();
    }

    [PunRPC]
    public void fireEvent(string name) {
        switch (name) {
            case "Event_Tunnel":
                GameObject newObject = Instantiate(Event_Tunnel);
                newObject.tag = "Event";
            break;
        }
        blockEvents = false;
    }
}