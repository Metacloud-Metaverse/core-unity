using System.Collections;
using Casino.Networking;
using Mirror;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using SystemInfo = UnityEngine.Device.SystemInfo;

namespace Network
{
	public class CustomNetworkManager : NetworkManager
	{
		public static bool IsServer { get; private set; } //TODO: clean these statics on server shutdown
		public static bool IsHeadlessServer { get; private set; }

		public static NetworkIdentity LocalPlayer;
		public static LocalPlayerController localPlayerController;
		public static CustomNetworkManager instance;
		public static int totalInstances;

		public override void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
			else
			{
				Destroy(gameObject);
			}
			base.Awake();
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			NetworkManagerExtensions.RegisterClientHandlers();
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			IsServer = true;
			NetworkManagerExtensions.RegisterServerHandlers();
			if (CheckHeadlessState())
			{
				StartCoroutine(WaitForMirror());
			}
		}
		
		public override void OnServerReady(NetworkConnectionToClient conn)
		{
			base.OnServerReady(conn);
			var connectedPlayer = new ConnectedPlayer();
			connectedPlayer.networkConnection = conn;
			connectedPlayer.currentSceneType = SceneType.Main;
			PlayersManager.Instance.playerList.Add(connectedPlayer);
		}

		public override void OnClientChangeScene(string newScenePath, SceneOperation sceneOperation, bool customHandling)
		{
			if (sceneOperation == SceneOperation.Normal)
			{
				if (IsServer)
				{
					//if the server player (for testing purposes, usually editor play) needs to move scenes, we dont need to load or unload anything
					SceneTransferCompleteMessage.Send();
					SetPlayerMode();
					var scene = SceneManager.GetSceneByPath(newScenePath);
					SceneLoadManager.Instance.InitializeClientObjects(scene);
				}
				else
				{
					UIManager.Instance.loadingScreen.ShowLoadingScreen();
					StartCoroutine(SceneLoadManager.Instance.LoadScene(newScenePath, LoadSceneMode.Single));
				}
			}
		}

		public void SetPlayerMode()
		{
			var sceneController = SceneLoadManager.sceneDictionary[localPlayerController.gameObject.scene];
			if (sceneController.sceneType == SceneType.Game)
			{
				//2D game scenes do not have player movement
				localPlayerController.TogglePlayerUsage(false);
				if (IsServer)
				{
					sceneController.camera.gameObject.SetActive(true);
					sceneController.sceneCanvas.enabled = true;
				}
			}
			else
			{
				localPlayerController.TogglePlayerUsage(true);
			}
		}

		public static void SwapNetworkScene(string newSceneName)
		{
			networkSceneName = newSceneName;
		}
		
		public override void OnClientConnect() 
		{
			base.OnClientConnect();
			StartCoroutine(WaitForMirror());
		}

		private IEnumerator WaitForMirror() //TODO: unproper way to wait for mirror's initializations
		{
			yield return new WaitForSeconds(0.1f);
			OrchestralManager.Instance.GameStarted();
		}

		private bool CheckHeadlessState()
		{
			if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null)
			{
				IsHeadlessServer = true;
				return true;
			}
			return false;
		}
	}
}
