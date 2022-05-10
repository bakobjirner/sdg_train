using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviourPun
{
    public abstract void interact();

    public abstract void hover(Transform player);

}
