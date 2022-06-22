using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_Station : MonoBehaviour
{
    // what does this event do?
    //  1.  spawn the station, pretend-move it until it covers the train.
    //  2.  progressively slow down the landscape as the station approaches, finally halting all movement.
    //  3.  the game loops ends here, disable all damage. display the game results on the station display.
    // when do we fire this event?
    //  after 5 Minutes we let the train arrive at the final station, ending the game.

    public string name = "station";
    // does this event run local or synchronized
    public bool pun = false;

    public GameObject Station;
    public GameObject LandscapeGenerator;
    public LandscapeGeneratorShader LandscapeScript;

    private GameObject Station_Instance;

    private float stopAtZ = -30.0f;
    private bool moving = false;

    void Start()
    {
        Station_Instance = Instantiate(Station, new Vector3(0, 0, 500), Quaternion.identity);
        moving = true;
        LandscapeGenerator = GameObject.FindWithTag("landscape");
        LandscapeScript = LandscapeGenerator.GetComponent<LandscapeGeneratorShader>();
    }

    void FixedUpdate()
    {
        if (moving)
        {
            Station_Instance.transform.Translate(new Vector3(0,0,-13.5f) * Time.deltaTime);
        }
        if (Station_Instance.transform.position.z < stopAtZ && moving)
        {
            moving = false;
            LandscapeScript.scrollSpeed = 0;
            PlayerController.LocalPlayerInstance.GetComponent<PlayerController>().moderator.endGame();
        }
    }
}
