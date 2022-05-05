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

    public GameObject text;

    private void Start()
    {
        value = maxValue;
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
        text.transform.Rotate(Vector3.up, 180);
        lookedAt = false; 
    }

    public override void interact()
    {
        Debug.Log("put coal in fire");
        value = maxValue;
        //TODO: sync fire-value
    }
}
