using Mirror;
using Network;
using UnityEngine;

namespace Messages.Server
{
    public class SpawnPoofMessage : ServerMessage<SpawnPoofMessage.NetMessage>
    {
        public struct NetMessage : NetworkMessage
        {
            public Vector3 pos;
        }

        public override void Process(NetMessage msg)
        {
            CustomNetworkManager.localPlayerController.thirdPersonController.effects.ActiveStepEffectMSG(msg.pos);
        }

        public static void Send(Vector3 position)
        {
            
            NetMessage msg = new NetMessage()
            {
               pos = position,
            };
            SendToAllClients(msg);

        }
    }
}