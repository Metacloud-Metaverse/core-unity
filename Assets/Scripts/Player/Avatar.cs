using System.Collections.Generic;
using UnityEngine;
using APISystem;

public class Avatar : MonoBehaviour
{
    private GameObject _model;
    public GameObject model
    {
        get
        {
            if (_model == null)
            {
                if (transform.childCount > 0)
                    _model = transform.GetChild(0).gameObject;

                if (_avatarAPIHandler != null)
                    _avatarAPIHandler.avatar = _model;

            }
            return _model;
        }
    }
    private AvatarAPIHandler _avatarAPIHandler;
    private AvatarInfo _avatarInfo;
    public AvatarInfo avatarInfo { get { return _avatarInfo; } }
    private List<APIConnection.ConnectionCallback> _saveAvatarCallbacks = new List<APIConnection.ConnectionCallback>();
    private List<APIConnection.ConnectionCallback> _loadAvatarCallbacks = new List<APIConnection.ConnectionCallback>();

    private void Awake()
    {
        if(transform.childCount > 0)
            _model = transform.GetChild(0).gameObject;
        if (Client.Instance != null)
        {
            _avatarAPIHandler = new AvatarAPIHandler(model, Client.Instance.id, this);
            AddLoadAvatarCallback(SetAvatarInfo);
        }
    }

    private void Start()
    {
        Load();
    }

    public void SetModel(GameObject model)
    {
        _model = model;
        _avatarAPIHandler.avatar = model;
    }


    public void LoadAvatarInfo(AvatarInfo avatarInfo)
    {
        _avatarInfo = avatarInfo;
    }

    public void AddLoadAvatarCallback(APIConnection.ConnectionCallback callback)
    {
        _loadAvatarCallbacks.Add(callback);
    }

    public void AddSaveAvatarCallback(APIConnection.ConnectionCallback callback)
    {
        _saveAvatarCallbacks.Add(callback);
    }


    public void Save()
    {
        _avatarAPIHandler.SaveAvatar(_avatarInfo, SaveCallback);
    }

    public void Load()
    {
        if (Client.Instance != null)
            _avatarAPIHandler.LoadAvatar(LoadCallback);
    }

    private void SaveCallback(string response)
    {
        foreach (var callback in _saveAvatarCallbacks)
        {
            callback(response);
        }
    }

    private void LoadCallback(string response)
    {
        foreach (var callback in _loadAvatarCallbacks)
        {
            callback(response);
        }
    }

    private void SetAvatarInfo(string json)
    {
        var response = JsonUtility.FromJson<LoadAvatarData>(json);
        if (response.statusCode != "SUCCESS") return;
        _avatarInfo = response.data.data;
    }
}
