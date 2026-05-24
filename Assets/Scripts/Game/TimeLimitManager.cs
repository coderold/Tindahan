using UnityEngine;
using TMPro; 

public class TimeLimitManager : MonoBehaviour
{
    [Header("Scene Transition Settings")]
    [SerializeField] private string lossSceneName = "Loss";
    
    [Header("UI Canvas Elements")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Timer Settings")]
    [SerializeField] private float timeRemaining = 240f; 

    private bool isTimerRunning = true;

    [System.Obsolete]
    private void Update()
    {
        if (!isTimerRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            timeRemaining = 0;
            TriggerLossTransition();
        }
    }

    private void UpdateTimerDisplay()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (timeRemaining <= 30f)
        {
            timerText.color = Color.red;
        }
    }

    [System.Obsolete]
    private void TriggerLossTransition()
    {
        isTimerRunning = false;
        Debug.Log("Ubos na ang oras! Calling SceneTransitionManager...");

        SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();

        if (transitionManager != null)
        {

            transitionManager.TransitionToScene(lossSceneName);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(lossSceneName);
        }
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }
}