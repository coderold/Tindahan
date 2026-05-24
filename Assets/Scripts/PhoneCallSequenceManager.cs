using System.Collections;
using UnityEngine;

public class PhoneCallSequenceManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;    
    [SerializeField] private AudioClip phoneConversation; 

    [Header("Scene Transition Custom Reference")]
    [SerializeField] private SceneTransitionManager transitionManager;
    [SerializeField] private string mainGameplaySceneName = "MainGameLoop";

    private void Start()
    {
        StartCoroutine(PlayCallAndTransition());
    }

    private IEnumerator PlayCallAndTransition()
    {
        if (audioSource != null && phoneConversation != null)
        {
            audioSource.clip = phoneConversation;
            audioSource.Play();

            yield return new WaitForSeconds(phoneConversation.length);
        }
        else
        {
            Debug.LogWarning("Missing AudioSource or PhoneConversation clip! Skipping audio delay.");
            yield return new WaitForSeconds(3f);
        }

        if (transitionManager != null)
        {
            transitionManager.TransitionToScene(mainGameplaySceneName);
        }
        else
        {
            Debug.LogError("SceneTransitionManager reference is missing on this GameObject! Instantly breaking scene.");
            UnityEngine.SceneManagement.SceneManager.LoadScene(mainGameplaySceneName);
        }
    }
}