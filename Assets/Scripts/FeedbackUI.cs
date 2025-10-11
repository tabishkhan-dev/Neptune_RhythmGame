using UnityEngine;
using TMPro;
using System.Collections;

public class FeedbackUI : MonoBehaviour
{
    public TextMeshProUGUI feedbackText;   // Assign in Inspector
    public float displayTime = 1.0f;       // How long normal feedback stays
    public Color perfectColor = Color.green;
    public Color goodColor = Color.yellow;
    public Color missColor = Color.red;
    public Color finalColor = new Color(0.8f, 0.8f, 1f); // light bluish

    private Coroutine currentRoutine;

    public void ShowFeedback(string message, bool isFinal = false)
    {
        if (feedbackText == null) return;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        // 🟢 For final score: show and keep longer
        if (isFinal)
        {
            feedbackText.text = message;
            feedbackText.color = finalColor;
            feedbackText.fontSize = 50;
            feedbackText.alignment = TextAlignmentOptions.Center;
            feedbackText.enabled = true;
            return;
        }

        // 🎯 Otherwise, it's normal gameplay feedback
        feedbackText.text = message;
        feedbackText.color = GetColor(message);
        feedbackText.fontSize = 60;
        feedbackText.enabled = true;

        currentRoutine = StartCoroutine(HideAfterSeconds(displayTime));
    }

    private IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        feedbackText.enabled = false;
    }

    private Color GetColor(string message)
    {
        if (message.Contains("Perfect")) return perfectColor;
        if (message.Contains("Good")) return goodColor;
        if (message.Contains("Miss")) return missColor;
        return Color.white;
    }
}
