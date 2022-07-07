using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapParcelInfo : MonoBehaviour
{
	private Vector2Int selectedPosition;
	public TextMeshProUGUI text;
	public GameObject anchor;
	public bool active;

	public void ShowParcelInformation()
	{
		if (active == false)
		{
			return;
		}
		
		if (ParcelManager.Instance.localParcelInfoDic.ContainsKey(selectedPosition))
		{
			FillText(ParcelManager.Instance.localParcelInfoDic[selectedPosition]);
		}
	}

	private void FillText(ParcelInfo parcelInfo)
	{
		text.text = $"Parcel info: \n {parcelInfo.Location} \n ";
	}
	
	private void ClearText()
	{
		text.text = "Loading...";
	}

	public void ToggleDisplay(Vector2Int currentMouseCoordinates)
	{
		if (active)
		{
			active = false;
			anchor.SetActive(false);
		}
		else
		{
			active = true;
			selectedPosition = currentMouseCoordinates;
			anchor.SetActive(true);
			anchor.transform.position = Input.mousePosition;
			if (ParcelManager.Instance.localParcelInfoDic.ContainsKey(selectedPosition) == false)
			{
				RequestParcelsMessage.Send(new List<Vector2Int>{selectedPosition});
				ClearText();
			}
			else
			{
				ShowParcelInformation();
			}
		}
	}
}