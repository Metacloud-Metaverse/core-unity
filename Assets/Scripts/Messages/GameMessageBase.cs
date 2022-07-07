using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Messages
{
	public abstract class GameMessageBase<T> where T : struct, NetworkMessage
	{
		//TODO: for logging/tracking, expand later
		public static string lastGameMessage;
		public static bool gameMessageProcessing;
		
		/// <summary>
		/// Called before any message processing takes place
		/// </summary>
		public virtual void PreProcess(NetworkConnection sentBy, T b)
		{
			gameMessageProcessing = true;
			lastGameMessage = ToString();
			Process(sentBy, b);
			gameMessageProcessing = false;
		}

		public abstract void Process(T msg);

		public virtual void Process(NetworkConnection sentBy, T msg)
		{
			// This is to stop the server disconnecting if theres an error in the processing of the net message
			// on either client or server side, mirror dislikes runtime errors
			try
			{
				Process(msg);
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
		}

		/// <summary>
		/// Finds and returns a NetworkIdentity based on an uint id
		/// </summary>
		protected NetworkIdentity LoadNetworkObject(uint id)
		{
			if (NetworkIdentity.spawned.ContainsKey(id))
			{
				var networkIdentity = NetworkIdentity.spawned[id];
				return networkIdentity;
			}
			return null;
		}

		/// <summary>
		/// Finds and returns several NetworkIdentitys based on uint ids
		/// </summary>
		protected NetworkIdentity[] LoadMultipleObjects(List<uint> ids)
		{
			var networkObjects = new NetworkIdentity[ids.Count];
			for (var i = 0; i < ids.Count; i++)
			{
				var netId = ids[i];
				networkObjects[i] = LoadNetworkObject(netId);
			}
			return networkObjects;
		}
	}
}