using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameUI : MonoBehaviour
{
    private Label Role;
    private VisualElement HealthBar;
    private Label HealthText;
    private VisualElement StaminaBar;
    private ProgressBar progressBar;
    private VisualElement infoContainer;
    private VisualElement inventoryContainer;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        Role = root.Q<Label>("Role");
        HealthBar = root.Q<VisualElement>("HealthBar");
        HealthText = root.Q<Label>("HealthText");
        StaminaBar = root.Q<VisualElement>("StaminaBar");
        progressBar = root.Q<ProgressBar>("fire_status_bar");
        infoContainer = root.Q<VisualElement>("info_container");
        inventoryContainer = root.Q<VisualElement>("inventoryContainer");

        setVisibility(false);
    }

    private void Update()
    {
        
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
    public void SetFireValue(float value)
    {
        progressBar.value = value;
    }

    public void setVisibility(bool visibility)
    {
        progressBar.visible = visibility;
        infoContainer.visible = visibility;
    }

    public void AddToInventory(string name)
    {
        Label newInventoryLabel = new Label(name);
        newInventoryLabel.AddToClassList("unselectedItem");
        inventoryContainer.Add(newInventoryLabel);
    }

    public void SetInventory(int indexBefore, int newIndex)
    {
        VisualElement selectedItem = inventoryContainer.ElementAt(indexBefore);
        selectedItem.RemoveFromClassList("selectedItem");
        selectedItem.AddToClassList("unselectedItem");
        VisualElement item = inventoryContainer.ElementAt(newIndex);
        item.AddToClassList("selectedItem");
        item.RemoveFromClassList("unselectedItem");
    }
}
