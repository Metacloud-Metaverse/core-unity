using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TextureDownloader
{
    public delegate void DownloadCallback(Texture2D texture);

    public IEnumerator GetTextureAndRender(string url, Renderer renderer)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            var texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            renderer.material.mainTexture = texture;
        }
    }

}
