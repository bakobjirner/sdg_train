using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using TMPro;

public class Moderator : MonoBehaviourPunCallbacks
{

    public PlayerController[] Players;
    public int murderers = 0;
    public int passengers = 0;
    public int security = 0;
    public int saboteurs = 0;

    public GameObject[] Active_Events;

    public GameObject Event_Tunnel;
    public GameObject Event_Station;
    public GameObject AudioTheme;
    private GameObject AudioTheme_Instance;
    public GameObject AudioTracks;
    private GameObject AudioTracks_Instance;

    private bool blockEvents = false;

    // counts the amount of Events that have triggered, terminating the game past a treshold
    private int EventCounter = 0;
    public int EventLimit = 1;
    
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
        AudioTracks_Instance = Instantiate(AudioTracks, new Vector3(0,0,0), Quaternion.identity);
    }

    void Update() {
        if (PhotonNetwork.IsMasterClient && photonView.IsMine) {
            Active_Events = GameObject.FindGameObjectsWithTag("Event");
            if (!blockEvents && Active_Events.Length == 0) {
                blockEvents = true;
                if (EventCounter < EventLimit) {
                    // if we had a list of collected data which determines preference on events to fire
                    // this is where it would be evaluated
                    photonView.RPC("fireEvent", RpcTarget.AllViaServer, "Event_Tunnel");
                    EventCounter++;
                } else {
                    trigger the end-of-game event
                    photonView.RPC("fireEvent", RpcTarget.AllViaServer, "Event_Station");
                }
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
                    case "Saboteur":
                        saboteurs++;
                        break;
                    case "Passenger":
                        passengers++;
                        break;
                }
            }
        }
    }

    // starts the ending theme and eventually quits the player out of the lobby
    // called when Event_Station halts the train
    public void endGame() {
        PUN_Manager.Instance.preventEndGame = true;
        GameObject UI = GameObject.FindGameObjectWithTag("game_ui");
        UI.SetActive(false);
        AudioTracks_Instance.GetComponent<AudioSource>().mute = true;
        AudioTheme_Instance = Instantiate(AudioTheme, new Vector3(0,0,0), Quaternion.identity);
        string summary = "Summary:";
        foreach(PlayerController player in Players) {
            summary += "\n"+player.photonView.Owner.NickName+" was a "+player.role.getRole();
            switch (player.role.getRole()) {
                case "Murderer":
                    summary += "";
                    break;
                case "Security":
                    summary += "";
                    break;
                case "Saboteur":
                    summary += "";
                    break;
                case "Passenger":
                    summary += " ";
                    break;
            }
        }
        // set the billboard to display end of game statistic
        GameObject StationScreen = GameObject.FindGameObjectWithTag("station_screen");
        StationScreen.GetComponentInChildren<TextMeshPro>().text = summary;
        // todo: gracefully quit game
    }

    public void setRoles() {
        Debug.Log("I am the masterclient moderator, setting roles now.");
        if (Players.Length <= 1) {
            Debug.Log("To few players to set roles, returning.");
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
                    Players[i].SetRole("Murderer", "Kill a passenger and reach the station.");
                    murderers++;
                } else if (security <= 0) {
                    Players[i].SetRole("Security", "Don't let anyone die on the train.");
                    security++;
                } else if (saboteurs <= 0) {
                    Players[i].SetRole("Saboteur", "Prevent the train from reaching the station.");
                    saboteurs++;
                } else {
                    Players[i].SetRole("Passenger", "Survive and reach the station.");
                    passengers++;
                }
                Debug.Log("set a new player role: "+Players[i].role.getRole() +" for "+Players[i].nickName);
            }
        }
        PhotonView photonView = PhotonView.Get(this);
        // pun2 can't handle 2d arrays as parameters, AMAZIN
        // lets not use pun ever again
        // synchronizing player roles took me 3hours of debugging with this AMAZIN pun RPC implementation
        string[] roles = new string[Players.Length];
        string[] objectives = new string[Players.Length];
        string[] nicknames = new string[Players.Length];
        for (int i = 0; i < Players.Length; i++) {
            nicknames[i] = Players[i].photonView.Owner.NickName;
            roles[i] = Players[i].role.getRole();
            objectives[i] = Players[i].role.getObjective();
        }
        // gather all player-role-data from the masterclient, then send it to all clients via RPC
        photonView.RPC("SynchRoles", RpcTarget.AllViaServer, nicknames, roles, objectives);
    }

    [PunRPC]
    public void SynchRoles(string[] nicknames, string[] roles, string[] objectives) {
        getAllPlayers();
        for (int i = 0; i < roles.Length; i++) {
            foreach (PlayerController player in Players) {
                if (player.photonView.Owner.NickName.Equals(nicknames[i])) {
                    Debug.Log("Moderator RPC: setting "+player.photonView.Owner.NickName+" to role "+roles[i]);
                    player.SetRole(roles[i], objectives[i]);
                    player.updateUI();
                }
            }
        }
        getAllPlayers();
    }

    [PunRPC]
    public void fireEvent(string name) {
        Debug.Log("starting Event "+name);
        switch (name) {
            case "Event_Tunnel":
                GameObject newObject = Instantiate(Event_Tunnel);
                newObject.tag = "Event";
                break;
            case "Event_Station":
                GameObject newObject2 = Instantiate(Event_Station);
                newObject2.tag = "Event";
                break;
        }
        blockEvents = false;
    }
}