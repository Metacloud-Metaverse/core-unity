using Casino;
using Messages.Client;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using GLTFast;
using UnityEngine;

public class EnterHouseRequestMessage : ClientMessage<EnterHouseRequestMessage.NetMessage>
{
    public struct NetMessage : NetworkMessage
    {

    }
		
    public override void Process(NetMessage msg)
    {
        var houseInfo = HousingManager.Instance.PlayerEnter(SentByPlayer);
        HouseInfoMessage.Send(SentByPlayer, houseInfo);
        SceneLoadManager.Instance.SendPlayerToScene(SentByPlayer, SceneLoadManager.GetScenePath(ParcelType.house), houseInfo.instanceNumber);
    }

    public static void Send()
    {
        NetMessage msg = new NetMessage()
        {

        };
        SendToServer(msg);
    }
}
