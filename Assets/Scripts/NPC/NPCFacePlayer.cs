using UnityEngine;

public class NPCFacePlayer : MonoBehaviour
{
    [Header("Tracking Settings")]
    public Transform playerTransform; 
    public float rotationSpeed = 5f; 

    private void Start()
    {
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null) 
            {
                playerTransform = playerObj.transform;
            }
        }
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            Vector3 targetDirection = playerTransform.position - transform.position;
            
            targetDirection.y = 0f; 

            if (targetDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}