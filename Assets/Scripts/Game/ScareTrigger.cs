using UnityEngine;

public class ScareTrigger : MonoBehaviour
{
    public enum ConditionType { None, ItemOnly, PhaseOnly, Both }

    [Header("Flexibility Settings")]
    [SerializeField] private ConditionType conditionToCheck = ConditionType.None;
    [SerializeField] private string requiredItem = ""; 
    [SerializeField] private int requiredPhase = 1;

    [Header("Jumpscare Elements")]
    [SerializeField] private GameObject jumpscareObject; 
    [SerializeField] private AudioSource audioSource;     
    [SerializeField] private AudioClip jumpscareSFX;     

    [Header("Camera Look Settings")]
    [SerializeField] private bool forceCameraFocus = true; 
    [SerializeField] private float lookSpeed = 15f; 
    [SerializeField] private Vector3 targetOffset = new Vector3(0f, 1.5f, 0f);
    
    private Transform playerCameraTransform;
    private MonoBehaviour mouseLookScript; 
    private CharacterController playerController; 

    [Header("Vacuum Pull Settings")]
    [SerializeField] private bool pullPlayerForward = true;
    [SerializeField] private float pullSpeed = 4f; 
    [SerializeField] private float stopDistance = 1.8f; 

    [Header("Settings")]
    [SerializeField] private float activeDuration = 2.0f; 
    [SerializeField] private string playerTag = "Player";

    private bool hasTriggered = false;
    private bool isFocusingCamera = false;

    private void Start()
    {
        if (jumpscareObject != null)
        {
            jumpscareObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isFocusingCamera && jumpscareObject != null)
        {
            Vector3 facePosition = jumpscareObject.transform.position + targetOffset;

            if (playerCameraTransform != null)
            {
                Vector3 directionToScare = facePosition - playerCameraTransform.position;
                Quaternion targetRotation = Quaternion.LookRotation(directionToScare);
                playerCameraTransform.rotation = Quaternion.Slerp(playerCameraTransform.rotation, targetRotation, lookSpeed * Time.deltaTime);
            }

            if (pullPlayerForward && playerController != null)
            {
                float distanceToEntity = Vector3.Distance(playerController.transform.position, jumpscareObject.transform.position);

                if (distanceToEntity > stopDistance)
                {
                    Vector3 pullDirection = jumpscareObject.transform.position - playerController.transform.position;
                    pullDirection.y = 0f; 
                    pullDirection.Normalize();

                    playerController.Move(pullDirection * pullSpeed * Time.deltaTime);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && !hasTriggered)
        {
            if (CheckConditionsPass())
            {
                playerController = other.GetComponent<CharacterController>();

                if (forceCameraFocus)
                {
                    Camera mainCam = other.GetComponentInChildren<Camera>();
                    if (mainCam != null)
                    {
                        playerCameraTransform = mainCam.transform;
                        mouseLookScript = other.GetComponent<MonoBehaviour>(); 
                        if (mouseLookScript == null) mouseLookScript = mainCam.GetComponent<MonoBehaviour>();
                    }
                }

                TriggerJumpscare();
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
            case ConditionType.ItemOnly:
                return itemMatches;

            case ConditionType.PhaseOnly:
                return phaseMatches;

            case ConditionType.Both:
                return itemMatches && phaseMatches;

            case ConditionType.None:
            default:
                return true; 
        }
    }

    private void TriggerJumpscare()
    {
        hasTriggered = true;

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        if (jumpscareObject != null)
        {
            jumpscareObject.SetActive(true);
        }

        if (forceCameraFocus && mouseLookScript != null)
        {
            mouseLookScript.enabled = false; 
            isFocusingCamera = true;
        }

        float soundDuration = 0f;
        if (audioSource != null && jumpscareSFX != null)
        {
            audioSource.PlayOneShot(jumpscareSFX);
            soundDuration = jumpscareSFX.length;
        }

        Invoke(nameof(HideJumpscare), activeDuration);

        float totalLifeTime = Mathf.Max(activeDuration, soundDuration);
        Destroy(gameObject, totalLifeTime);
    }

    private void HideJumpscare()
    {
        isFocusingCamera = false;
        
        if (mouseLookScript != null)
        {
            mouseLookScript.enabled = true;
        }

        if (jumpscareObject != null)
        {
            jumpscareObject.SetActive(false);
        }
    }
}