using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{

    [SerializeField] private int rayLength = 5;
    [SerializeField] private LayerMask layerMask;

    private Interactable interactable;
    private const string interactableTag = "interactable";
    public PlayerController playerController;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (this.GetComponentInParent<PhotonView>().IsMine)
        {
            if(Physics.Raycast(transform.position,fwd,out hit, rayLength,layerMask))
            {
                if (hit.collider.CompareTag(interactableTag))
                {
                    interactable = hit.collider.gameObject.GetComponent<Interactable>();

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                                            
                        if (hit.collider.gameObject.name == "fire")
                        {
                            Debug.Log("name == fire");
                            if (playerController.inventory[playerController.inventorySelectedItem] == "Shovel")
                            {
                                interactable.interact();
                                Debug.Log("inventory Shobel");
                            }
                        }
                        else
                        {
                            interactable.interact();
                        }
                    }
                    else
                    {
                        interactable.hover(this.transform);
                    }
                }
            }
        }
    }
}
