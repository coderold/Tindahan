using UnityEngine;

public class WinCollision : MonoBehaviour
{
    private bool playerIsClose = false;
    InventoryManager inv = InventoryManager.Instance;

    [System.Obsolete]
    private void Update()
    {
        InventoryManager inv = InventoryManager.Instance;
        if (inv == null) return;

        if (playerIsClose && inv.currentBarya >= inv.targetBarya)
        {
            inv.ShowInteractPrompt(false);

            SceneTransitionManager transitioner = FindObjectOfType<SceneTransitionManager>();
            if (transitioner != null)
            {
                transitioner.TransitionToScene("Win");
            }
            else
            {
                Debug.LogError("SceneTransitionManager was not found in the scene!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
        }
    }
}
