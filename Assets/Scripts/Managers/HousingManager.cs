using System.Collections.Generic;
using House;
using Network;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class HousingManager : MonoBehaviourSingleton<HousingManager>
{
    public Dictionary<ConnectedPlayer, HouseInfo> houseInfoDictionary = new Dictionary<ConnectedPlayer, HouseInfo>();
    
    public HouseInfo localHouseInfo;
    public List<Sprite> DEBUGnftSprites = new List<Sprite>();
    public Material nftMaterial;

    public HouseInfo PlayerEnter(ConnectedPlayer houseOwner)
    {
        if (houseInfoDictionary.ContainsKey(houseOwner) == false)
        {
            var houseInfo = GetHouseInfo(houseOwner);
            houseInfoDictionary.Add(houseOwner, houseInfo);
        }
        return houseInfoDictionary[houseOwner];
    }
    
    public HouseInfo GetHouseInfo(ConnectedPlayer houseOwner)
    {
        //TODO: get house info from api
        var randomPictureFrames = new int[9];
        for (int i = 0; i < randomPictureFrames.Length; i++)
        {
            randomPictureFrames[i] = Random.Range(0, 7);
        }
        
        CustomNetworkManager.totalInstances ++;
        var houseInfo = new HouseInfo()
        {
            instanceNumber = CustomNetworkManager.totalInstances,
            pictureFrames = randomPictureFrames
        };
        return houseInfo;
    }
    
    public Sprite GetNFT(int numberID)
    {
        var NFTtexture = DEBUGnftSprites[localHouseInfo.pictureFrames[numberID]];
        return NFTtexture;
    }

    public void SaveLocalHouseInfo(HouseInfo houseInfo)
    {
        localHouseInfo = houseInfo;
    }
}

public struct HouseInfo
{
    public string nameTest;
    public int instanceNumber;
    public int[] pictureFrames;
}