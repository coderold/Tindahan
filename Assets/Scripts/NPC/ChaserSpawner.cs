using UnityEngine;

public class ChaserSpawner : MonoBehaviour
{
    public enum ConditionType { None, ItemOnly, PhaseOnly, Both }

    [Header("Flexibility Settings")]
    [SerializeField] private ConditionType conditionToCheck = ConditionType.None;
    [SerializeField] private string requiredItem = ""; 
    [SerializeField] private int requiredPhase = 1;

    [Header("Spawn Settings")]
    [SerializeField] private GameObject chaserPrefab; 
    [SerializeField] private Transform spawnPoint; 

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            if (CheckConditionsPass())
            {
                hasTriggered = true;
                
                if (chaserPrefab != null && spawnPoint != null)
                {
                    Instantiate(chaserPrefab, spawnPoint.position, spawnPoint.rotation);
                }

                Destroy(gameObject);
            }
        }
    }

    private bool CheckConditionsPass()
    {
        InventoryManager inv = InventoryManager.Instance;
        if (inv == null) return false;

        bool itemMatches = inv.currentItem == requiredItem;
        bool phaseMatches = inv.deliveryPhase == requiredPhase;

        switch (conditionToCheck)
        {
            case ConditionType.ItemOnly: return itemMatches;
            case ConditionType.PhaseOnly: return phaseMatches;
            case ConditionType.Both: return itemMatches && phaseMatches;
            case ConditionType.None:
            default: return true; 
        }
    }
}