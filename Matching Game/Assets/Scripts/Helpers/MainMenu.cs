// Responsible for displaying the main menu of the game.

using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button levelButton;
    [SerializeField] private TextMeshProUGUI levelText;

        private void Awake()
    {
        int level = PlayerPrefs.GetInt("Level", 1);

        if (level > 3)
        {
            // Reset the level to 1
            level = 1;
            PlayerPrefs.SetInt("Level", level);
            PlayerPrefs.Save(); // Ensure the change is saved

            levelText.text = "Done!";
        }
        else
        {
            levelText.text = "Level " + level;
        }

        levelButton.onClick.RemoveAllListeners();
        levelButton.onClick.AddListener(() => GameManager.Instance.LoadLevelScene());
    }

}

