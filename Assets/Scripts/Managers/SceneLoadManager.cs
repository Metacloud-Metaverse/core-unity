using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Casino.Networking;
using Mirror;
using Network;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviourSingleton<SceneLoadManager>
{
    public static Dictionary<Scene, SceneController> sceneDictionary = new Dictionary<Scene, SceneController>();
    public static Scene firstLoadedScene;

    private bool isLoadingScene;
    public void Start()
    {
        OrchestralManager.Instance.OnGameStart += Initialize;
    }

    public void Initialize()
    {
        var currentSceneController = sceneDictionary[firstLoadedScene];
        currentSceneController.initialized = true;
        
        if (CustomNetworkManager.IsServer)
        {
            if (currentSceneController.sceneType == SceneType.Main)
            {
                //load outworld scenes, the parcel manager will load the world scenes used in the db
                StartCoroutine(LoadScene(GetScenePath(ParcelType.casino), LoadSceneMode.Additive));
                StartCoroutine(LoadScene(GetScenePath(ParcelType.roulette), LoadSceneMode.Additive));
                StartCoroutine(LoadScene(GetScenePath(ParcelType.house), LoadSceneMode.Additive));
                StartCoroutine(LoadScene(GetScenePath(ParcelType.poker), LoadSceneMode.Additive));
            }
        }
    }

    //load an additive scene
    public IEnumerator LoadScene(string scenePath, LoadSceneMode loadSceneMode, Vector2Int location = new Vector2Int(), Parcel parcel = null)
    {
        while (isLoadingScene)
        {
            yield return null;
        }
        isLoadingScene = true;

        AsyncOperation AsyncOP = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);

        while (UIManager.Instance.loadingScreen.IsProgressFinished(AsyncOP, loadSceneMode) == false)
        {
            yield return null;
        }

        UIManager.Instance.loadingScreen.HideLoadingScreen();
        //slightly hacky, loop through all scenes to find the one we just loaded
        while (true)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (sceneDictionary.ContainsKey(scene))
                {
                    var sceneController = sceneDictionary[scene];
                    if (sceneController.initialized == false)
                    {
                        sceneController.initialized = true;
                        isLoadingScene = false;
                        if (loadSceneMode == LoadSceneMode.Additive)
                        {
                            FinishLoadingAdditiveScene(parcel, location, sceneController);
                        }
                        else
                        {
                            FinishLoadingSingleScene(scene, sceneController, scenePath);
                        }

                        if (CustomNetworkManager.IsServer)
                        {
                            AssignInstanceNumber(sceneController, scene);
                        }
                        else
                        {
                            InitializeClientObjects(scene);
                        }
                        UIManager.Instance.loadingScreen.HideLoadingScreen();

                        yield break;
                    }
                }
            }
            yield return null;
        }
    }


    /// <summary>
    /// Gets all objects in the scene of the specific interface to run its function
    /// </summary>
    //TODO: optimize
    public void InitializeClientObjects(Scene scene)
    {
        var rootGameObjects = scene.GetRootGameObjects().ToList();
        var interfaceList = new List<iClientSceneLoad>();
        foreach (var objectInScene in rootGameObjects)
        {
            if (objectInScene.TryGetComponent<iClientSceneLoad>(out var interfaceComponent))
            {
                interfaceList.Add(interfaceComponent);
            }
            foreach (var interfaceComponentChild in objectInScene.GetComponentsInChildren<iClientSceneLoad>())
            {
                interfaceList.Add(interfaceComponentChild);
            }
        }
        foreach (var initializationInterface in interfaceList)
        {
            initializationInterface.FinishedLoadingScene();
        }
    }

    private void FinishLoadingSingleScene(Scene scene, SceneController sceneController, string newScenePath)
    {
        var playerGameobject = CustomNetworkManager.localPlayerController.gameObject;

        SceneManager.MoveGameObjectToScene(playerGameobject, scene);
        CustomNetworkManager.SwapNetworkScene(newScenePath);
        
        //remove scene camera
        if (sceneController.sceneType != SceneType.Game)
        {
            sceneController.camera.gameObject.SetActive(false);
        }

        //tell mirror we finished loading the scene
        NetworkClient.isLoadingScene = false;

        //unload all other scenes
        foreach (var otherSceneController in sceneDictionary.Values)
        {
            if (sceneController != otherSceneController)
            {
                SceneManager.UnloadSceneAsync(otherSceneController.gameObject.scene);
            }
        }
        
        //Tell the server we loaded the scene
        SceneTransferCompleteMessage.Send();
        CustomNetworkManager.instance.SetPlayerMode();
    }


    private void FinishLoadingAdditiveScene(Parcel parcel, Vector2Int location, SceneController sceneController)
    {
        if (parcel != null) //client only (for now?)
        {
            parcel.loadedScene = sceneController.gameObject.scene;
        }
        if (sceneController.sceneType != SceneType.Game)
        {
            sceneController.camera.gameObject.SetActive(false);
        }
        if (sceneController.sceneType == SceneType.WorldScene)
        {
            //Scene found through exploration, its loaded in the wrong spot, bring it over!
            MoveScenePosition(sceneController, location);
        }
    }
    
    /// <summary>
    /// Assigns on each networkidentity the instance number
    /// </summary>
    private void AssignInstanceNumber(SceneController sceneController, Scene scene)
    {
        var instanceNumber = 0;
        if (sceneController.sceneType != SceneType.WorldScene)
        {
            CustomNetworkManager.totalInstances ++;
        }
        instanceNumber = CustomNetworkManager.totalInstances;
        
        var sceneObjects = scene.GetRootGameObjects();
        foreach (var sceneObject in sceneObjects)
        {
            if (sceneObject.TryGetComponent<NetworkIdentity>(out var networkIdentity))
            {
                networkIdentity.instanceNumber = instanceNumber;
            }
        }
    }


    /// <summary>
    /// Moves all objects of the scene to a specific location using an anchor object
    /// </summary>
    private void MoveScenePosition(SceneController sceneController, Vector2 location)
    {
        var anchor = new GameObject("Anchor");
        anchor.transform.position = Vector3.zero;
        SceneManager.MoveGameObjectToScene(anchor, sceneController.gameObject.scene);
        var sceneObjects = sceneController.gameObject.scene.GetRootGameObjects().ToList();
        for (int i = sceneObjects.Count - 1; i >= 0; i--)
        {
            var objectInScene = sceneObjects[i];
            if (objectInScene.transform.parent != null)
            {
                sceneObjects.Remove(objectInScene);
                continue;
            }
            objectInScene.transform.SetParent(anchor.transform);
        }

        //anchor location
        var xPos = location.x * ParcelManager.Instance.parcelSize;
        var yPos = location.y * ParcelManager.Instance.parcelSize;

        anchor.transform.position = new Vector3(xPos, 0, yPos);
    }

    public void SendPlayerToScene(ConnectedPlayer connectedPlayer, string sceneName, int instancenumber)
    {
        //TODO: remove these checks
        if (sceneName == "Casino")
        {
            instancenumber = 1;
        }
        else if (sceneName == "Roulette")
        {
            instancenumber = 2;
        }
        else if (sceneName == "Poker")
        {
            instancenumber = 4;
        }
        
        
        var networkIdentity = connectedPlayer.networkConnection.identity;
        var targetScene = SceneManager.GetSceneByName(sceneName);
        NetworkConnectionToClient conn = networkIdentity.connectionToClient;

        // Tell client to load the new scene
        conn.Send(new SceneMessage {sceneName = targetScene.path, sceneOperation = SceneOperation.Normal, customHandling = true});

        // Move player to new subscene.
        SceneManager.MoveGameObjectToScene(networkIdentity.gameObject, targetScene);

        connectedPlayer.instanceNumber = instancenumber;
    }
    
    public static string GetScenePath(ParcelType parcelType)
    {
        if (parcelType == ParcelType.lobbyScene)
        {
            return "Modules/Backgrounds/Lobby/Scenes/Lobby";
        }
        else if (parcelType == ParcelType.casino)
        {
            return "Modules/Backgrounds/Casino/Scenes/Casino";
        }
        else if (parcelType == ParcelType.worldMain)
        {
            return "Scenes/Parcel Loading Test";
        }
        else if (parcelType == ParcelType.comedyClub)
        {
            return "Modules/Backgrounds/ComedyClub/Scene/ComedyClub_Scene";
        }
        else if (parcelType == ParcelType.artGallery)
        {
            return "Modules/Backgrounds/ArtGallery/Scenes/Nature";
        }
        else if (parcelType == ParcelType.roulette)
        {
            return "Modules/Backgrounds/Casino/Scenes/Roulette/Roulette";
        }
        else if (parcelType == ParcelType.house)
        {
            return "Modules/Backgrounds/MyHouse/Scene/MyHouse";
        }
        else if (parcelType == ParcelType.poker)
        {
            return "Scenes/Poker";
        }
        else if (parcelType == ParcelType.poolParty)
        {
            return "Modules/Backgrounds/PoolParty/Scene/PoolParty";
        }
        else if (parcelType == ParcelType.marketPlace)
        {
            return "Modules/Backgrounds/Markeplace/Scenes/Marketplace";
        }

        return "";
}

}
