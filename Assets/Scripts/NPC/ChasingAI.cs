using UnityEngine;
using UnityEngine.AI;

public class ChasingAI : MonoBehaviour
{
    private Transform playerTransform;
    private Transform playerCameraTransform;
    private NavMeshAgent agent;
    private Animator animator;
    private MonoBehaviour mouseLookScript; 
    private CharacterController playerController;

    [Header("Camera Look Settings")]
    [SerializeField] private float lookSpeed = 15f; 
    [SerializeField] private Vector3 targetOffset = new Vector3(0f, 0.6f, 0f); 

    [Header("Intro Pull Settings")]
    [SerializeField] private bool pullPlayerForward = true; 
    [SerializeField] private float pullSpeed = 2f;

    [Header("Kill Camera Shake Settings")]
    [SerializeField] private bool shakeCameraOnKill = true;
    [SerializeField] private float shakeIntensity = 0.45f; 
    [SerializeField] private float shakeSpeed = 55f;       
    private Vector3 originalCamLocalPosition;
    private float shakeTimer = 0f;

    [Header("Animation Parameter Names")]
    [SerializeField] private string runTriggerName = "Run";
    [SerializeField] private string killTriggerName = "Kill";

    [Header("Gameplay Settings")]
    [SerializeField] private float stopDistance = 3.5f; 
    [SerializeField] private float movementSpeed = 4.5f; 
    [SerializeField] private string gameOverSceneName = "Loss";

    private bool isIntroActive = true;
    private bool isKillingActive = false;
    private bool hasTriggeredKill = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent != null)
        {
            agent.stoppingDistance = stopDistance;
            agent.speed = movementSpeed;
            agent.isStopped = true; 
            agent.updatePosition = true; 
            agent.updateRotation = true; 
        }

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            playerController = playerObj.GetComponent<CharacterController>();
            
            Camera mainCam = playerObj.GetComponentInChildren<Camera>();
            if (mainCam != null) 
            {
                playerCameraTransform = mainCam.transform;
                originalCamLocalPosition = playerCameraTransform.localPosition; 
                
                mouseLookScript = playerObj.GetComponent<MonoBehaviour>(); 
                if (mouseLookScript == null) mouseLookScript = mainCam.GetComponent<MonoBehaviour>();
                
                if (mouseLookScript != null) mouseLookScript.enabled = false; 
            }
        }
        else
        {
            Debug.LogError("[ChasingAI] Player object with tag 'Player' not found!");
        }

        Invoke(nameof(StartChasingPlayer), 3f);
    }

    private void Update()
    {
        if (playerTransform == null) return;

        if (isIntroActive)
        {
            ExecuteForcedLookAt();
            ExecuteIntroPull();
            return; 
        }

        if (isKillingActive)
        {
            ExecuteForcedLookAt();
            
            if (shakeCameraOnKill && playerCameraTransform != null)
            {
                shakeTimer += Time.deltaTime * shakeSpeed;
                float offsetX = Mathf.Sin(shakeTimer) * shakeIntensity;
                float offsetY = Mathf.Cos(shakeTimer * 1.5f) * shakeIntensity;
                playerCameraTransform.localPosition = originalCamLocalPosition + new Vector3(offsetX, offsetY, 0f);
            }
            return;
        }

        float currentDistance = Vector3.Distance(transform.position, playerTransform.position);

        if (currentDistance <= stopDistance) 
        {
            if (!hasTriggeredKill)
            {
                TriggerKillSequence();
            }
        } 
        else 
        {
            if (agent != null && agent.isActiveAndEnabled)
            {
                agent.isStopped = false;
                agent.SetDestination(playerTransform.position);

                if (agent.hasPath && agent.velocity.sqrMagnitude < 0.1f)
                {
                    Vector3 targetDir = (agent.steeringTarget - transform.position).normalized;
                    targetDir.y = 0; 
                    transform.position += targetDir * movementSpeed * Time.deltaTime;
                }
            }
        }
    }

    private void ExecuteForcedLookAt()
    {
        if (playerCameraTransform != null)
        {
            Vector3 facePosition = transform.position + targetOffset;
            Vector3 directionToScare = facePosition - playerCameraTransform.position;
            
            if (directionToScare != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToScare);
                playerCameraTransform.rotation = Quaternion.Slerp(playerCameraTransform.rotation, targetRotation, lookSpeed * Time.deltaTime);
            }
        }
    }
    private void ExecuteIntroPull()
    {
        if (pullPlayerForward && playerController != null)
        {
            float distanceToEntity = Vector3.Distance(playerController.transform.position, transform.position);

            if (distanceToEntity > stopDistance)
            {
                Vector3 pullDirection = transform.position - playerController.transform.position;
                pullDirection.y = 0f; 
                pullDirection.Normalize();

                playerController.Move(pullDirection * pullSpeed * Time.deltaTime);
            }
        }
    }

    private void StartChasingPlayer()
    {
        isIntroActive = false;
        if (mouseLookScript != null) mouseLookScript.enabled = true; 
        
        if (animator != null) 
        {
            animator.SetTrigger(runTriggerName);
        }

        if (agent != null) 
        {
            agent.isStopped = false;
            agent.speed = movementSpeed;
        }
    }

    private void TriggerKillSequence()
    {
        hasTriggeredKill = true;
        isKillingActive = true;

        if (mouseLookScript != null) mouseLookScript.enabled = false;

        if (agent != null) 
        {
            agent.velocity = Vector3.zero; 
            agent.isStopped = true;
            agent.enabled = false; 
        }

        if (playerController != null) playerController.enabled = false;

        Rigidbody playerRb = playerTransform.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerRb.isKinematic = true;
            playerRb.linearVelocity = Vector3.zero; 
        }

        if (animator != null)
        {
            animator.SetTrigger(killTriggerName);
        }

        Invoke(nameof(LoadGameOverScene), 2.5f);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SafeZone") && !isKillingActive)
        {
            DespawnChaser();
        }
    }

    public void DespawnChaser()
    {
        if (mouseLookScript != null) mouseLookScript.enabled = true;
        Destroy(gameObject);
    }

    private void LoadGameOverScene()
    {
        if (InventoryManager.Instance != null) InventoryManager.Instance.ShowInteractPrompt(false);

        SceneTransitionManager transitioner = FindFirstObjectByType<SceneTransitionManager>();
        if (transitioner != null) transitioner.TransitionToScene(gameOverSceneName);
        else UnityEngine.SceneManagement.SceneManager.LoadScene(gameOverSceneName);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}