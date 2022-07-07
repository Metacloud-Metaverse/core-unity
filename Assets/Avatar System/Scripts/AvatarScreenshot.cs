using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

public class AvatarScreenshot : MonoBehaviour
{
    private Camera _cam;

    public int screenshotWidth;
    public int screenshotHeight;
    private bool _takeScreenShot;

    private void OnEnable()
    {
        RenderPipelineManager.endCameraRendering += EndCameraRendering;
    }

    private void OnDisable()
    {
        RenderPipelineManager.endCameraRendering -= EndCameraRendering;
    }

    private void EndCameraRendering(ScriptableRenderContext arg1, Camera arg2)
    {
        if (!_takeScreenShot) return;
        screenshotHeight = Screen.width;
        screenshotHeight = Screen.height;
        _takeScreenShot = false;
        var renderResult = new Texture2D(screenshotWidth, screenshotHeight, TextureFormat.ARGB32, false);
        var rect = new Rect(0, 0, screenshotWidth, screenshotHeight);
        renderResult.ReadPixels(rect,0,0);
        renderResult.Apply();
        Debug.Log(Application.dataPath);
        Debug.Log(Application.persistentDataPath);
        var byteArray = renderResult.EncodeToPNG();
        Destroy(renderResult);
        var dateTime = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        //var dir = $"{Application.dataPath}/Screenshots/screenshot-{dateTime}.png";
        var dir = $"data:image/png;base64,screenshot-{dateTime}";
        OpenWindow(dir);
        //System.IO.File.WriteAllBytes(dir, byteArray);
        print("Saved screenshot");
    }

    private void Awake()
    {
        _cam = GetComponent<Camera>();
    }

    public void TakePicture()
    {
        _takeScreenShot = true;
    }

    [DllImport("__Internal")]
    private static extern void OpenWindow(string url);
}
