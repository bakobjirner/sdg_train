using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class PUN_Manager : MonoBehaviourPunCallbacks
{
    public static PUN_Manager Instance;

    // prefabs
    public GameObject Moderator;
    public GameObject Player;
    
    // instances
    private Moderator ModeratorInstance;
    public GameObject PlayerInstance;

    private GameObject RespawnLocation;

    void Start() {
        Instance = this;
        RespawnLocation = GameObject.FindGameObjectWithTag("RespawnLocation");
        if(PlayerController.LocalPlayerInstance == null) {
            this.PlayerInstance = PhotonNetwork.Instantiate(this.Player.name, this.RespawnLocation.transform.position, Quaternion.identity);
        }
        this.ModeratorInstance = PhotonNetwork.Instantiate(this.Moderator.name, new Vector3(0,0,0), Quaternion.identity).GetComponent<Moderator>();
    }

    public void RespawnPlayer(String team) {
        RespawnLocation = GameObject.FindGameObjectWithTag("RespawnLocation");
        PhotonNetwork.Destroy(PlayerController.LocalPlayerInstance);
        this.PlayerInstance = PhotonNetwork.Instantiate(this.Player.name, this.RespawnLocation.transform.position, Quaternion.identity);
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.LogFormat("PhotonNetwork : PlayerCount : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel("MultiplayerSample");
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
        StartCoroutine(updateModerators());
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        ModeratorInstance.getAllPlayers();
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects
    }

    public IEnumerator updateModerators() {
        yield return new WaitForSeconds(5);
        this.ModeratorInstance.getAllPlayers();
        if (PhotonNetwork.IsMasterClient) {
            this.ModeratorInstance.setRoles();
        }
    }
}