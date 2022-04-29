using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NameTag : MonoBehaviourPun
{
    public string getPlayerName;
    public TextMesh nameTag;
    void Start()
    {
        if(!photonView.IsMine)
        {
            nameTag = gameObject.GetComponent<TextMesh>();
            nameTag.text = photonView.Owner.NickName;
        } else
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        gameObject.transform.LookAt(PUN_Manager.Instance.PlayerInstance.gameObject.transform);
    }
}
