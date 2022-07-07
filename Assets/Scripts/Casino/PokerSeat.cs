using System.Collections.Generic;
using TMPro;

namespace Casino.Poker
{
	public class PokerSeat
	{
		public ConnectedPlayer connectedPlayer;
		public bool unresponsive;
		public bool occupied;
		public bool allowed; //can't play till you placed a high blind
		public int availableCash;
		
		//current game variables
		public List<PokerCard> cards = new List<PokerCard>();
		public bool playedThisPhase;
		public int currentBet;
		public HandCombinationRank handRank;

		public TextMeshPro betTextUI;
	}
	
	public enum PokerDecision 
	{
		Raise,
		Fold,
		Call,
		Idle
	}

	public enum PokerPhase
	{
		PreGame,
		Preflop,
		Flop,
		Turn,
		River,
		GameEnd
	}
	
}