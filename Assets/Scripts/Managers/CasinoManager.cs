using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Casino
{
	public class CasinoManager : MonoBehaviourSingleton<CasinoManager>
	{
		public List<RoulettePosition> AllPositions = new List<RoulettePosition>();
		public Dictionary<int, RoulettePosition> AllPositionsByID = new Dictionary<int, RoulettePosition>();

		//client only wallet, server updates it
		public PlayerWallet localPlayerWallet;

		public Dictionary<int, PlayerWallet> AllWallets = new Dictionary<int, PlayerWallet>();
		public List<string> availableColors = new List<string>();
		
		public TextMeshProUGUI chipsUIText;
		
		public List<HandCombinations>  pokerHandCombinationsList = new List<HandCombinations>();
		public List<PokerCard> pokerCardList = new List<PokerCard>();
		public PokerCard upsideDownCard;

		public void Start()
		{
			localPlayerWallet = new PlayerWallet(1000);
			OrchestralManager.Instance.OnGameStart += Initialize;
			availableColors = ColorsHex.LightColors();
		}

		public void Initialize()
		{
			RequestCasinoChipsMessage.Send();
			foreach (var roulettePosition in AllPositions)
			{
				AllPositionsByID.Add(roulettePosition.ID, roulettePosition);
			}

			for (int i = 0; i < 13; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					var pokerCard = new PokerCard();
					pokerCard.number = (PokerNumbers)i;
					pokerCard.suit = (PokerSuit)j;
					pokerCardList.Add(pokerCard);
				}
			}
			upsideDownCard = new PokerCard();
			upsideDownCard.suit = PokerSuit.UpsideDown;
			
			//TODO: change into scripteable objects
			pokerHandCombinationsList.Add(new StraightFlush());
			pokerHandCombinationsList.Add(new FourOfAKind());
			pokerHandCombinationsList.Add(new FullHouse());
			pokerHandCombinationsList.Add(new Flush());
			pokerHandCombinationsList.Add(new Straight());
			pokerHandCombinationsList.Add(new ThreeOfAKind());
			pokerHandCombinationsList.Add(new TwoPair());
			pokerHandCombinationsList.Add(new OnePair());
			pokerHandCombinationsList.Add(new HighCard());
		}
		
		public void ServerRequestFunds(ConnectedPlayer connectedPlayer)
		{
			var playerWallet = new PlayerWallet(10000); //TODO: get wallet info from DB
			playerWallet.totalFunds = 10000;
			playerWallet.userID = connectedPlayer.networkConnection.connectionId;
			playerWallet.connectedPlayer = connectedPlayer;

			var colour = availableColors[Random.Range(0, availableColors.Count)];
			playerWallet.color = colour;
			if (availableColors.Count == 0)
			{
				availableColors = ColorsHex.LightColors();
			}

			if (AllWallets.ContainsKey(playerWallet.userID))
			{
				AllWallets.Remove(playerWallet.userID); //TODO: there's no saving method, userIDs get re-used 
			}
			AllWallets.Add(playerWallet.userID, playerWallet);
			
			UpdateCasinoChipsMessage.Send(connectedPlayer, playerWallet);
		}
		
	}
}


//TODO: util, move somewhere else and expand with more colors, functions, etc
public static class ColorsHex
{
	public static List<string> LightColors()
	{
		var colorList = new List<string>();
		colorList.Add(Silver);
		colorList.Add(Red);
		colorList.Add(Yellow);
		colorList.Add(Lime);
		colorList.Add(Aqua);
		colorList.Add(Fuchsia);
		return colorList;
	}

	public static string Lime = "#00FF00";
	public static string White = "#FFFFFF";
	public static string Silver = "#C0C0C0";
	public static string Gray = "#808080";
	public static string Black = "#000000";
	public static string Red = "#FF0000";
	public static string Maroon = "#800000";
	public static string Yellow = "#FFFF00";
	public static string Olive = "#808000";
	public static string Green = "#008000";
	public static string Aqua = "#00FFFF";
	public static string Teal = "#008080";
	public static string Blue = "#0000FF";
	public static string Navy = "#000080";
	public static string Fuchsia = "#FF00FF";
	public static string Purple = "#800080";
}