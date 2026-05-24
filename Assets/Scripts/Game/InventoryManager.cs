using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [Header("Interaction UI")]
    public GameObject interactPromptUI;
    public static InventoryManager Instance;

    [Header("Gameplay State")]
    public string currentItem = ""; 
    public int deliveryPhase = 1;  
    public float currentBarya = 0f;
    public float targetBarya = 50f;

    [Header("UI Text Fields")]
    public TextMeshProUGUI baryaText;
    public TextMeshProUGUI taskText; 
    public TextMeshProUGUI itemText; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ShowInteractPrompt(false);
        UpdateGameplayUI();
    }

    public void UpdateGameplayUI()
    {
        // Update Barya Display
        if (baryaText != null) baryaText.text = $"₱{currentBarya:F2}";

        if (taskText != null)
        {
            if (currentBarya >= targetBarya)
            {
                taskText.text = "Balik ka na sa Tindahan.";
                return;
            }

            if (currentItem == "")
            {
                taskText.text = $"Punta sa Tindahan.";
            }
            else
            {
                taskText.text = $"Hanapin ang Nagpapabili at ihatid ang {currentItem}.";
            }
        }

        if (itemText != null)
        {
            if(currentItem != "")
            {
                itemText.text = currentItem;
            }
            else
            {
                itemText.text = "Wala";
            }
            
        }


    }

    public void AddMoney(float amount)
    {
        currentBarya += amount;
        deliveryPhase++; 
        UpdateGameplayUI();
    }

    public void ShowInteractPrompt(bool isVisible)
{
    if (interactPromptUI != null)
    {
        interactPromptUI.SetActive(isVisible);
    }
}
}