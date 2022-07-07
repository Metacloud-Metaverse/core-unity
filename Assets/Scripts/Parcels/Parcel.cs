using System;
using System.Collections;
using System.Collections.Generic;
using GLTFast;
using Network;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Parcel : MonoBehaviour
{
    public ParcelInfo parcelInfo;
    public bool ready;
    
    //
    public Scene loadedScene;
    
    
    public void Initialize(ParcelInfo newParcelInfo)
    {
        parcelInfo = newParcelInfo;
        ready = false;

        if (newParcelInfo.parcelType != ParcelType.defaultType)
        {
            Destroy(transform.GetChild(0).gameObject);
        }

        if (newParcelInfo.parcelType == ParcelType.street)
        {
            ParcelManager.Instance.InstantiateParcelPrefab(ParcelManager.Instance.streetPrefab, newParcelInfo.Location);
        }
        else if (newParcelInfo.parcelType == ParcelType.streetRotated)
        {
            var streetRotated = ParcelManager.Instance.InstantiateParcelPrefab(ParcelManager.Instance.streetPrefab, newParcelInfo.Location);
            var newPos = streetRotated.transform.position;
            streetRotated.transform.position = new Vector3(newPos.x, newPos.y, newPos.z + ParcelManager.Instance.parcelSize);
            streetRotated.transform.Rotate(0, 90, 0); //= new Quaternion(0,1,0,0);
        }

        if (newParcelInfo.pendingObjects != null)
        {
            foreach (var parcelObjectInfo in newParcelInfo.pendingObjects)
            {
                DownloadAndInstantiateGltf(parcelObjectInfo);    
            }
        }

        if (IsParcelTypeWorldScene(newParcelInfo.parcelType) && CustomNetworkManager.IsServer == false)
        {
            StartCoroutine(SceneLoadManager.Instance.LoadScene(SceneLoadManager.GetScenePath(newParcelInfo.parcelType), LoadSceneMode.Additive, newParcelInfo.Location, this));
        }
    }

    public void ShutDown()
    {
        if (IsParcelTypeWorldScene(parcelInfo.parcelType) && CustomNetworkManager.IsServer == false)
        {
            SceneManager.UnloadSceneAsync(loadedScene);
        }
    }

    //TODO: move into util? - same as AvatarSystem.cs
    public async void DownloadAndInstantiateGltf(ParcelObjectInfo parcelObjectInfo)
    {
        var dummy = new GameObject(); //TODO: pool this
        var gltf = new GltfImport();
        var success = await gltf.Load(parcelObjectInfo.url);
        if (success)
        {
            if (this == null)
            {
                //parcel was unloaded before finishing loading the gltf
                Destroy(dummy);
                return;
            }
            gltf.InstantiateMainScene(dummy.transform);
            var instance = dummy.transform.GetChild(0);
            instance.SetParent(transform);
            Destroy(dummy);
            instance.localPosition = parcelObjectInfo.localPosition;
            instance.rotation = parcelObjectInfo.rotation;
            instance.localScale = parcelObjectInfo.localScale;
        }
    }

    /// <summary>
    /// There's so few custom scenes in the world that this doesn't seem terrible, in the case of having way more this has to change
    /// </summary>
    public static bool IsParcelTypeWorldScene(ParcelType parcelType)
    {
        if (parcelType == ParcelType.lobbyScene || parcelType == ParcelType.artGallery || parcelType == ParcelType.poolParty || parcelType == ParcelType.marketPlace)
        {
            return true;
        }
        return false;
    }
}

[Serializable]
//Apart to serialize and send/receive easily
public struct ParcelInfo
{
    public Vector2Int Location;
    public Vector2 Size;
    public List<ParcelObjectInfo> pendingObjects;
    public ParcelType parcelType;
    //for structures that occupy more than one parcel in size
    public ParcelPattern parcelPattern;
    public Vector2Int parent;
    public List<Vector2Int> children;
    
}

public struct ParcelObjectInfo
{
    public string url;
    public Vector3 localPosition;
    public Quaternion rotation;
    public Vector3 localScale;
}


/// <summary>
/// To set if the parcel should load alone or is the parent/children of a pattern of multiple parcels (ex: scene)
/// </summary>
public enum ParcelPattern
{
    alone,
    children,
    parent
}


//TODO: enum is out of scope being used to both load scenes and parcels
public enum ParcelType
{
    defaultType, //nothing at all
    nothing,
    grass,
    street,
    occupied, //made by a player owning the parcel
    worldMain,
    lobbyScene,
    casino,
    artGallery,
    comedyClub,
    roulette,  
    house,
    poker,
    poolParty,
    marketPlace,
    streetRotated
}