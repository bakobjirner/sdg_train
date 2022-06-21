using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arcade : MonoBehaviour
{
    private int rotRed;
    private int rotGreen;
    private int rotBlue;
    //must divide 360
    private int rotateAmount = 36;
    Material material;
    public float frequency = 2;
    private TrainSpawner train;
    private bool used = false;


    public void Start()
    {
        material = this.GetComponent<MeshRenderer>().material;
        material.SetVector("_frequency", new Vector4(frequency, frequency, frequency, frequency));
        int maxNumberRotation = 360 / rotateAmount;
        rotRed = Random.Range(0, maxNumberRotation) *rotateAmount;
        rotGreen = Random.Range(0, maxNumberRotation) *rotateAmount;
        rotBlue = Random.Range(0, maxNumberRotation) *rotateAmount;
        material.SetFloat("_rotRed", rotRed);
        material.SetFloat("_rotGreen", rotGreen);
        material.SetFloat("_rotBlue", rotBlue);
        train = GameObject.FindWithTag("train").GetComponent<TrainSpawner>();
    }
    
    

   
    public void Rotate(int Color)
    {
        Debug.Log("buttonPressed");

        switch (Color)
        {
            case 0: rotRed += rotateAmount;
                break;
            case 1:
                rotGreen += rotateAmount;
                break;
            default:
                rotBlue += rotateAmount;
                break;
        }
        material.SetFloat("_rotRed", rotRed);
        material.SetFloat("_rotGreen", rotGreen);
        material.SetFloat("_rotBlue", rotBlue);

        if (rotRed%360==0&& rotGreen % 360 == 0 && rotBlue % 360 == 0)
        {
            if(!used)
            {
                train.DetachLastWagon();
                used = true;
            }
            
        }
    }

}
