using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Messages.Server;
using Mirror;
using Network;
using UnityEngine;
using UnityEditor;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
using TMPro;

/*
namespace Casino
{
	public class RouletteOLD : MonoBehaviour
	{
		public bool betsClosed;
		private List<RouletteBet> currentBets = new List<RouletteBet>();
		private Queue<int> lastResults = new Queue<int>();
		
		private Stopwatch stopWatch;
		private float betsClosedTime = 1000;
		private float betsOpenTime = 5000;

		public TextMeshProUGUI betsListUI;
		private NetworkIdentity networkIdentity;
		
		private int DEBUGbetValue = 2;

		private void Awake()
		{
			networkIdentity = GetComponent<NetworkIdentity>();
			stopWatch = new Stopwatch();
			stopWatch.Start();
		}

		private void Update()
		{
			if (networkIdentity.isServer)
			{
				if (betsClosed)
				{
					if (stopWatch.ElapsedMilliseconds > betsClosedTime)
					{
						SpinRoulette();
						betsClosed = false;
						stopWatch.Restart();
						//RouletteBetsClosedMessage.Send(this, betsClosed);
					}
				}
				else
				{
					if (stopWatch.ElapsedMilliseconds > betsOpenTime)
					{
						betsClosed = true;
						stopWatch.Restart();
						//RouletteBetsClosedMessage.Send(this, betsClosed);
					}
				}
			}
		}

		private void UpdateBetsUI()
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("Last results: - ");
			foreach (var result in lastResults)
			{
				stringBuilder.Append($"{result} - ");
			}
			stringBuilder.AppendLine();
			foreach (var rouletteBet in currentBets)
			{
				stringBuilder.AppendLine($"<color={rouletteBet.playerWallet.color}>{rouletteBet.value} to {rouletteBet.roulettePosition.name}</color>");
			}
			betsListUI.text = stringBuilder.ToString();
		}

		public void PlaceBet(RoulettePosition roulettePosition)
		{
			var value = DEBUGbetValue;
			if (CanBet(value, CasinoManager.Instance.localPlayerWallet))
			{
				//RoulettePlaceBetMessage.Send(this, roulettePosition, DEBUGbetValue);
			}
		}
		
		public void ServerPlaceBet(ConnectedPlayer connectedPlayer, int value, int positionID)
		{
			var playerWallet = CasinoManager.Instance.AllWallets[connectedPlayer.networkConnection.connectionId];
			if (CanBet(value, playerWallet))
			{
				var rouletteBet = new RouletteBet();
				rouletteBet.value = value;
				rouletteBet.roulettePosition = CasinoManager.Instance.AllPositionsByID[positionID];
				rouletteBet.playerWallet = playerWallet;
				currentBets.Add(rouletteBet);
				
				playerWallet.funds -= value;
				UpdateCasinoChipsMessage.Send(playerWallet.connectedPlayer, playerWallet.funds);
				UpdateBetsUI();
				//RouletteUpdateTextMessage.Send(this, betsListUI.text);
			}
		}

		private bool CanBet(int value, PlayerWallet playerWallet)
		{
			if (betsClosed || value > playerWallet.funds || value <= 0)
			{
				return false;
			}
			return true;
		}
		
		private void SpinRoulette()
		{
			var result = Random.Range(0, 37);
			lastResults.Enqueue(result);
			if (lastResults.Count > 5)
			{
				lastResults.Dequeue();
			}
			
			var winnerList = new List<PlayerWallet>(); //TODO: gc
			foreach (var rouletteBet in currentBets)
			{
				if(rouletteBet.roulettePosition.includedNumbers.Contains(result))
				{
					var playerWallet = rouletteBet.playerWallet;
					playerWallet.funds += rouletteBet.value * (int)rouletteBet.roulettePosition.rewardMultiplier;
					if (winnerList.Contains(playerWallet) == false)
					{
						winnerList.Add(playerWallet);
					}
				}
			}
			currentBets.Clear();
			foreach (var playerWallet in winnerList)
			{
				UpdateCasinoChipsMessage.Send(playerWallet.connectedPlayer, playerWallet.funds);
			}
			UpdateBetsUI();
			//RouletteUpdateTextMessage.Send(this, betsListUI.text);
		}
		
	}
}
*/