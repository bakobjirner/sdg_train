using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    public static GameObject LocalPlayerInstance;

    public Role role;
    public Moderator moderator;
    public GameObject uiGameObject;
    public Camera PlayerCamera;
    public float health = 1.0f;
    public float stamina = 1.0f;
    private bool canSprint = true;
    public float speed = 1.0f;
    public float shiftSpeed = 1.5f;
    public string nickName = "";
    public GameObject Equipment;
    public GameObject Equipment_Overlay;
    public Animator characterAnimator;
    public SkinnedMeshRenderer playerMesh;
    public ParticleSystem gunParticles;
    public int ActorNumber;
    private Color[] colors = { Color.red, Color.green, Color.blue, Color.cyan, Color.yellow, Color.magenta };
    private int myColor = 0;

    Vector3 direction;  
    float rotationX;
    
    public List<string> inventory = new List<string>();
    public int inventorySelectedItem = 0;
    public GameObject rightHand;

    // clip parts of the player model
    public GameObject beard;
    public GameObject hair;


    void Awake() {
        if (photonView.IsMine) {
            PlayerController.LocalPlayerInstance = this.gameObject;
            Renderer renderer = this.gameObject.GetComponent<Renderer>();
            myColor = Random.Range(0, colors.Length - 1);
            Color newColor = colors[myColor];
            renderer.material.SetColor("_Color", newColor);
            updateEquipment();
            uiGameObject = GameObject.FindGameObjectWithTag("game_ui");
            uiGameObject.GetComponent<GameUI>().setVisibility(true);
            // disable player mesh
            // playerMesh.enabled = false;
            beard.layer = LayerMask.NameToLayer("Equipment_Player");
            hair.layer = LayerMask.NameToLayer("Equipment_Player");
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetRole(string name, string objective) {
        role = new Role(name, objective);
        Debug.Log("Player " + ActorNumber + " is now now Role " + role.getRole());
    }

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine == false) {
            PlayerCamera.enabled = false;
            PlayerCamera.GetComponent<AudioListener>().enabled = false;
            updateUI();
        } else {
            ActorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            Debug.Log("init Player, i am ActorNumber" + ActorNumber);
            Cursor.lockState = CursorLockMode.Locked;
            nickName = PhotonNetwork.LocalPlayer.NickName;
            // AddItemToInventory("Pistol", Equipment);
            AddItemToInventory("Hands");
            AddItemToInventory("Gun");
            AddItemToInventory("Shovel");
            // Set the UI
            uiGameObject.GetComponent<GameUI>().SetInventory(0, 0);
            // Enable current Item
            rightHand.transform.Find(inventory[0]).gameObject.SetActive(true);
            PlayerCamera.transform.Find(inventory[0] + "_Overlay").gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) {
            return;
        }
        GetInput();
        CheckInBoundries();
    }

    // Gets called every 0.02 seconds (Time.fixedDeltaTime)
    void FixedUpdate() {
        if (!photonView.IsMine) {
            return;
        }
        ExecMovement();
        ConsumeOrRefillStamina();
        updateUI();
    }

    void GetInput()
    {
        direction = new Vector3(0,0,0);
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (vertical > 0)
        {
            direction += transform.forward;
        } 
        else if (vertical < 0)
        {
            direction -= transform.forward;
        }
        if (horizontal > 0)
        {
            direction += transform.right;
        } 
        else if (horizontal < 0)
        {
            direction -= transform.right;
        }
        characterAnimator.SetFloat("X", vertical);
        characterAnimator.SetFloat("Z", horizontal);
        
        if (Input.GetKey(KeyCode.LeftShift) && canSprint)
        {
            direction *= shiftSpeed;
            characterAnimator.SetFloat("Shift", 1);
        }
        else
        {
            characterAnimator.SetFloat("Shift", 0);
        }
        if (Input.GetKey(KeyCode.Space) && this.GetComponent<Rigidbody>().useGravity) {
            direction += transform.up*1.5f;
            characterAnimator.SetTrigger("Jump");
        }
        if (Input.GetButtonDown("Fire1") && inventory[inventorySelectedItem] == "Gun" && Cursor.lockState == CursorLockMode.Locked) {
            characterAnimator.SetTrigger("Shoot");
            ShootGun();
        }
        // Rotate Camera and Player
        if (Cursor.lockState == CursorLockMode.Locked) {
            rotationX += -Input.GetAxis("Mouse Y") * 3f;
            rotationX = Mathf.Clamp(rotationX, -85.0f, 51.5f);
            PlayerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * 3f, 0);
        }
        HandleInventoryInput();
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

    public void updateUI()
    {
        if (photonView.IsMine)
        {
            GameUI ui = uiGameObject.GetComponent<GameUI>();
            ui.setHealth(health);
            ui.setStamina(stamina);
            if(role != null) {
                ui.setRole(role.getRole());
                ui.setObjective(role.getObjective());
            }
        }
    }

    private void updateEquipment() {
        // set the players physical/observable equipment to a culled layer (dont clip weapons into environment)
        int LayerEquipment_Player = LayerMask.NameToLayer("Equipment_Player");
        Equipment.layer = LayerEquipment_Player;
        foreach(Transform el in Equipment.transform) {
            el.gameObject.layer = LayerEquipment_Player;
        }
        // set the overlay weapon to a non-culled layer (only show the overlay weapon to this player)
        int LayerEquipment_Overlay_Player = LayerMask.NameToLayer("Equipment_Overlay_Player");
        Equipment_Overlay.layer = LayerEquipment_Overlay_Player;
        foreach(Transform el in Equipment_Overlay.transform) {
            el.gameObject.layer = LayerEquipment_Overlay_Player;
        }
    }

    private void OnCollisionEnter(Collision collision) {
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

    public void ShootGun() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit)) {
            if (hit.collider.CompareTag("Player")) {
                // Debug.Log(hit.collider.gameObject.GetComponent<PlayerController>().photonView.Owner.NickName);
                PhotonView.Get(this).RPC("DamagePlayer", RpcTarget.AllViaServer,
                    hit.collider.gameObject.GetComponent<PlayerController>().photonView.Owner.NickName,
                    100.0f, "shot by "+PlayerController.LocalPlayerInstance.GetComponent<PlayerController>().photonView.Owner.NickName);
            }
        }
        PhotonView.Get(this).RPC("PlayGunSound", RpcTarget.AllViaServer);
        
    }

    [PunRPC]
    private void PlayGunSound()
    {
        this.GetComponent<AudioSource>().Play();
        gunParticles.Play();
    }

    [PunRPC]
    public void DamagePlayer(string nickname, float dmg, string reason) {
        Debug.Log("received RPC DamagePlayer "+nickname);
        PlayerController localPC = PlayerController.LocalPlayerInstance.GetComponent<PlayerController>();
        if (localPC.photonView.Owner.NickName.Equals(nickname)) {
            // the hit player was us, refresh UI and Destroy if necessary
            Debug.Log("we were hit");
            localPC.health -= dmg;
            updateUI();
            if (localPC.health <= 0.0f) {
                PhotonView.Get(this).RPC("Die", RpcTarget.AllViaServer, localPC.photonView.ViewID, reason);
            }
        }
    }
    
    [PunRPC]
    private void Die(int viewId, string reason){
        //Destroy(localPC.uiGameObject);
        //PhotonNetwork.Destroy(PlayerController.LocalPlayerInstance);
        //PUN_Manager.Instance.LeaveRoom();
        //UnityEngine.Cursor.lockState = CursorLockMode.None;
        PhotonView v = PhotonNetwork.GetPhotonView(viewId);
        PlayerController playerController = v.GetComponent<PlayerController>();
        Debug.Log("died" + playerController.photonView.Owner.NickName);
        playerController.GetComponent<Rigidbody>().useGravity = false;
        playerController.GetComponent<Rigidbody>().velocity = Vector3.zero;
        playerController.GetComponent<Collider>().enabled = false;
        playerController.inventory = new List<string>();
        playerController.transform.position = new Vector3(this.transform.position.x, 1.5f,
            playerController.transform.position.z);
        foreach (MeshRenderer r in playerController.GetComponentsInChildren<MeshRenderer>())
        {
            r.enabled = false;
        }
        foreach (SkinnedMeshRenderer r in playerController.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            r.enabled = false;
        }
        // update role flags for end of game evaluation
        playerController.role.alive = false;
        playerController.role.deathReason = reason;
    }   

    // we can probably move this to RPCs
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Renderer renderer = this.gameObject.GetComponent<Renderer>();
        if (stream.IsWriting)
        {
            stream.SendNext(myColor);
        }
        else if (stream.IsReading)
        {

            int newColor = (int)stream.ReceiveNext();
            renderer.material.SetColor("_Color", colors[newColor]);
        }
    }

    /**
     *this function gets triggered if a player leaves the world, for example by falling out of the train 
     **/
    private void CheckInBoundries()
    {
        float maxX = 200;
        float minX = -200;
        float maxY = 100;
        float minY = 0;
        float maxZ = 500;
        float minZ = -500;

        if (transform.position.x > maxX ||
            transform.position.x < minX ||
            transform.position.y > maxY ||
            transform.position.y < minY ||
            transform.position.z > maxZ ||
            transform.position.z < minZ){
            PhotonView.Get(this).RPC("DamagePlayer", RpcTarget.AllViaServer,
                   photonView.Owner.NickName,health*2, "fell out of the train");
        }
    }

    void HandleInventoryInput()
    {
        var scroll = Input.mouseScrollDelta.y;
        if (scroll != 0)
        {
            int indexBefore = inventorySelectedItem;
            if (inventory.Count > 1)
            {
                if (scroll > 0)
                {
                    if (inventorySelectedItem == 0)
                    {
                        inventorySelectedItem = inventory.Count - 1;
                    }
                    else
                    {
                        inventorySelectedItem--;
                    }
                } 
                else if (scroll < 0)
                {
                    if (inventorySelectedItem == inventory.Count - 1)
                    {
                        inventorySelectedItem = 0;
                    }
                    else
                    {
                        inventorySelectedItem++;
                    }
                }
                SelectItem(indexBefore, inventorySelectedItem);
            }
        }
    }

    void SelectItem(int previousItem, int itemToSelect)
    {
        // Set the UI
        uiGameObject.GetComponent<GameUI>().SetInventory(previousItem, itemToSelect);
        // Disable previous Item
        rightHand.transform.Find(inventory[previousItem]).gameObject.SetActive(false);
        PlayerCamera.transform.Find(inventory[previousItem] + "_Overlay").gameObject.SetActive(false);
        // Enable current Item
        rightHand.transform.Find(inventory[itemToSelect]).gameObject.SetActive(true);
        PlayerCamera.transform.Find(inventory[itemToSelect] + "_Overlay").gameObject.SetActive(true);
        // Change on other instances
        photonView.RPC("ChangeItem", RpcTarget.AllBuffered, inventory[previousItem], inventory[itemToSelect]);
    }

    [PunRPC]
    void ChangeItem(string previousItem, string itemToSelect)
    {
        rightHand.transform.Find(previousItem).gameObject.SetActive(false);
        rightHand.transform.Find(itemToSelect).gameObject.SetActive(true);
    }

    void AddItemToInventory(string name)
    {
        inventory.Add(name);
        GameUI ui = uiGameObject.GetComponent<GameUI>();
        ui.AddToInventory(name);
    }
}
