//Manages the player's moves in the game, updates UI, and triggers an event when moves are 0.

using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;


public class MovesManager : Singleton<MovesManager>
{
    [SerializeField] private TextMeshProUGUI movesText;
    private int moves;
    public int Moves => moves;
    public Action OnMovesFinished;

    //Initializes the moves counter with a given value and updates the UI.
    public void Init(int moves)
    {
        this.moves = moves;
        movesText.text = moves.ToString();
    }

    //Decreases the move count asynchronously
    public async Task DecreaseMovesAsync()
    {
        moves--;

        if (moves <= 0)
        {
            // Disable player input to prevent further interactions
            this.GetComponent<TouchManager>().enabled = false; // discard inputs
            moves = 0;
            movesText.text = moves.ToString();
            await Task.Delay(TimeSpan.FromSeconds(1));
            OnMovesFinished?.Invoke();
        }

        movesText.text = moves.ToString();
    }
}