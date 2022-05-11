using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Interactable
{
    private bool lookedAt;
    Transform player;
    public float maxValue = 100;
    public float value;
    public float burnrate = .1f;
    public GameObject uiGameObject;

    public GameObject text;

    private void Start()
    {
        value = maxValue;
        uiGameObject = GameObject.FindGameObjectWithTag("game_ui");
    }
    public override void hover(Transform player)
    {
        lookedAt = true;
        this.player = player;
    }

    private void Update()
    {
        text.SetActive(lookedAt);
            text.transform.LookAt(player);
            lookedAt = false;
            updateUI();
    }

    private void FixedUpdate()
    {
        if (PUN_Manager.Instance.IsGameRunning() && PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("decreaseValue", RpcTarget.All);
        }
    }

    [PunRPC]
    private void decreaseValue()
    {
        value -= burnrate;
        if (value <= 0)
        {
            EndGame();
        }
    }

    [PunRPC]
    public void interactRemote()
    {
        value = maxValue;
    }

    public override void interact()
    {
        photonView.RPC("interactRemote", RpcTarget.All);
    }

    private void updateUI()
    {
            GameUI ui = uiGameObject.GetComponent<GameUI>();
            ui.SetFireValue(value);
    }

    private void EndGame()
    {
        GameUI ui = uiGameObject.GetComponent<GameUI>();
        ui.setVisibility(false);
        GameObject[] players = new GameObject[0];
        PUN_Manager.Instance.EndGame("train ran out of coal", players);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(photonView);
        }
    }
}
