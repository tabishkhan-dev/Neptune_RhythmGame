using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour
{
    [Header("References")]
    public AudioManager audioManager;
    public FeedbackUI feedbackUI;
   

    private int perfectCount = 0;
    private int goodCount = 0;
    private int missCount = 0;
    private int totalScore = 0;

    private bool songEnded = false;
    private int lastBeatIndex = -1;

    void OnEnable() => AudioManager.OnMusicEnded += HandleMusicEnded;
    void OnDisable() => AudioManager.OnMusicEnded -= HandleMusicEnded;

    void Update()
    {
        if (audioManager == null || songEnded)
            return;

        if (Input.GetMouseButtonDown(0))
            CheckTiming();
    }

    void CheckTiming()
    {
        if (songEnded) return;

        AudioSource src = audioManager.GetComponent<AudioSource>();
        if (!src.isPlaying) return;

        float songPosition = audioManager.GetAccurateSongTime();
        float beatInterval = 60f / audioManager.BPM;

        int currentBeatIndex = Mathf.FloorToInt(songPosition / beatInterval);
        if (currentBeatIndex == lastBeatIndex)
            return;

        lastBeatIndex = currentBeatIndex;

        float nearestBeatTime = Mathf.Round(songPosition / beatInterval) * beatInterval;
        float delta = Mathf.Abs(songPosition - nearestBeatTime);

        string feedback;
        int points;

        // Determine accuracy
        if (delta <= 0.07f)
        {
            feedback = "Perfect!";
            perfectCount++;
            points = 100;
        }
        else if (delta <= 0.15f)
        {
            feedback = "Good!";
            goodCount++;
            points = 50;
        }
        else
        {
            feedback = "Miss!";
            missCount++;
            points = 0;
        }

        totalScore += points;

        // Display running count (e.g., "3 Perfect!")
        int currentCount =
            feedback == "Perfect!" ? perfectCount :
            feedback == "Good!" ? goodCount :
            missCount;

        string countedFeedback = $"{currentCount} {feedback}";

        // Update UI feedback
        feedbackUI?.ShowFeedback(countedFeedback);

    }

    // Triggered when AudioManager signals the music has ended
    private void HandleMusicEnded()
    {
        if (songEnded) return;
        songEnded = true;

        StartCoroutine(DelayedFinalScore());
    }

    private IEnumerator DelayedFinalScore()
    {
        // Short delay before showing final results
        yield return new WaitForSeconds(3f);
        ShowFinalScore();
    }

    public void ShowFinalScore()
    {
        Debug.Log("Game Over — starting count-up animation.");

        // Smoothly count up and show final score
        StartCoroutine(feedbackUI.CountUpFinalScore(
            perfectCount,
            goodCount,
            missCount,
            totalScore
        ));
    }
}
