using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
//using Siccity.GLTFUtility;

public class ModelLoader1 : MonoBehaviour
{
    //GameObject wrapper;
    //string filePath;

    //private void Start()
    //{
    //    filePath = $"{Application.persistentDataPath}/Files/";
    //    wrapper = new GameObject
    //    {
    //        name = "Model"
    //    };
    //}

    //public void DownloadFile(string url)
    //{
    //    string path = GetFilePath(url);
    //    if (File.Exists(path))
    //    {
    //        Debug.Log("Found file locally, loading...");
    //        LoadModel(path);
    //        return;
    //    }
    //    StartCoroutine(GetFileRequest(url, (UnityWebRequest req) =>
    //    {
    //        if (req.result == UnityWebRequest.Result.ConnectionError)
    //        {
    //            // Log any errors that may happen
    //            Debug.Log($"{req.error} : {req.downloadHandler.text}");
    //        }
    //        else
    //        {
    //            // Save the model into a new wrapper
    //            LoadModel(path);
    //        }
    //    }));

    //}

    //string GetFilePath(string url)
    //{
    //    string[] pieces = url.Split('/');
    //    string filename = pieces[pieces.Length - 1];

    //    return $"{filePath}{filename}";
    //}

    //public void LoadModel(string path)
    //{
    //    ResetWrapper();
    //    Debug.Log("Importing model... " + path);

    //    AnimationClip[] animations;
    //    var importSettings = new ImportSettings();
    //    importSettings.animationSettings.useLegacyClips = true;
    //    importSettings.animationSettings.interpolationMode = InterpolationMode.STEP;
    //    GameObject model = Importer.LoadFromFile(path, importSettings, out animations);

    //    Debug.Log("Import finalized");

    //    if (animations.Length != 0)
    //    {
    //        Animation anim = model.AddComponent<Animation>();
    //        anim.playAutomatically = true;

    //        foreach (var animation in animations)
    //        {
    //            anim.AddClip(animation, animation.name);
    //        }
    //        anim.Play(animations[0].name);
    //        anim.wrapMode = WrapMode.Loop;
    //    }
       
    //}

    //IEnumerator GetFileRequest(string url, Action<UnityWebRequest> callback)
    //{
    //    using (UnityWebRequest req = UnityWebRequest.Get(url))
    //    {
    //        Debug.Log(GetFilePath(url));
    //        req.downloadHandler = new DownloadHandlerFile(GetFilePath(url));
    //        yield return req.SendWebRequest();
    //        callback(req);
    //    }
    //}

    //void ResetWrapper()
    //{
    //    if (wrapper != null)
    //    {
    //        foreach (Transform trans in wrapper.transform)
    //        {
    //            Destroy(trans.gameObject);
    //        }
    //    }
    //}
}