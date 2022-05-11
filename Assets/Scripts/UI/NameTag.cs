using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class NameTag : MonoBehaviourPun
{
    public TextMeshPro nameTag;
    void Start()
    {
        if(!photonView.IsMine)
        {
            nameTag = gameObject.GetComponentInChildren<TextMeshPro>();
            nameTag.text = photonView.Owner.NickName;
        } else
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        transform.LookAt(PUN_Manager.Instance.PlayerInstance.gameObject.transform.position);
    }
}
