using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Unity.Mathematics;

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
        // transform.LookAt(PUN_Manager.Instance.PlayerInstance.gameObject.transform.position);
        transform.LookAt(PUN_Manager.Instance.PlayerInstance.gameObject.transform.position);
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z);
        // Vector3 rotation = Quaternion.LookRotation(PUN_Manager.Instance.PlayerInstance.gameObject.transform.position).eulerAngles;
        // rotation.x = 0f;
        // transform.rotation = Quaternion.Euler(rotation);
    }
}
