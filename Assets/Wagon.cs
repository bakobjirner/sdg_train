using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    public float speed = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += speed*Time.deltaTime*Vector3.back;
    }

    public void Detach(float speed)
    {
        this.speed = speed;
        Door[] doors = this.GetComponentsInChildren<Door>();
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].isEnabled = false;
        }
    }
}
