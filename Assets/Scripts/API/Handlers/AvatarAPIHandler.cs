using UnityEngine;
using APISystem;

public class AvatarAPIHandler
{
    private static AvatarAPI _api;
    private static AvatarParser _avatarParser = new AvatarParser();
    private APIConnection.ConnectionCallback _loadCallback;

    public GameObject avatar;
    private int _userId;

    public AvatarAPIHandler(GameObject avatar, int userId, MonoBehaviour invoker)
    {
        this.avatar = avatar;
        _userId = userId;
        _api = new AvatarAPI(invoker);
    }

    public void LoadAvatar(APIConnection.ConnectionCallback callback)
    {
        _loadCallback = callback;
        _api.LoadAvatar(_userId, LoadCallback);
    }

    public void SaveAvatar(AvatarInfo avatarInfo, APIConnection.ConnectionCallback callback)
    {
        _api.SaveAvatar(Client.Instance.id, avatarInfo, callback);
    }

    private void LoadCallback(string response)
    {
        SetAvatarInfo(response);
        _loadCallback(response);
    }

    private void SetAvatarInfo(string response)
    {
        var loadAvatarData = JsonUtility.FromJson<LoadAvatarData>(response);
        if(loadAvatarData.statusCode == "SUCCESS")
            _avatarParser.ApplyConfigToMesh(loadAvatarData.data.data, avatar, avatar.transform.parent);
    }
}
