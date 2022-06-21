using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_Tunnel : MonoBehaviour
{
    // what does this event do?
    //  1.  spawn the mountain and tunnel system, pretend-move it until it covers the train
    //      and the train sits in the middle of the tunnel
    //  2.  then stop moving the tunnel and set the shaders Speed to 5 to cover this
    //  3.  once the hold-timer has run out, stop the shaders Speed and begin moving the tunnel past the train
    // when do we fire this event?
    //      this event is supposed to provide opportunity for distraction by creating a low light situation
    //      fire this event to support roles that have trouble completing their objective unseen

    public string name = "tunnel";
    // does this event run local or synchronized
    public bool pun = false;

    public GameObject Tunnel;
    public Material Tunnel_Material;

    private GameObject Tunnel_Instance;

    private float stopAtZ = 30.0f;
    private float pauseUntil = 0.0f;
    private bool moving = false;
    private bool halting = false;
    private bool selfDestroy = false;

    void Start()
    {
        Tunnel_Instance = Instantiate(Tunnel, new Vector3(0,7.7f, 500), Quaternion.identity);
        Tunnel_Material.SetFloat("_Speed", 0.0f);
        moving = true;
    }

    void FixedUpdate()
    {
        if (moving)
        {
            Tunnel_Instance.transform.Translate(new Vector3(0,0,-13.5f) * Time.deltaTime);
        }
        // we have stopped once already, now destroy the Event
        if (Tunnel_Instance.transform.position.z < stopAtZ && selfDestroy)
        {
            Destroy(Tunnel_Instance);
            Destroy(gameObject);
        }
        // we have arrived at the stop, fake move with the shader now
        if (Tunnel_Instance.transform.position.z < stopAtZ && !halting)
        {
            haltTrain();
        }
        // stop the shader, start moving the tunnel again
        if (halting && Time.time > pauseUntil) {
            moveTrain();
        }
    }

    private void haltTrain() {
        halting = true;
        moving = false;
        // rm random time inside event, needs to be fully deterministic if not synched through pun
        // pauseUntil = Time.time + Random.Range(30,90);
        pauseUntil = Time.time + 30.0f;
        Tunnel_Material.SetFloat("_Speed", 5.0f);
        stopAtZ = -500;
        selfDestroy = true;
    }

    private void moveTrain() {
        halting = false;
        moving = true;
        Tunnel_Material.SetFloat("_Speed", 0.0f);
    }

}
