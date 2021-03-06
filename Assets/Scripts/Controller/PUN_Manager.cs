
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class PUN_Manager : MonoBehaviourPunCallbacks
{
    public static PUN_Manager Instance;

    // prefabs
    public GameObject Moderator;
    public GameObject Player;
    public VisualTreeAsset gameOverLobby;

    // instances
    private Moderator ModeratorInstance;
    public GameObject PlayerInstance;
    public TrainSpawner train;

    private List<Vector3> spawnLocations;

    public GameObject UI;
    private GameObject lobbyGo;
    private bool GameRunning = false;

    public bool preventEndGame = false;


    void Start() {
        Instance = this;
        ThrowPlayersInLobby();
    }

    public void ThrowPlayersInLobby()
    {
        lobbyGo = GameObject.FindGameObjectWithTag("Lobby");
        Player[] playerList = PhotonNetwork.PlayerList;
        
        for (int i = 0; i < playerList.Length; i++)
        {
            lobbyGo.GetComponent<Lobby>().AddPlayer(playerList[i].NickName);
        }
    }

    public void StartGame()
    {

        spawnLocations = train.getSpawnLocations();

        GameRunning = true;

        // Disable Lobby UI
        UI.GetComponent<UIDocument>().visualTreeAsset = null;

        // Instance Players   
        if (PlayerController.LocalPlayerInstance == null) {
            Vector3 spawnPos = spawnLocations[Random.Range(0,spawnLocations.Count-1)];
            Debug.Log(spawnPos);
            this.PlayerInstance = PhotonNetwork.Instantiate(this.Player.name, spawnPos, Quaternion.identity);
        }
        if (this.ModeratorInstance == null) {
            this.ModeratorInstance = PhotonNetwork.Instantiate(this.Moderator.name, new Vector3(0,0,0), Quaternion.identity).GetComponent<Moderator>();
            this.PlayerInstance.GetComponent<PlayerController>().moderator = this.ModeratorInstance;
        }

        // Disable Lobby Camera
        GameObject lobbyCam = lobbyGo.transform.GetChild(0).gameObject;
        lobbyCam.GetComponent<Camera>().enabled = false;
        StartCoroutine(updateModerators());
    }

    public void EndGame(string reason)
    {
        if (preventEndGame) {
            return;
        }
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        //delete Player

        //enable Lobby Camera
        GameObject lobbyCam = lobbyGo.transform.GetChild(0).gameObject;
        lobbyCam.GetComponent<Camera>().enabled = true;
        // Enable Game-Over-Lobby UI
        UI.GetComponent<UIDocument>().visualTreeAsset = gameOverLobby;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        lobbyGo.GetComponent<Lobby>().showGameOver(UI.GetComponent<UIDocument>().rootVisualElement, reason, players);
    }

    public void RespawnPlayer() {
        Vector3 spawnPos = spawnLocations[Random.Range(0, spawnLocations.Count - 1)];
        if (PlayerController.LocalPlayerInstance != null) {
            PhotonNetwork.Destroy(PlayerController.LocalPlayerInstance);
        }
        this.PlayerInstance = PhotonNetwork.Instantiate(this.Player.name, spawnPos, Quaternion.identity);
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
        lobbyGo.GetComponent<Lobby>().AddPlayer(other.NickName);
        if (GameRunning) {
            StartCoroutine(updateModerators());
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        lobbyGo.GetComponent<Lobby>().RemovePlayer(other.NickName);
        ModeratorInstance.getAllPlayers();
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects
    }

    public IEnumerator updateModerators() {
        yield return new WaitForSeconds(3);
        ModeratorInstance.getAllPlayers();
        if (PhotonNetwork.IsMasterClient) {
            ModeratorInstance.setRoles();
        }
    }

    public bool IsGameRunning()
    {
        return GameRunning;
    }
}