//A singleton that manages the game state

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private LoadingScreen loadingScreen;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void NextLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("Level", 1);
        PlayerPrefs.SetInt("Level", currentLevel + 1);
        LoadMainMenu();
    }

    public void LoadLevelScene()
    {
        StartCoroutine(LoadAsyncScene("LevelScene"));
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }

    IEnumerator LoadAsyncScene(string sceneName)
    {

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
