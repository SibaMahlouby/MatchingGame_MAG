//This is the base to create special combos’ effects (TNTs) it is not yet completed
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ComboManager : Singleton<ComboManager>
{
    private List<Cell> matchedCells;

    protected override void Awake()
    {
        base.Awake();
    }

  

    public async void TryExecute(Cell cell)
    {
        // No combo detection, so just execute the tile's default behavior
        cell.tile.TryExecute();

        // Decrease the number of moves
        _ = MovesManager.Instance.DecreaseMovesAsync();
    }
}