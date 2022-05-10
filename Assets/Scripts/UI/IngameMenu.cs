using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class IngameMenu : MonoBehaviour
{
    public VisualTreeAsset ingameMenu;
    private UIDocument uiDocument;
    private Button respawnButton;
    private Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        uiDocument = gameObject.GetComponent<UIDocument>();
    }

    // Update is called once per frame
    void Update()
    {
        // toggle menu
        if (PUN_Manager.Instance.IsGameRunning() && Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        if (UnityEngine.Cursor.lockState == CursorLockMode.Locked)
        {
            uiDocument.visualTreeAsset = ingameMenu;
            respawnButton = uiDocument.rootVisualElement.Q<Button>("respawnButton");
            quitButton = uiDocument.rootVisualElement.Q<Button>("quitButton");
            respawnButton.clicked += RespawnButtonPressed;
            quitButton.clicked += QuitButtonPressed;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            uiDocument.visualTreeAsset = null;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            respawnButton.clicked -= RespawnButtonPressed;
            quitButton.clicked -= QuitButtonPressed;
        }
    }

    public void RespawnButtonPressed()
    {
        Destroy(PUN_Manager.Instance.PlayerInstance.gameObject.GetComponent<PlayerController>().uiGameObject);
        PUN_Manager.Instance.RespawnPlayer();
        ToggleMenu();
    }

    public void QuitButtonPressed()
    {
        PUN_Manager.Instance.LeaveRoom();
        Application.Quit();
    }    
}
