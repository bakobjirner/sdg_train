using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameUI : MonoBehaviour
{
    private VisualElement healthBar;
    private Label healthText;
    private VisualElement staminaBar;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        healthBar = root.Q<VisualElement>("HealthBar");
        healthText = root.Q<Label>("HealthText");
        staminaBar = root.Q<VisualElement>("StaminaBar");
    }

    public void setHealth(float health)
    {
        healthBar.style.width = Length.Percent(health * 100f);
        healthText.text = (health * 100f).ToString();
    }

    public void setStamina(float stamina)
    {
        staminaBar.style.width = Length.Percent(stamina * 100f);
    }
}
