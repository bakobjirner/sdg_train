using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    public static GameObject LocalPlayerInstance;

    public Camera PlayerCamera;
    public float Health = 1.0f;
    public float speed = 1.0f;
    public float shiftSpeed = 1.5f;
    
    public int ActorNumber;

    Vector3 direction;
    float rotationX;

    void Awake() {
        if (photonView.IsMine) {
            PlayerController.LocalPlayerInstance = this.gameObject;
        }
        DontDestroyOnLoad(this.gameObject);
    }



    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine == false) {
            PlayerCamera.enabled = false;
            PlayerCamera.GetComponent<AudioListener>().enabled = false;
        } else {
            ActorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            Debug.Log("init Player, i am ActorNumber" + ActorNumber);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) {
            return;
        }
        GetInput();
    }

    void FixedUpdate() {
        if (!photonView.IsMine) {
            return;
        }
        ExecMovement();
    }

    void GetInput()
    {
        direction = new Vector3(0,0,0);
        if (Input.GetKey(KeyCode.W))
        {
            direction += transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction -= transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction -= transform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += transform.right;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            direction *= shiftSpeed;
        }
        if (Input.GetKey(KeyCode.Space)) {
            direction += transform.up*1.5f;
        }
        // toggle menu
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (Cursor.lockState == CursorLockMode.Locked) {
                Cursor.lockState = CursorLockMode.None;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        // Rotate Camera and Player
        if (Cursor.lockState == CursorLockMode.Locked) {
            rotationX += -Input.GetAxis("Mouse Y") * 3f;
            rotationX = Mathf.Clamp(rotationX, -85.0f, 85.0f);
            PlayerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * 3f, 0);
        }
    }

    void ExecMovement() {
        // apply vector3 direction to transform
        gameObject.GetComponent<Rigidbody>().MovePosition(
            gameObject.GetComponent<Transform>().position +
            direction * speed * Time.deltaTime
        );
    }

    private bool isGrounded() {
        if (Physics.Raycast(transform.position,
            Vector3.down,
            this.GetComponent<MeshFilter>().mesh.bounds.size.y/2+0.01f)) {
                return true;
            }
        return false;
    }

    void OnCollisionEnter(Collision collision) {
        if (!photonView.IsMine) {
            return;
        }
        // Debug.Log(collision.gameObject.tag);
    }

    private void OnTriggerEnter(Collider other) {
        if (!photonView.IsMine) {
            return;
        }
        // Debug.Log(other.gameObject.tag);
    }
}
