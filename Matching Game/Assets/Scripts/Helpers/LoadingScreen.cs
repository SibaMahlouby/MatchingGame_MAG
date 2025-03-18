//Display a loading screen
using TMPro;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public TextMeshProUGUI loadingText;

    public void SetProgress(float progress)
    {
        loadingText.text = progress < 0.25f ? "Something Awesome is loading" :
                           progress < 0.50f ? "Something Awesome is loading." :
                           progress < 0.75f ? "Something Awesome is loading.." :
                                              "Something Awesome is loading...";
    }
}
