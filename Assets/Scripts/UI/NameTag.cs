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
            nameTag = gameObject.GetComponent<TextMeshPro>();
            nameTag.text = photonView.Owner.NickName;
        } else
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        gameObject.transform.LookAt(PUN_Manager.Instance.PlayerInstance.gameObject.transform);
        Vector3 rotation = Quaternion.LookRotation(PUN_Manager.Instance.PlayerInstance.gameObject.transform.position).eulerAngles;
        rotation.x = 0f;
        transform.rotation = Quaternion.Euler(rotation);
    }
}
