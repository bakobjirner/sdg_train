using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    public static GameObject LocalPlayerInstance;

    public Role role;

    public GameObject uiPrefab;
    public GameObject uiGameObject;
    public Camera PlayerCamera;
    public float health = 1.0f;
    public float stamina = 1.0f;
    private bool canSprint = true;
    public float speed = 1.0f;
    public float shiftSpeed = 1.5f;

    public GameObject Flintlock;
   
    public int ActorNumber;

    Vector3 direction;
    float rotationX;

    void Awake() {
        if (photonView.IsMine) {
            PlayerController.LocalPlayerInstance = this.gameObject;
            var Renderer = this.gameObject.GetComponent<Renderer>();
            Color newColor = new Color(Random.Range(0.0f, 1.0f),Random.Range(0.0f, 1.0f),Random.Range(0.0f, 1.0f));
            Renderer.material.SetColor("_Color", newColor);
            int LayerEquipment_Player = LayerMask.NameToLayer("Equipment_Player");
            Flintlock.layer = LayerEquipment_Player;
            foreach(Transform el in Flintlock.transform) {
                el.gameObject.layer = LayerEquipment_Player;
            }
            uiGameObject = Instantiate(uiPrefab);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetRole(string name) {
        role = new Role(name);
        Debug.Log("Player " + ActorNumber + " is now now Role " + role.getRole());
    }

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine == false) {
            PlayerCamera.enabled = false;
            PlayerCamera.GetComponent<AudioListener>().enabled = false;
            SetHealthAndStaminaToUI();
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

    // Gets called every 0.02 seconds (Time.fixedDeltaTime)
    void FixedUpdate() {
        if (!photonView.IsMine) {
            return;
        }
        ExecMovement();
        ConsumeOrRefillStamina();
        SetHealthAndStaminaToUI();
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
        if (Input.GetKey(KeyCode.LeftShift) && canSprint)
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

    // we should prefer to keep all Input polling inside 'Update' and not 'FixedUpdate' (which this is)
    // so that no GetKeys are skipped and Keys are read consistently. but it probably works fine here :) 
    void ConsumeOrRefillStamina()
    {
        if (Input.GetKey(KeyCode.LeftShift) && canSprint)
        {
            stamina -= Time.fixedDeltaTime / 2f;
            if(stamina <= 0)
            {
                canSprint = false;
                stamina = 0f;
            }
        } 
        else if(stamina < 1.0f)
        {
            stamina += Time.fixedDeltaTime / 3f;
            if(stamina > 0.2f)
            {
                canSprint = true;
            }
            if(stamina > 1.0f)
            {
                stamina = 1.0f;
            }
        }
    }

    private bool isGrounded() {
        if (Physics.Raycast(transform.position,
            Vector3.down,
            this.GetComponent<MeshFilter>().mesh.bounds.size.y/2+0.01f)) {
                return true;
            }
        return false;
    }

    private void SetHealthAndStaminaToUI()
    {
        GameUI ui = uiGameObject.GetComponent<GameUI>();
        ui.setHealth(health);
        ui.setStamina(stamina);
        if(role != null) {
            ui.setRole(role.getRole());
        }
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
