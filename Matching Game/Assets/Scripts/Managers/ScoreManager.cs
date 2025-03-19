using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private TextMeshProUGUI scoreText; // Reference to the UI Text element displaying the score
    private int currentScore = 0; // Current score

    protected override void Awake()
    {
        base.Awake();
        UpdateScoreText(); // Initialize the score text
    }


    // Increase the score by 5 for each popped tile
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