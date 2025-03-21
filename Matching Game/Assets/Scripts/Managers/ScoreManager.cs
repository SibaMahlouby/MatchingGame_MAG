// Manages the player's score, updates the score UI, and allows resetting the score.
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int currentScore = 0;

    protected override void Awake()
    {
        base.Awake();
        UpdateScoreText(); 
    }


    // Increase the score for each popped tile
    public void IncreaseScore(int score)
    {
        currentScore += score;
        UpdateScoreText();
    }

    // Update the score text UI
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
        }
    }

    // Reset the score
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreText();
    }
}