using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public Animator faderAnimator;
    public string defaultSceneToLoad = "Baranggay";

    public void StartGame()
    {
        TransitionToScene(defaultSceneToLoad);
    }

    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        if (faderAnimator != null)
        {
            faderAnimator.SetTrigger("StartFade");
        }
        else
        {
            Debug.LogWarning("Fader Animator missing! Loading scene immediately.");
        }

        yield return new WaitForSeconds(1.2f);

        SceneManager.LoadScene(sceneName);
    }
}