using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DEBUGParcelInfoGenerator
{
	public static void OverwriteWithScene(int xMin, int yMin, int xMax, int yMax, int sceneX, int sceneY, ParcelType parcelType)
	{
		var parentPos = new Vector2Int(sceneX, sceneY);
		var childrenList = new List<Vector2Int>();
		for (int x = xMin; x <= xMax; x++)
		{
			for (int y = yMin; y <= yMax; y++)
			{
				var childrenPos = new Vector2Int(x, y);
				var parcelInfo = ParcelManager.Instance.serverAllParcelInfo[childrenPos];
				childrenList.Add(childrenPos);
				parcelInfo.parcelType = ParcelType.nothing;
				parcelInfo.parcelPattern = ParcelPattern.children;
				parcelInfo.parent = parentPos;
				ParcelManager.Instance.serverAllParcelInfo[childrenPos] = parcelInfo;
			}
		}
		
		var sceneParcelInfo = ParcelManager.Instance.serverAllParcelInfo[parentPos];
		sceneParcelInfo.parcelType = parcelType;
		sceneParcelInfo.parcelPattern = ParcelPattern.parent;
		sceneParcelInfo.children = childrenList;
		ParcelManager.Instance.serverAllParcelInfo[parentPos] = sceneParcelInfo;
	}

	public static void OverwriteWithSpecificParcel(int xMin, int yMin, int xMax, int yMax, ParcelType parcelType)
	{
		for (int x = xMin; x <= xMax; x++)
		{
			for (int y = yMin; y <= yMax; y++)
			{
				var parcelPos = new Vector2Int(x, y);
				var parcelInfo = ParcelManager.Instance.serverAllParcelInfo[parcelPos];
				parcelInfo.parcelType = parcelType;
				ParcelManager.Instance.serverAllParcelInfo[parcelPos] = parcelInfo;
			}
		}
	}

}
