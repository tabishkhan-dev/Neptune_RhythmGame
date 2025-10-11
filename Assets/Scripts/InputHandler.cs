using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public AudioManager audioManager;   // Reference to AudioManager
    public FeedbackUI feedbackUI;       // Reference to Feedback UI (on-screen text)

    private int perfectCount = 0;
    private int goodCount = 0;
    private int missCount = 0;
    private int totalScore = 0;

    private bool songEnded = false;

    private void Update()
    {
        if (audioManager == null)
            return;

        AudioSource src = audioManager.GetComponent<AudioSource>();

        // Check if song has ended
        if (!songEnded && !src.isPlaying)
        {
            songEnded = true;
            ShowFinalScore();
            return;
        }

        // Handle clicks/taps
        if (Input.GetMouseButtonDown(0))
        {
            CheckTiming();
        }
    }

    void CheckTiming()
    {
        if (!audioManager.GetComponent<AudioSource>().isPlaying)
            return;

        // Latency compensation (~100ms earlier)
        float offset = -0.10f;
        float songPosition = audioManager.GetAccurateSongTime() + offset;

        float beatInterval = 60f / audioManager.BPM;
        float nearestBeatTime = Mathf.Round(songPosition / beatInterval) * beatInterval;
        float delta = Mathf.Abs(songPosition - nearestBeatTime);

        string feedback;
        int points = 0;

        if (delta <= 0.10f)
        {
            feedback = "🎯 Perfect!";
            perfectCount++;
            points = 100;
        }
        else if (delta <= 0.22f)
        {
            feedback = "👍 Good!";
            goodCount++;
            points = 50;
        }
        else
        {
            feedback = "❌ Miss!";
            missCount++;
            points = 0;
        }

        totalScore += points;

        Debug.Log(feedback);
        if (feedbackUI != null)
            feedbackUI.ShowFeedback(feedback);
    }

    void ShowFinalScore()
    {
        string result = $"🏁 Game Over!\n\n" +
                        $"🎯 Perfect: {perfectCount}\n" +
                        $"👍 Good: {goodCount}\n" +
                        $"❌ Miss: {missCount}\n\n" +
                        $"🏆 Final Score: {totalScore}";

        Debug.Log(result);

        if (feedbackUI != null)
            feedbackUI.ShowFeedback(result, true); // Show full summary
    }
}
