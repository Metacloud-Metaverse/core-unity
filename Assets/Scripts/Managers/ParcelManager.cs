using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Casino.Networking;
using Mirror;
using UnityEngine;
using Network;
using UnityEngine.SceneManagement;

public class ParcelManager : MonoBehaviourSingleton<ParcelManager>
{
	//definitions
	public int Ymax;
	public int Xmax;
	public int parcelSize;
	//ranges are from center to side
	public int loadViewRange;
	public int unloadViewRange;
	
	public Dictionary<Vector2Int, ParcelInfo> serverAllParcelInfo = new Dictionary<Vector2Int, ParcelInfo>();
	
	//list of serialized information of each parcel
	public Dictionary<Vector2Int, ParcelInfo> localParcelInfoDic = new Dictionary<Vector2Int, ParcelInfo>();
	
	//list of the actual gameobjects in the scene
	public Dictionary<Vector2Int, Parcel> localParcelDic = new Dictionary<Vector2Int, Parcel>();
	
	public GameObject parcelPrefab;
	public GameObject streetPrefab;
	public Vector2Int currentPlayerLocation;
	
	public void Start()
	{
		OrchestralManager.Instance.OnGameStart += Initialize;
	}

	public void Initialize()
	{
		for (int x = 0-Xmax/2; x < Xmax/2; x++)
		{
			for (int y = 0-Ymax/2; y < Ymax/2; y++)
			{
				var parcelInfo = new ParcelInfo();
				parcelInfo.Location.x = x;
				parcelInfo.Location.y = y;
				parcelInfo.Size = new Vector2(1,1);
				serverAllParcelInfo.Add(parcelInfo.Location, parcelInfo);
			}	
		}
		//DEBUG: adds some large combinations of parcel close to spawn
		DEBUGParcelInfoGenerator.OverwriteWithScene( -6, -6, 7, 7, -6, -6,ParcelType.lobbyScene);
		DEBUGParcelInfoGenerator.OverwriteWithScene( -11, -4, -8, 0, -11, -4, ParcelType.artGallery);
		DEBUGParcelInfoGenerator.OverwriteWithScene( -18, -5, -13, 0, -18, -5, ParcelType.poolParty);
		DEBUGParcelInfoGenerator.OverwriteWithScene( -11, 3, -9, 5, -11, 3, ParcelType.marketPlace);
		DEBUGParcelInfoGenerator.OverwriteWithSpecificParcel(-20, 1, -7, 2, ParcelType.streetRotated);
		//DEBUGAddGltfToParcel();

		
		LoadCloseParcels();

		if (CustomNetworkManager.IsServer)
		{
			foreach (var parcelInfo in serverAllParcelInfo.Values)
			{
				if (Parcel.IsParcelTypeWorldScene(parcelInfo.parcelType))
				{
					StartCoroutine(SceneLoadManager.Instance.LoadScene(SceneLoadManager.GetScenePath(parcelInfo.parcelType), LoadSceneMode.Additive, parcelInfo.Location));
				}
			}
		}
		UpdateManager.Add(UpdateMe);
	}

	//TODO: remove, its just for debug/testing
	public void DEBUGAddGltfToParcel()
	{
		var location = new Vector2Int(0, 0);
		var parcelInfo = serverAllParcelInfo[location];
		parcelInfo.pendingObjects = new List<ParcelObjectInfo>();
		var parcelObjectInfo = new ParcelObjectInfo();
		parcelObjectInfo.localPosition = new Vector3(0,0,0);
		parcelObjectInfo.localScale = new Vector3(1,1,1);
		parcelObjectInfo.rotation = new Quaternion(0,0,0,0);
		parcelObjectInfo.url = "https://raw.githubusercontent.com/KhronosGroup/glTF-Sample-Models/master/2.0/Duck/glTF/Duck.gltf";
		parcelInfo.pendingObjects.Add(parcelObjectInfo);
		serverAllParcelInfo[location] = parcelInfo;
	}
	

	public void UpdateMe()
	{
		if (CustomNetworkManager.localPlayerController == null) //TODO: just use updatemanager once the localplayercontroller is set, avoid null check
		{
			return;
		}

		//TODO: not terrible or even bad, but low on performance
		var sceneController = SceneLoadManager.sceneDictionary[CustomNetworkManager.localPlayerController.gameObject.scene];
		if (sceneController.sceneType != SceneType.Main)
		{
			localParcelDic.Clear();
			return;
		}

		var playerPos = CustomNetworkManager.localPlayerController.thirdPersonController.transform.position;
		var playerLocX = Mathf.RoundToInt(playerPos.x / parcelSize);
		var playerLocY = Mathf.RoundToInt(playerPos.z / parcelSize);
		if (playerLocX != currentPlayerLocation.x || playerLocY != currentPlayerLocation.y)
		{
			currentPlayerLocation.x = playerLocX;
			currentPlayerLocation.y = playerLocY;
			
			LoadCloseParcels();
			UnloadFarAwayParcels();
		}
	}


	public void LoadCloseParcels()
	{
		//form a square with the player as the center to know which locations to load
		var loadSquare = new Vector2Int(currentPlayerLocation.x - loadViewRange, currentPlayerLocation.y - loadViewRange);
		var index = new Vector2Int();
		var listToRequest = new List<Vector2Int>();
		for (int y = loadSquare.y; y < loadViewRange * 2 + loadSquare.y; y++)
		{
			for (int x = loadSquare.x; x < loadViewRange * 2 + loadSquare.x; x++)
			{
				index.x = x;
				index.y = y;
				
				//TODO: check out of map limits (check it on server too later)

				if (localParcelDic.ContainsKey(index))
				{
					continue; //the parcel is currently loaded, ignore
				}

				var parcel = GenerateParcel(index);
				
				if (localParcelInfoDic.ContainsKey(index))
				{
					var parcelInfo = localParcelInfoDic[index];
					parcel.Initialize(parcelInfo);
					
					//the parcel is part of a bigger pattern of parcels, load them all
					if (parcelInfo.parcelPattern != ParcelPattern.alone)
					{
						var parentParcelInfo = localParcelInfoDic[parcelInfo.parent];
						foreach (var childrenPos in parentParcelInfo.children)
						{
							if (localParcelDic.ContainsKey(childrenPos) == false)
							{ 
								var adjacentParcel = GenerateParcel(childrenPos);
								var childrenParcelInfo = localParcelInfoDic[childrenPos];
								adjacentParcel.Initialize(childrenParcelInfo);
							}
						}
					}
				}
				else
				{
					listToRequest.Add(index);
				}
			}
		}
		RequestParcelsMessage.Send(listToRequest);
	}
	
	//add chunks to lower list sizes?
	public Parcel GenerateParcel(Vector2Int location)
	{
		var parcelObject = InstantiateParcelPrefab(parcelPrefab, location);
		var parcel = parcelObject.GetComponent<Parcel>();
		parcel.parcelInfo.Location = location; //TODO: remove
		localParcelDic.Add(location, parcel);
		return parcel;
	}

	public GameObject InstantiateParcelPrefab(GameObject usedPrefab, Vector2Int location)
	{
		var parcelObject = Instantiate(usedPrefab);
		parcelObject.transform.position = new Vector3(location.x * parcelSize, 0, location.y * parcelSize);
		parcelObject.name = $"Parcel - ({location.x}, {location.y})";
		return parcelObject;
	}

	//to avoid GC
	private List<Parcel> parcelsToUnload = new List<Parcel>();
	private List<Parcel> parcelsToCheck = new List<Parcel>();
	public void UnloadFarAwayParcels()
	{
		parcelsToCheck = localParcelDic.Values.ToList();
		while (parcelsToCheck.Count > 0)
		{
			var parcel = parcelsToCheck[0];
			parcelsToCheck.Remove(parcel);
			if (IsParcelOutOfView(parcel.parcelInfo))
			{
				if (parcel.parcelInfo.parcelPattern != ParcelPattern.alone)
				{
					//the parcel is part of a pattern, make sure they're all out of vision before unloading the entire thing
					var parentParcelInfo = localParcelInfoDic[parcel.parcelInfo.parent];
					var patternInView = false;
					foreach (var childrenPos in parentParcelInfo.children)
					{
						if (localParcelDic.ContainsKey(childrenPos))
						{
							var childrenParcel = localParcelDic[childrenPos];
							parcelsToCheck.Remove(childrenParcel);
						}
						var childrenParcelInfo = localParcelInfoDic[childrenPos];
						if (patternInView == false && IsParcelOutOfView(childrenParcelInfo) == false)
						{
							//we still loop to remove all the parcels from the check list
							patternInView = true;
						}
					}

					if (patternInView == false)
					{
						foreach (var childrenPos in parentParcelInfo.children)
						{						
							if (localParcelDic.ContainsKey(childrenPos))
							{
								var childrenParcel = localParcelDic[childrenPos];
								parcelsToUnload.Add(childrenParcel);
							}
						}
					}
				}
				else
				{
					parcelsToUnload.Add(parcel);
				}
			}
		}
		
		foreach (var parcel in parcelsToUnload)
		{
			localParcelDic.Remove(parcel.parcelInfo.Location);
			parcel.ShutDown();
			Destroy(parcel.gameObject); //TODO: pooling
		}
		parcelsToUnload.Clear();
	}

	public bool IsParcelOutOfView(ParcelInfo parcelInfo)
	{
		var distanceX = Mathf.Abs(parcelInfo.Location.x - currentPlayerLocation.x);
		var distanceY = Mathf.Abs(parcelInfo.Location.y - currentPlayerLocation.y);
		if (distanceX > unloadViewRange || distanceY > unloadViewRange)
		{
			return true;
		}

		return false;
	}
	
}



