using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSequenceManager : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("The parent GameObject containing both text elements.")]
    [SerializeField] private GameObject warningTextGroup; 

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;     
    [SerializeField] private AudioClip playCallSFX;       

    [Header("Timing Settings")]
    [SerializeField] private float warningDuration = 2f;  

    [Header("Scene Transition")]
    [SerializeField] private string nextSceneName = "BarangayMapConvo"; 

    private void Start()
    {
        // Ensure the entire parent group is visible at the start
        if (warningTextGroup != null)
        {
            warningTextGroup.SetActive(true);
        }

        // Start the timed sequence
        StartCoroutine(RunIntroSequence());
    }

    private IEnumerator RunIntroSequence()
    {
        // 1. Wait for the 2-second trigger warning duration
        yield return new WaitForSeconds(warningDuration);

        // 2. Hide the entire parent group (both texts turn off simultaneously)
        if (warningTextGroup != null)
        {
            warningTextGroup.SetActive(false);
        }

        // 3. Play the audio effect
        if (audioSource != null && playCallSFX != null)
        {
            audioSource.PlayOneShot(playCallSFX);
            
            // 4. Wait precisely until the audio clip finishes playing (approx 2 seconds)
            yield return new WaitForSeconds(playCallSFX.length);
        }
        else
        {
            // Fallback safety delay if audio component setup is forgotten
            yield return new WaitForSeconds(2f);
        }

        // 5. Change scene to the Barangay map dialogue scene
        SceneManager.LoadScene(nextSceneName);
    }
}