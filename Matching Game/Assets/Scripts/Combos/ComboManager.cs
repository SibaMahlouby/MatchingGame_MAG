using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ComboManager : Singleton<ComboManager>
{
    private Dictionary<ComboType, ComboEffect> comboEffects;
    private List<Cell> matchedCells;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Init()
    {
        // Initialize the combo effects dictionary (empty for now)
        comboEffects = new Dictionary<ComboType, ComboEffect>();
    }

    public ComboType GetComboType(Cell cell)
    {
        // No combo detection logic
        return ComboType.None;
    }

    public async void TryExecute(Cell cell)
    {
        // No combo detection, so just execute the tile's default behavior
        cell.tile.TryExecute();

        // Decrease the number of moves
        _ = MovesManager.Instance.DecreaseMovesAsync();
    }
}