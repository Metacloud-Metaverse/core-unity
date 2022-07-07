using Casino;
using Messages.Server;
using Mirror;

public class UpdateCasinoChipsMessage : ServerMessage<UpdateCasinoChipsMessage.NetMessage>
{
    public struct NetMessage : NetworkMessage
    {
        public int totalFunds;
        public int availableFunds;
        public int bettedFounds;
        public int lastBetLosses;
        public int lastBetEarnings;
    }
		
    public override void Process(NetMessage msg)
    {
        var localPlayerWallet = CasinoManager.Instance.localPlayerWallet;
        
        localPlayerWallet.totalFunds = msg.totalFunds;
        localPlayerWallet.availableFunds = msg.availableFunds;
        localPlayerWallet.bettedFounds = msg.bettedFounds;
        localPlayerWallet.lastBetLosses = msg.lastBetLosses;
        localPlayerWallet.lastBetEarnings = msg.lastBetEarnings;
        localPlayerWallet.GetWalletString();
    }

    public static void Send(ConnectedPlayer connectedPlayer, PlayerWallet playerWallet)
    {
        NetMessage msg = new NetMessage()
        {
            totalFunds = playerWallet.totalFunds,
            availableFunds = playerWallet.availableFunds,
            bettedFounds = playerWallet.bettedFounds,
            lastBetLosses = playerWallet.lastBetLosses,
            lastBetEarnings = playerWallet.lastBetEarnings
        };
        SendToClient(connectedPlayer.networkConnection, msg);
    }
}
