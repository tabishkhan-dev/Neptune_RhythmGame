using UnityEngine;
using TMPro;
using System.Collections;

public class FeedbackUI : MonoBehaviour
{
    [Header("UI Settings")]
    public TextMeshProUGUI feedbackText;

    [Header("Display Durations")]
    public float displayTime = 1.0f; // How long Perfect/Good/Miss stays

    [Header("Feedback Colors")]
    public Color perfectColor = Color.green;
    public Color goodColor = Color.yellow;
    public Color missColor = Color.red;
    public Color finalColor = new Color(0.8f, 0.8f, 1f);

    private Coroutine currentRoutine;
    private bool finalDisplayed = false;

    private void Start()
    {
        // Keep object active but hide text visually
        if (feedbackText != null)
            feedbackText.enabled = false;
    }

    /// <summary>
    /// Shows feedback during gameplay or the final score at the end.
    /// </summary>
    public void ShowFeedback(string message, bool isFinal = false)
    {
        if (feedbackText == null) return;

        // Stop any currently running hide coroutine
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        // Always enable text when showing
        feedbackText.enabled = true;
        feedbackText.text = message;
        feedbackText.alignment = TextAlignmentOptions.Center;

        if (isFinal)
        {
            finalDisplayed = true;
            feedbackText.color = finalColor;
            feedbackText.fontSize = 50;
        }
        else
        {
            feedbackText.color = GetColor(message);
            feedbackText.fontSize = 60;
            currentRoutine = StartCoroutine(HideAfterSeconds(displayTime));
        }
    }

    private IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        // Only hide if not showing final score
        if (!finalDisplayed && feedbackText != null)
            feedbackText.enabled = false;
    }

    private Color GetColor(string message)
    {
        if (message.Contains("Perfect")) return perfectColor;
        if (message.Contains("Good")) return goodColor;
        if (message.Contains("Miss")) return missColor;
        return Color.white;
    }

    /// <summary>
    /// Smoothly counts up the final score while keeping Perfect/Good/Miss static.
    /// </summary>
    public IEnumerator CountUpFinalScore(int perfect, int good, int miss, int finalScore)
    {
        if (feedbackText == null) yield break;

        feedbackText.enabled = true;
        feedbackText.alignment = TextAlignmentOptions.Center;
        feedbackText.color = finalColor;
        feedbackText.fontSize = 50;
        finalDisplayed = true;

        // Move text to the center when the final score appears
        RectTransform rt = feedbackText.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;
        }

        // Fade in smoothly
        CanvasGroup cg = feedbackText.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = feedbackText.gameObject.AddComponent<CanvasGroup>();
            cg.alpha = 0;
        }

        float fadeDuration = 0.6f;
        float fadeElapsed = 0f;

        int current = 0;
        float duration = 1.5f; // Total countdown time
        float elapsed = 0f;

        // Play success sound once when countdown starts
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null) audio.Play();

        // Fade and count-up combined
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fadeElapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsed / duration);
            current = Mathf.RoundToInt(Mathf.Lerp(0, finalScore, progress));

            // Fade-in alpha
            if (cg != null)
                cg.alpha = Mathf.Clamp01(fadeElapsed / fadeDuration);

            feedbackText.text =
                "Perfect: " + perfect + "\n" +
                "Good: " + good + "\n" +
                "Miss: " + miss + "\n" +
                "Final Score: " + current;

            yield return null;
        }

        // Final state
        if (cg != null) cg.alpha = 1f;
        feedbackText.text =
            "Perfect: " + perfect + "\n" +
            "Good: " + good + "\n" +
            "Miss: " + miss + "\n" +
            "Final Score: " + finalScore;
    }

    /// <summary>
    /// Hides the final score manually (for restart or new song).
    /// </summary>
    public void HideFinal()
    {
        finalDisplayed = false;
        if (feedbackText != null)
            feedbackText.enabled = false;
    }
}
