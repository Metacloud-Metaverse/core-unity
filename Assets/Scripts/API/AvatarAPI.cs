using UnityEngine;

namespace APISystem
{
    public class AvatarAPI: API
    {
        public static new string url { get; private set; } = "https://avatar.api.meta-cloud.io";
        public static string saveDataPath { get; private set; } = "/avatar/save";
        public static string fetchDataPath { get; private set; } = "/avatar/fetch/{user_id}";
        
        public AvatarAPI(MonoBehaviour invoker) : base(invoker) { }


        public void SaveAvatar(int userId, AvatarInfo avatarInfo, APIConnection.ConnectionCallback callback)
        {
            var avatarData = new SaveAvatarData();
            avatarData.user_id = Client.Instance.id;
            avatarData.data = avatarInfo;
            var data = JsonUtility.ToJson(avatarData);

            var endpoint = CreateEndpoint(url, saveDataPath);
            _invoker.StartCoroutine(APIConnection.Send(APIConnection.Method.Post, endpoint, callback, data, Client.Instance.accessToken));
        }


        public void LoadAvatar(int userId, APIConnection.ConnectionCallback callback)
        {
            var endpoint = CreateEndpoint(url, fetchDataPath, userId.ToString());

            _invoker.StartCoroutine(APIConnection.Send(APIConnection.Method.Get, endpoint, callback));
        }     
    }
}
