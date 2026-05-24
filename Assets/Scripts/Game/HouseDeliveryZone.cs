using UnityEngine;

public class HouseDeliveryZone : MonoBehaviour
{
    [Header("Which House Number is this?")]
    public int houseID; 
    public string itemRequired;

    [Header("Visual Indicator (Sign / Light / Arrow)")]
    public GameObject targetIndicator;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource; 
    [SerializeField] private AudioClip baryaSFX;   

    private bool playerIsClose = false;

    private void Start()
    {
        if (targetIndicator != null)
        {
            targetIndicator.SetActive(false);
        }
    }

    private void Update()
    {
        InventoryManager inv = InventoryManager.Instance;
        if (inv != null && targetIndicator != null)
        {
            if (inv.deliveryPhase == houseID && inv.currentItem == itemRequired)
            {
                targetIndicator.SetActive(true);
            }
            else
            {
                targetIndicator.SetActive(false);
            }
        }

        if (playerIsClose && Input.GetKeyDown(KeyCode.E) && inv != null)
        {
            if (inv.deliveryPhase == houseID && inv.currentItem == itemRequired)
            {
                inv.currentItem = "";
                inv.AddMoney(10f); 
                inv.ShowInteractPrompt(false);
                playerIsClose = false; 

                if (audioSource != null && baryaSFX != null)
                {
                    audioSource.PlayOneShot(baryaSFX);
                }
                
                Debug.Log($"Salamat sa paghatid ng {itemRequired} sa Bahay #{houseID}!");

                if (targetIndicator != null)
                {
                    Destroy(targetIndicator);
                }
                
                DisableZoneAndCleanUp();
            }
            else if (inv.deliveryPhase == houseID && inv.currentItem != itemRequired)
            {
                Debug.Log($"Nandito ka sa tamang bahay ngunit kulang o mali ang dala mong item! Kailangan: {itemRequired}");
            }
        }
    }

    private void DisableZoneAndCleanUp()
    {

        if (TryGetComponent<Collider>(out Collider col))
        {
            col.enabled = false;
        }
        
        if (TryGetComponent<MeshRenderer>(out MeshRenderer ren))
        {
            ren.enabled = false;
        }

        Destroy(gameObject, 2.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager inv = InventoryManager.Instance;
            
            if (inv != null && inv.deliveryPhase == houseID)
            {
                playerIsClose = true;
                inv.ShowInteractPrompt(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager inv = InventoryManager.Instance;

            if (playerIsClose)
            {
                playerIsClose = false;
                if (inv != null)
                {
                    inv.ShowInteractPrompt(false);
                }
            }
        }
    }
}