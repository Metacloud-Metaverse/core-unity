using UnityEngine;
using UnityEngine.UI;

public class TestImport : MonoBehaviour
{
    //public ModelLoader gltfLoader;
    public string domain = "localhost";
    public string file = "separated.gltf";

    public InputField inputDomain;
    public InputField inputFile;

    void Start()
    {
        inputDomain.text = domain;
        inputFile.text = file;
    }

    public void Download()
    {
        //gltfLoader.DownloadFile(inputDomain.text + "/" + inputFile.text);
    }
}
