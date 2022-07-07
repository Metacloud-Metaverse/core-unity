using UnityEngine;
using GLTFast;

public class JsonToAvatarTest : MonoBehaviour
{
    public AvatarSystem cc;
    public GameObject prefab;
    public TextAsset json;

    void Start()
    {
        var avatar = CreateAvatar();
        var ap = new AvatarParser();
        ap.cc = cc;
        cc.SetBones(avatar);
        var message = json.text;

        ap.ApplyConfigToMesh(message,avatar);
    }

    public GameObject CreateAvatar()
    {
        var go = Instantiate(prefab);
        go.transform.position = Vector3.zero;
        go.transform.eulerAngles = new Vector3(0, 180, 0);
        //cc.SetRandomAvatar(go);
        return go;
    }

    public async void Load(GLTFast.GltfAsset gltf)
    {
        await gltf.Load("https://raw.githubusercontent.com/KhronosGroup/glTF-Sample-Models/master/2.0/Duck/glTF/Duck.gltf");
        Debug.Log("Loaded completed");
    }

}
