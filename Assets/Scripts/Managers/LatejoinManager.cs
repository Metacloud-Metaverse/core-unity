using System.Collections;
using System.Collections.Generic;
using Network;
using UnityEngine;
using System.Linq;
using Messages.Client;
using Mirror;

public class LatejoinManager : MonoBehaviourSingleton<LatejoinManager>
{
		public void Start()
		{
			OrchestralManager.Instance.OnGameStart += Initialize;
		}

		public void Initialize()
		{
			//clients make a list of all the objects that need an update of their ongoing status
			if (CustomNetworkManager.IsServer == false)
			{
				var IDList = new List<uint>();
				var objectArray = FindObjectsOfType<MonoBehaviour>();
				for (int i = objectArray.Length - 1; i >= 0; i--)
				{
					var monoBehaviour = objectArray[i];
					var requestInterface = (iNeedsServerUpdate)monoBehaviour.GetComponent(typeof(iNeedsServerUpdate));
					if (requestInterface != null)
					{
						var networkIdentity = monoBehaviour.GetComponent<NetworkIdentity>();
						IDList.Add(networkIdentity.netId);
					}
				}
				RequestLatejoinUpdate.Send(IDList);
			}
		}
	
}
