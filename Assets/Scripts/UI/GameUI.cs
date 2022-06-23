using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameUI : MonoBehaviour
{
    private Label Role;
    private Label Objective;
    private VisualElement HealthBar;
    private Label HealthText;
    private VisualElement StaminaBar;
    private ProgressBar progressBar;
    private VisualElement infoContainer;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        Role = root.Q<Label>("Role");
        Objective = root.Q<Label>("Objective");
        HealthBar = root.Q<VisualElement>("HealthBar");
        HealthText = root.Q<Label>("HealthText");
        StaminaBar = root.Q<VisualElement>("StaminaBar");
        progressBar = root.Q<ProgressBar>("fire_status_bar");
        infoContainer = root.Q<VisualElement>("info_container");
        setVisibility(false);
    }

    public void setHealth(float health)
    {
        HealthBar.style.width = Length.Percent(health * 100f);
        HealthText.text = (health * 100f).ToString();
    }

    public void setStamina(float stamina)
    {
        StaminaBar.style.width = Length.Percent(stamina * 100f);
    }

    public void setRole(string description)
    {
        Role.text = description;
    }

    public void setObjective(string description) {
        Objective.text = description;
    }

    public void SetFireValue(float value)
    {
        progressBar.value = value;
    }

    public void setVisibility(bool visibility)
    {
        progressBar.visible = visibility;
        infoContainer.visible = visibility;
    }
}
