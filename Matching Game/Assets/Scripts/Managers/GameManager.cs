//Singleton that manages the game state, including level progression and scene transitions.

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private LoadingScreen loadingScreen;

    //Ensures this GameManager persists across scenes.
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    //Increments the level counter and loads the main menu.
    public void NextLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("Level", 1);
        PlayerPrefs.SetInt("Level", currentLevel + 1);
        LoadMainMenu();
    }

    //Loads the level scene asynchronously with a loading screen.
    public void LoadLevelScene()
    {
        StartCoroutine(LoadAsyncScene("LevelScene"));
    }


    //Loads the main menu scene synchronously.
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }

    //Loads a scene asynchronously while displaying a loading screen.
    IEnumerator LoadAsyncScene(string sceneName)
    {
        if (loadingScreen != null)
        {
            loadingScreen.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Loading screen is not assigned in GameManager.");
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        loadingScreen.gameObject.SetActive(true);

        while (!asyncLoad.isDone)
        {
            loadingScreen.SetProgress(asyncLoad.progress);
            yield return null;
        }

        loadingScreen.SetProgress(100);

        yield return new WaitForSeconds(0.2f);
        loadingScreen.gameObject.SetActive(false);
    }
}
