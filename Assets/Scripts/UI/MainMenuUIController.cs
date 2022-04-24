using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUIController : MonoBehaviour
{
    // Store the PlayerPref Key to avoid typos
    const string playerNamePrefKey = "PlayerName";
    private Button joinButton;
    private TextField nameInput;
    private Label errorMessage;
    [SerializeField]
    private Launcher launcher;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        joinButton = root.Q<Button>("joinButton");
        nameInput = root.Q<TextField>("nameInput");
        errorMessage = root.Q<Label>("errorMessage");
        if(joinButton != null)
        {
            joinButton.clicked += JoinButtonPressed;
        }
        string defaultName = string.Empty;
        if (PlayerPrefs.HasKey(playerNamePrefKey))
        {
            defaultName = PlayerPrefs.GetString(playerNamePrefKey);
            nameInput.value = defaultName;
        }
    }

    private void JoinButtonPressed()
    {
        joinButton.clicked -= JoinButtonPressed;
        errorMessage.style.visibility = Visibility.Hidden;

        string playerName = nameInput.value;
        if (string.IsNullOrEmpty(playerName))
        {
            errorMessage.text = "Please enter a avlid login name.";
            errorMessage.style.visibility = Visibility.Visible;
            joinButton.clicked += JoinButtonPressed;
            return; 
        }
        SetPlayerName(playerName);
        joinButton.text = "Joining";
        InvokeRepeating("LoadingAnimation", 0f, 0.2f);
        launcher.Connect();
    }

    public void SetPlayerName(string value)
    {
        PhotonNetwork.NickName = value;
        PlayerPrefs.SetString(playerNamePrefKey, value);
    }

    public void ConnectingFailed()
    {
        CancelInvoke();
        joinButton.text = "Joining";
        errorMessage.text = "Connection failed, please try again.";
        errorMessage.style.visibility = Visibility.Visible;
        joinButton.clicked += JoinButtonPressed;
    }

    private void LoadingAnimation()
    {
        if(joinButton.text == "Joining")
        {
            joinButton.text = "Joining.";
        }
        else if (joinButton.text == "Joining.")
        {
            joinButton.text = "Joining..";
        }
        else if (joinButton.text == "Joining..")
        {
            joinButton.text = "Joining...";
        }
        else if (joinButton.text == "Joining...")
        {
            joinButton.text = "Joining";
        }
    }
}
