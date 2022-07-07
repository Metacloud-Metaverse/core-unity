using System;
using System.Collections;
using System.Collections.Generic;
using Network;
using UnityEngine;

namespace Casino.Networking
{
    public class SceneController : MonoBehaviour
    {
        public SceneType sceneType;
        public Camera camera;
        public Canvas sceneCanvas;
        public GameObject postProcess;
        
        [HideInInspector]
        public List<CustomStartPosition> customStartPositionList = new List<CustomStartPosition>();
        
        /// <summary>
        /// used to find the scene being loaded after a LoadSceneAsync()
        /// </summary>
        [HideInInspector] 
        public bool initialized;
        
        public void Awake()
        {
            if (SceneLoadManager.sceneDictionary.Count == 0)
            {
                SceneLoadManager.firstLoadedScene = gameObject.scene;
            }
            else
            {
                if (sceneType == SceneType.Game && CustomNetworkManager.IsServer) //2D scene game, disable canvas for editor tests 
                {
                    //TODO: scenetype.game organization is a bit messy, overview and centralize
                    sceneCanvas.enabled = false;
                    camera.gameObject.SetActive(false);
                }
            }
            SceneLoadManager.sceneDictionary.Add(gameObject.scene, this);
        }

        public void OnDestroy()
        {
            SceneLoadManager.sceneDictionary.Remove(gameObject.scene);
        }
    }

    public enum SceneType
    {
        Main,
        WorldScene,
        Standalone,
        Game,
        FirstMenu,
    }
}