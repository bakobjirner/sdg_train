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
}
