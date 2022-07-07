using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using GLTFast;
//using Siccity.GLTFUtility;

public class ModelLoader : MonoBehaviour
{


    //GameObject wrapper;
    //string filePath;
    public delegate void DownloadGLTFCallback(GameObject instance);

    //private void Start()
    //{
    //    filePath = $"{Application.persistentDataPath}/Files/";
    //    wrapper = new GameObject
    //    {
    //        name = "Model"
    //    };
    //}
    //public GameObject lastModel;

    //public void DownloadFile(string url)
    //{
    //    string path = GetFilePath(url);
    //    if (File.Exists(path))
    //    {
    //        Debug.Log("Found file locally, loading...");
    //        lastModel = LoadModel(path);
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
    //            lastModel = LoadModel(path);
    //        }
    //    }));
    //}

    public async void DownloadAndInstantiateGLTF(string url, DownloadGLTFCallback callback)
    {
        var gltf = new GltfImport();
        bool success = await gltf.Load(url);
        if (success)
        {
            var parent = new GameObject();
            gltf.InstantiateMainScene(parent.transform);
            var instance = parent.transform.Find("Scene");
            instance.parent = null;
            Destroy(parent);
            callback(instance.gameObject);
        }

    }
    //public void DownloadFile(string url, DownloadCallback callback)
    //{
    //    string path = GetFilePath(url);
    //    if (File.Exists(path))
    //    {
    //        Debug.Log("Found file locally, loading...");
    //        lastModel = LoadModel(path);
    //        callback();
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
    //            lastModel = LoadModel(path);
    //            callback();
    //        }
    //    }));
    //}


    //string GetFilePath(string url)
    //{
    //    string[] pieces = url.Split('/');
    //    string filename = pieces[pieces.Length - 1];

    //    return $"{filePath}{filename}";
    //}

    //public GameObject LoadModel(string path)
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
    //    return model;
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

    //public Texture2D lastTexture;
    //IEnumerator GetTexture(string url, DownloadCallback callback)
    //{
    //    UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
    //    yield return www.SendWebRequest();

    //    if (www.result != UnityWebRequest.Result.Success)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        lastTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
    //        callback();
    //    }
    //}


    //public void DownloadTexture(string url, DownloadCallback callback)
    //{
    //    StartCoroutine(GetTexture(url, callback));
    //}
}