using UnityEngine;

public class TindahanCollider : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource; 
    [SerializeField] private AudioClip itemPickupSFX;

    private bool playerIsClose = false;

    private string GetItemForPhase(int phase)
    {
        switch (phase)
        {
            case 1: return "Suka";     
            case 2: return "Yelo";        
            case 3: return "Yosi";       
            case 4: return "Kape";       
            case 5: return "Coke Mismo";    
            default: return "Wala";
        }
    }

    private void Update()
    {
        InventoryManager inv = InventoryManager.Instance;
        if (inv == null) return;

        if (playerIsClose && inv.currentBarya >= inv.targetBarya)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                inv.ShowInteractPrompt(false);
                playerIsClose = false;
                Debug.Log("Laro Tapos! Naabot ang ₱50.");
            }
            return;
        }

        if (playerIsClose && Input.GetKeyDown(KeyCode.E))
        {
            if (inv.currentItem == "")
            {

                string assignedItem = GetItemForPhase(inv.deliveryPhase);
                inv.currentItem = assignedItem;
                inv.UpdateGameplayUI();
                
                if (audioSource != null && itemPickupSFX != null)
                {
                    audioSource.PlayOneShot(itemPickupSFX);
                }

                Debug.Log($"Binigay ng Tindera ang: {assignedItem} para sa Phase {inv.deliveryPhase}");

                inv.ShowInteractPrompt(false);
                playerIsClose = false; 
            }
            else
            {
                Debug.Log("May bitbit ka pa! Ihatid mo muna yung kasalukuyang utos.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager inv = InventoryManager.Instance;
            if (inv != null)
            {
                if (inv.currentItem == "" || inv.currentBarya >= inv.targetBarya)
                {
                    playerIsClose = true;
                    inv.ShowInteractPrompt(true);
                }
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