using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstScreenManager : MonoBehaviourSingleton<FirstScreenManager>
{
    public void LoadScene(SceneIndex sceneIndexIndex)
    {
        StartCoroutine(LoadSceneCoroutine(sceneIndexIndex));
    }

    public void SkipFirstScreen()
    {
        LoadScene(SceneIndex.WORLD);
    }

    private IEnumerator LoadSceneCoroutine(SceneIndex sceneIndexIndex)
    {
        UIManager.Instance.loadingScreen.ShowLoadingScreen();
        var asyncOperation = SceneManager.LoadSceneAsync((int)sceneIndexIndex);
        while (asyncOperation.isDone == false)
        {
            UIManager.Instance.loadingScreen.SetLoadingBarProgress(asyncOperation.progress);
            yield return null;
        }
        UIManager.Instance.loadingScreen.HideLoadingScreen();
        if (sceneIndexIndex == SceneIndex.WORLD)
        {
            StartMenu.Instance.ToggleMenu(true);
        }
    }

}
