using UnityEngine;

public class ScareTrigger : MonoBehaviour
{
    [Header("Jumpscare Elements")]
    [SerializeField] private GameObject jumpscareObject; 
    [SerializeField] private AudioSource audioSource;     
    [SerializeField] private AudioClip jumpscareSFX;     

    [Header("Settings")]
    [SerializeField] private float activeDuration = 2.0f; 
    [SerializeField] private string playerTag = "Player";

    private bool hasTriggered = false;

    private void Start()
    {
        if (jumpscareObject != null)
        {
            jumpscareObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && !hasTriggered)
        {
            TriggerJumpscare();
        }
    }

    private void TriggerJumpscare()
    {
        hasTriggered = true;

        if (jumpscareObject != null)
        {
            jumpscareObject.SetActive(true);
        }

        if (audioSource != null && jumpscareSFX != null)
        {
            audioSource.PlayOneShot(jumpscareSFX);
        }

        Invoke(nameof(HideJumpscare), activeDuration);
    }

    private void HideJumpscare()
    {
        if (jumpscareObject != null)
        {
            jumpscareObject.SetActive(false);
        }
        
        Destroy(gameObject);
    }
}