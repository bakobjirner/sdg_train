using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;

public class Lobby : MonoBehaviour
{
    public GameObject uiGo;
    private VisualElement playerlistContainer;
    private Button startGameButton;
    private PhotonView photonView;

    private void Awake()
    {
        var root = uiGo.GetComponent<UIDocument>().rootVisualElement;
        playerlistContainer = root.Q<VisualElement>("PlayerlistContainer"); 
        startGameButton = root.Q<Button>("StartGameButton");
        startGameButton.clicked += StartGameButtonClicked;
        photonView = gameObject.GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPlayer(string playerName)
    {
        Label label = new Label(playerName);
        label.style.color = new Color(0, 0, 0);
        playerlistContainer.Add(label);
    }

    public void RemovePlayer(string playerName)
    {
        // Start at i = 1 because first Element is the header
        for (int i = 1; i < playerlistContainer.childCount; i++)
        {
            Debug.Log(playerlistContainer[i]);
            Label label = (Label)playerlistContainer[i];
            if (label.text == playerName)
            {
                playerlistContainer.Remove(playerlistContainer.ElementAt(i));
                break;
            }
        }
    }

    private void StartGameButtonClicked()
    {
        PUN_Manager.Instance.StartGame();
        photonView.RPC("RemoteStartGameCall", RpcTarget.All);
    }

    [PunRPC]
    public void RemoteStartGameCall()
    {
        PUN_Manager.Instance.StartGame();
    }

    public void showGameOver(VisualElement gameOverRoot, string reason, GameObject[] players)
    {
        Debug.Log(players.Length);
        VisualElement gameOverListContainer = gameOverRoot.Q<VisualElement>("game_over_list_container");
        Label titel = gameOverRoot.Q<Label>("title");
        titel.text = "Game over because " + reason;
        

        //print players
        for(int i = 0; i< players.Length; i++)
        {
            VisualElement playerContainer = new VisualElement();
            Label nameLabel = new Label(players[i].GetComponent<PlayerController>().nickName);
            nameLabel.style.color = new Color(0, 0, 0);
            string role = players[i].GetComponent<PlayerController>().role != null ? players[i].GetComponent<PlayerController>().role.getRole() : "no role";
            Label roleLabel = new Label(role);
            roleLabel.style.color = new Color(0, 0, 0);
            Label resultLabel = new Label("lost because the train stopped");
            resultLabel.style.color = new Color(1, 1, 1);
            resultLabel.style.backgroundColor = new Color(1, 0, 0);
            playerContainer.Add(nameLabel);
            playerContainer.Add(roleLabel);
            playerContainer.Add(resultLabel);
            playerContainer.style.flexDirection = FlexDirection.Row;
            gameOverListContainer.Add(playerContainer);
        }
    }
}
