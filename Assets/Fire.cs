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
        value -= burnrate * Time.deltaTime;
        text.SetActive(lookedAt);
        text.transform.LookAt(player);
        lookedAt = false;
        updateUI();
    }

    [PunRPC]
    public void interactRemote()
    {
        Debug.Log("put coal in fire");
        value = maxValue;
    }

    public override void interact()
    {
        photonView.RPC("interactRemote", RpcTarget.All);
    }

    private void updateUI()
    {
        if (photonView.IsMine)
        {
            GameUI ui = uiGameObject.GetComponent<GameUI>();
            ui.SetFireValue(value);
        }
    }
}
