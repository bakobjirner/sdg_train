using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public float speed = .1f;
    public Transform doorLeft;
    public Transform doorRight;
    public float maxOpening;
    private bool open;
    private Vector3 targetLeft;
    private Vector3 targetRight;
    private Vector3 positionOpenLeft;
    private Vector3 positionClosedLeft;
    private Vector3 positionOpenRight;
    private Vector3 positionClosedRight;

    // Start is called before the first frame update
    void Start()
    {
        if (doorLeft != null)
        {
           positionOpenLeft = doorLeft.position + -transform.right * maxOpening;
            positionClosedLeft = doorLeft.position;
            targetLeft = positionClosedLeft;
        }
        if (doorRight != null)
        {
            positionOpenRight = doorRight.position + transform.right * maxOpening;
            positionClosedRight = doorRight.position;
            targetRight = positionClosedRight;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
            if (doorLeft != null)
            {
                doorLeft.transform.position = Vector3.MoveTowards(doorLeft.transform.position, targetLeft, speed);
            }
            if (doorRight != null)
            {
                doorRight.transform.position = Vector3.MoveTowards(doorRight.transform.position, targetRight, speed);
            }
    }

    public void Open()
    {
        if (doorLeft != null)
        {
            targetLeft = positionOpenLeft;
        }
        if (doorRight != null)
        {
            targetRight = positionOpenRight;
        }
        open = true;
    }
    public void Close()
    {
        if (doorLeft != null)
        {
            targetLeft = positionClosedLeft;
        }
        if (doorRight != null)
        {
            targetRight = positionClosedRight;
        }
        open = false;
    }

    public override void interact()
    {
        Debug.Log("Interact");
        if (open)
        {
            Close();
            GetComponent<BoxCollider>().isTrigger = false;
        }
        else
        {
            Open();
            GetComponent<BoxCollider>().isTrigger = true;
        }
    }
}
