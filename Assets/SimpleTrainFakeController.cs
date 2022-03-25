using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrainFakeController : MonoBehaviour
{

    public float speed;
    public float turnspeed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.forward * speed * Time.deltaTime;
        transform.position = transform.position + movement;
    }
}
