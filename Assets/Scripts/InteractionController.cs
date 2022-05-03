using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{

    [SerializeField] private int rayLength = 5;
    [SerializeField] private LayerMask layerMask;

    private Interactable interactable;
    private const string interactableTag = "interactable";


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if(Physics.Raycast(transform.position,fwd,out hit, rayLength,layerMask))
        {
            if (hit.collider.CompareTag(interactableTag))
            {
                interactable = hit.collider.gameObject.GetComponent<Interactable>();
                
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.interact();
                }
            }
        }
    }
}
