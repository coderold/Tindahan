using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Animator faderAnimator;
    public string sceneToLoad = "Baranggay";

    public void StartGame()
    {
        StartCoroutine(FadeAndLoad());
    }

    IEnumerator FadeAndLoad()
    {
        // Trigger the animation (Ensure the parameter name matches)
        faderAnimator.SetTrigger("StartFade");

        // Wait for the animation to finish (usually 1 second)
        yield return new WaitForSeconds(1f);

        // Load the actual scene
        SceneManager.LoadScene(sceneToLoad);
    }
}