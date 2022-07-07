using Messages.Server;
using Mirror;

public class HouseInfoMessage : ServerMessage<HouseInfoMessage.NetMessage>
{
    public struct NetMessage : NetworkMessage
    {
        public HouseInfo houseInfo;
    }
		
    public override void Process(NetMessage msg)
    {
        HousingManager.Instance.SaveLocalHouseInfo(msg.houseInfo);
    }

    public static void Send(ConnectedPlayer connectedPlayer, HouseInfo newHouseInfo)
    {
        NetMessage msg = new NetMessage()
        {
            houseInfo = newHouseInfo
        };
        SendToClient(connectedPlayer.networkConnection, msg);
    }
}