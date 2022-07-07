using System;
using UnityEngine;

namespace House
{
	public class HousePictureFrame : MonoBehaviour, iClientSceneLoad
	{
		public int numberID;
		private MeshRenderer meshRenderer;

		public void Awake()
		{
			meshRenderer = GetComponentInChildren<MeshRenderer>();
		}

		public void FinishedLoadingScene()
		{
			var pickedNft = HousingManager.Instance.GetNFT(numberID);
			meshRenderer.materials[1].SetTexture("_BaseMap", pickedNft.texture);
		}
	}
}