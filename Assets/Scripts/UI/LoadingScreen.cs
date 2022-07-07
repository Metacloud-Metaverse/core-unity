using System.Collections;
using System.Collections.Generic;
using System.Text;
using Network;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoadingScreen : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI percentText;
    [SerializeField]private TextMeshProUGUI loadingText;
    public Canvas canvas;
    public GameObject background;
    public GameObject logo;
    public float fadeTime = 0.5f;
    public Image loadingBar;
    public float fakeTimer;
    public float speedFill = 3;
    private readonly float fakeTimerLimit = 2;

    private Coroutine _coroutineProgress;
    public bool IsProgressFinished(AsyncOperation asyncOperation, LoadSceneMode loadSceneMode)
    {
        if (asyncOperation.isDone)
        {
            if (CustomNetworkManager.IsServer || loadSceneMode == LoadSceneMode.Additive)
            {
                return true;
            }
            if (fakeTimer > fakeTimerLimit)
            {
                return true;
            }
        }
        fakeTimer += Time.deltaTime;
     //   SetLoadingBarProgress(Mathf.Min(fakeTimer, asyncOperation.progress));
        return false;
    }
    
    
    public void ShowLoadingScreen()
    {
        canvas.enabled = true;
        iTween.FadeFrom(background, 0, fadeTime);
        iTween.FadeFrom(logo, 0, fadeTime);
    }

    public void SetLoadingBarProgress(float progress)
    {
        loadingBar.transform.localScale = new Vector3(progress, 1, 1);
    }

    public void SetProgres(int target)
    {
        if (target == 1)
        {
            StartCoroutine(LoadingAnimation());
        }
        if(gameObject.activeInHierarchy == false)
            gameObject.SetActive(true);
        if(_coroutineProgress != null)
           StopCoroutine((_coroutineProgress)); 
        _coroutineProgress = StartCoroutine(SetProgressBarCoroutine(target));
    }

    private IEnumerator LoadingAnimation()
    {
        while (loadingBar.transform.localScale.x < 1)
        {
            loadingText.text = "Loading.";
            yield return Yielders.Seconds(0.1f);
            loadingText.text = "Loading..";
            yield return Yielders.Seconds(0.1f);
            loadingText.text = "Loading...";
            yield return Yielders.Seconds(0.1f);
        }
        
        loadingText.text = "Starting Game.";
        yield return Yielders.Seconds(0.1f);
        loadingText.text = "Starting Game..";
        yield return Yielders.Seconds(0.1f);
        loadingText.text = "Starting Game...";
    }
    private IEnumerator SetProgressBarCoroutine(int target)
    {
        //3 --- 1
        //x --- ?
        float fixedProgress = target * 1 / 3;
        var targetVector = new Vector3(fixedProgress, 1, 1);
        StringBuilder sBuilder = new StringBuilder();
        while ( loadingBar.transform.localScale != targetVector)
        {
            sBuilder.Clear();
            loadingBar.transform.localScale = Vector3.MoveTowards(loadingBar.transform.localScale,targetVector,speedFill * Time.deltaTime);
            var floatPercent = loadingBar.transform.localScale.x * 100;
            sBuilder.Append(floatPercent.ToString("00.00"));
            sBuilder.Append(" %");
            percentText.text = sBuilder.ToString();
            yield return null;
        }
        if (loadingBar.transform.localScale.x == 1)
        {
            yield return Yielders.Seconds(1f);
            gameObject.SetActive(false);
        }
    }
    
    public void HideLoadingScreen()
    {
       // fakeTimer = 0;
       // canvas.enabled = false;
       // SetLoadingBarProgress(0);
    }
    
}
