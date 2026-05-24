using UnityEngine;

public class SafeZoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the safe zone!");

            ChasingAI[] activeChasers = FindObjectsByType<ChasingAI>(FindObjectsSortMode.None);
            
            foreach (ChasingAI chaser in activeChasers)
            {
                chaser.DespawnChaser();
            }
        }
    }
}