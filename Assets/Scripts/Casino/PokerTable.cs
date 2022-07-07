using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Messages.Client;
using Messages.Server;
using Mirror;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Casino.Poker
{
	public class PokerTable : MonoBehaviour, iNeedsServerUpdate
	{
		//DEFINITIONS
		public int smallBlindAmount = 5;
		public int highBlindAmount = 10;
		public int minRaise = 5;
		public int maxRaise = 100;
		public int antes = 1;
		public int playerWaitingTime = 60000;
		public int playerShortWaitingTime = 5000;
		public int gameEndTimer = 5000;
		public int maxPlayerLimit = 8;

		private List<PokerSeat> AllSeats = new List<PokerSeat>(); //TODO merge into 1 list, it's too complicated
		public List<PokerSeat> CurrentHandPlayers = new List<PokerSeat>();
		private Stopwatch WaitingTimer;
		public PokerInterface pokerInterface;

		//CURRENT GAME:
		public List<PokerCard> communityCards = new List<PokerCard>();
		public List<PokerCard> deckCards = new List<PokerCard>();
		private int dealerPositionIndex;
		private int smallBlindIndex;
		private int highBlindIndex;
		private PokerPhase currentPhase = PokerPhase.PreGame;
		private int mainPotBet;
		private int totalPotBet;
		private PokerSeat currentPlayerTurn;
		private bool waitingForPlayer;
		private bool waitingGameEnd;

		private void Start() //TODO: move to custom startup by interface
		{
			for (int i = 0; i < maxPlayerLimit; i++)
			{
				AllSeats.Add(new PokerSeat());
			}
			UpdateManager.Add(UpdateMe);
			WaitingTimer = new Stopwatch();
			pokerInterface = GetComponent<PokerInterface>();
		}
		
		private void UpdateMe()
		{
			if (waitingForPlayer)
			{
				var timeUsed = currentPlayerTurn.unresponsive ? playerShortWaitingTime : playerWaitingTime;
				if (WaitingTimer.ElapsedMilliseconds > timeUsed)
				{
					//player afk! he keeps playing unless the bet increased, in this case we force fold him
					var pokerDecision = PokerDecision.Call;
					if (currentPlayerTurn.currentBet < mainPotBet)
					{
						pokerDecision = PokerDecision.Fold;
					}
					currentPlayerTurn.unresponsive = true;
					EndPlayerTurn(pokerDecision);
				}
			}
			else if (waitingGameEnd)
			{
				if (WaitingTimer.ElapsedMilliseconds > gameEndTimer)
				{
					waitingGameEnd = false;
					PokerGamePhaseMessage.Send(this, currentPhase);
					ClearValues();
					BeginNextPhase();
				}
			}
		}

		public void TryPlayHand(PokerDecision pokerDecision, ConnectedPlayer sentBy)
		{
			if (waitingForPlayer && currentPlayerTurn != null && sentBy == currentPlayerTurn.connectedPlayer)
			{
				if(pokerDecision == PokerDecision.Call)
				{
					if (currentPlayerTurn.currentBet < mainPotBet)
					{
						TryBet(mainPotBet - currentPlayerTurn.currentBet, currentPlayerTurn);
					}
				}
				else if (pokerDecision == PokerDecision.Raise)
				{
					var betAmount = Mathf.Max(mainPotBet - currentPlayerTurn.currentBet, 0);
					betAmount += minRaise; //TODO: let player decide how much they bet (include in message?)
					TryBet(betAmount, currentPlayerTurn);
				}
				currentPlayerTurn.unresponsive = false;
				EndPlayerTurn(pokerDecision);
			}
		}

		public void EndPlayerTurn(PokerDecision pokerDecision)
		{
			var lastPlayer = currentPlayerTurn;
			currentPlayerTurn.playedThisPhase = true;
			var nextSeatIndex = CurrentHandPlayers.IndexOf(currentPlayerTurn) + 1;
			if (pokerDecision == PokerDecision.Fold)
			{
				//player folded, remove him and fix the next player's index
				CurrentHandPlayers.Remove(currentPlayerTurn);
				nextSeatIndex--;
			}

			if (CurrentHandPlayers.Count == nextSeatIndex)
			{
				currentPlayerTurn = CurrentHandPlayers[0];
			}
			else
			{
				currentPlayerTurn = CurrentHandPlayers[nextSeatIndex];
			}

			//only one player left, winner! Game ends early.
			if (CurrentHandPlayers.Count == 1)
			{
				DeclareWinner(currentPlayerTurn);
				currentPhase = PokerPhase.GameEnd;
				waitingForPlayer = false;
				return;
			}

			//inform players the decision of the player and who's turn is now
			PokerPlayerTurnUpdateMessage.Send(this, AllSeats.IndexOf(currentPlayerTurn), pokerDecision, AllSeats.IndexOf(lastPlayer));
			
			if (currentPlayerTurn.playedThisPhase == false || currentPlayerTurn.currentBet < mainPotBet)
			{
				WaitingTimer.Restart();
			}
			else
			{
				//full loop and nobody raised the bet again, end phase
				currentPhase++;
				waitingForPlayer = false;
				foreach (var pokerSeat in CurrentHandPlayers)
				{
					pokerSeat.playedThisPhase = false;
				}
				BeginNextPhase();
			}
		}
		
		public void ShowCommunityCard()
		{
			var card = deckCards[Random.Range(0, deckCards.Count)];
			deckCards.Remove(card);
			communityCards.Add(card);
			PokerCommunityCardMessage.Send(this, card, communityCards.IndexOf(card));
		}

		public void DistributePlayerCards()
		{
			foreach (var pokerSeat in CurrentHandPlayers)
			{
				for (int i = 0; i < 2; i++)
				{
					var card = deckCards[Random.Range(0, deckCards.Count)];
					deckCards.Remove(card);
					pokerSeat.cards.Add(card);
					var cardIndex = pokerSeat.cards.IndexOf(card);
					var playerIndex = AllSeats.IndexOf(pokerSeat);
					PokerPlayerCardMessage.SendToAll(this, card, cardIndex, playerIndex, true, pokerSeat.connectedPlayer);
				}
			}
			StartCoroutine(pokerInterface.MoveOtherPlayerCards());
		}

		private void AssignDealerAndBlinds() //TODO: overcomplicated, just assign highblind first
		{
			dealerPositionIndex = ReturnNextOccupiedSeat(dealerPositionIndex, true);
			smallBlindIndex = ReturnNextOccupiedSeat(dealerPositionIndex, true);
			highBlindIndex = ReturnNextOccupiedSeat(smallBlindIndex, false);
			var currentPlayerTurnIndex = ReturnNextOccupiedSeat(highBlindIndex, true);
			currentPlayerTurn = AllSeats[currentPlayerTurnIndex];
			var playerSeat = AllSeats[highBlindIndex];
			playerSeat.allowed = true; //placed high blind! he's now allowed to play
		}

		private int ReturnNextOccupiedSeat(int index, bool onlyAllowed)
		{
			for (int i = 0; i < AllSeats.Count; i++)
			{
				index++;
				if (index == AllSeats.Count)
				{
					index = 0;
				}
				var playerSeat = AllSeats[index];
				if (playerSeat.occupied && (onlyAllowed == false || playerSeat.allowed))
				{
					return AllSeats.IndexOf(playerSeat);
				}
			}
			return dealerPositionIndex;
		}
		

		private void BeginNextPhase()
		{
			if (currentPhase == PokerPhase.Preflop)
			{
				deckCards = new List<PokerCard>(CasinoManager.Instance.pokerCardList);
			
				CurrentHandPlayers.Clear();
				foreach (var pokerSeat in AllSeats)
				{
					if (pokerSeat.occupied) //TODO: check how many chips they have left
					{
						CurrentHandPlayers.Add(pokerSeat);
					}
				}

				if (CurrentHandPlayers.Count < 2) //not enough players, stop entirely
				{
					currentPhase = PokerPhase.PreGame;
					CurrentHandPlayers.Clear();
					return;
				}

				AssignDealerAndBlinds();
				
				//remove all players not allowed to play
				for (int i = CurrentHandPlayers.Count - 1; i >= 0; i--)
				{
					var playerSeat = CurrentHandPlayers[i];
					if (playerSeat.allowed == false)
					{
						CurrentHandPlayers.Remove(playerSeat);
					}
				}
				
				if (CurrentHandPlayers.Count < 2) //TODO: blind order lost, restart game and reward the last player remaining as dealer, if any
				{
					CurrentHandPlayers.Clear();
					return;
				}

				Debug.Log($"--PHASE Preflop started");
				TryBet(smallBlindAmount, AllSeats[smallBlindIndex]);
				TryBet(highBlindAmount, AllSeats[highBlindIndex]);
				DistributePlayerCards();
				PokerPlayerTurnUpdateMessage.Send(this, AllSeats.IndexOf(currentPlayerTurn));
				WaitForDecisions();
			}
			else if (currentPhase == PokerPhase.Flop)
			{
				Debug.Log($"--PHASE Flop started");
				ShowCommunityCard();
				ShowCommunityCard();
				ShowCommunityCard();
				WaitForDecisions();
			}
			else if (currentPhase == PokerPhase.Turn)
			{
				Debug.Log($"--PHASE Turn started");
				ShowCommunityCard();
				WaitForDecisions();
				
			}
			else if (currentPhase == PokerPhase.River)
			{
				Debug.Log($"--PHASE River started");
				ShowCommunityCard();
				WaitForDecisions();
			}
			else if (currentPhase == PokerPhase.GameEnd)
			{
				Debug.Log($"--PHASE GameEnded started");
				CalculateWinner();
			}
		}
		
		private void CalculateWinner()
		{
			var cardsArray = new PokerCard[7];
			for (int i = 0; i < communityCards.Count; i++)
			{
				cardsArray[i] = communityCards[i];
			}
			
			foreach (var pokerSeat in CurrentHandPlayers)
			{
				cardsArray[5] = pokerSeat.cards[0];
				cardsArray[6] = pokerSeat.cards[1];
				foreach (var handCombination in CasinoManager.Instance.pokerHandCombinationsList)
				{
					if (handCombination.IsPossible(cardsArray))
					{
						pokerSeat.handRank = handCombination.GetRank();
						Debug.Log($"Hand combination for player {AllSeats.IndexOf(pokerSeat)} is {pokerSeat.handRank}");
						break;
					}
				}
			}

			//checks for players with the same hand combination rank and checks for kickers to determine who's ahead - note: ties are still possible
			/*
			for (int i = 0; i < CurrentHandPlayers.Count; i++)
			{
				var checkA = false;
				var pokerSeatA = CurrentHandPlayers[i];
				for (int j = i; j < CurrentHandPlayers.Count; j++)
				{
					var pokerSeatB = CurrentHandPlayers[j];
					if (pokerSeatA.handRank == pokerSeatB.handRank)
					{
						checkA = true;
						pokerSeatB.handRank = HandCombinations.DeterminePerfectRank(cardsArray);
					}
				}
				if (checkA)
				{
					pokerSeatA.handRank = HandCombinations.DeterminePerfectRank(cardsArray);
				}
			}
			*/
			CurrentHandPlayers = CurrentHandPlayers.OrderByDescending(x => x.handRank).ToList();

			DeclareWinner(CurrentHandPlayers[0]);
		}

		private void DeclareWinner(PokerSeat winnerSeat)
		{
			winnerSeat.availableCash += totalPotBet;
			PokerChipsUpdate.Send(this, winnerSeat.currentBet, false, AllSeats.IndexOf(winnerSeat), winnerSeat.availableCash);
			PokerChipsUpdate.Send(this, 0, true);
			PokerWinnerAnnounceMessage.Send(this, $"PLAYER {AllSeats.IndexOf(winnerSeat)} WON \n {totalPotBet} COINS!");
			WaitingTimer.Restart();
			waitingGameEnd = true;
			foreach (var pokerSeat in CurrentHandPlayers)
			{
				for (int i = 0; i < 2; i++)
				{
					PokerPlayerCardMessage.SendToAll(this, pokerSeat.cards[i], i, AllSeats.IndexOf(pokerSeat), false, pokerSeat.connectedPlayer);
				}
			}
			
		}

		private void ClearValues()
		{
			currentPlayerTurn = null;
			waitingForPlayer = false;
			mainPotBet = 0;
			totalPotBet = 0;
			currentPhase = PokerPhase.Preflop;
			communityCards.Clear();
			foreach (var pokerSeat in AllSeats)
			{
				if (pokerSeat.unresponsive)
				{
					//afk, kick the player out
					TryLeave(AllSeats.IndexOf(pokerSeat), pokerSeat.connectedPlayer);
				}
				pokerSeat.cards.Clear();
				pokerSeat.currentBet = 0;
				pokerSeat.playedThisPhase = false;
				pokerSeat.handRank = 0;
				
				//player no longer at his seat, free it completely
				if (pokerSeat.occupied == false)
				{
					FreeSeat(pokerSeat);
				}
			}
		}

		/// <summary>
		/// frees up the seat completely, no longer being owned by a player in case of leaving by mistake or disconnect
		/// </summary>
		private void FreeSeat(PokerSeat pokerSeat)
		{
			pokerSeat.connectedPlayer = null;
			pokerSeat.unresponsive = false;
			pokerSeat.allowed = false;
			PokerPlayerLeaveJoinMessage.SendToAll(this, AllSeats.IndexOf(pokerSeat), false);
		}

		private void WaitForDecisions()
		{
			waitingForPlayer = true;
			WaitingTimer.Restart();
		}

		public void TryJoin(int playerSeatIndex, ConnectedPlayer connectedPlayer)
		{
			if (AllSeats.Count <= playerSeatIndex)
			{
				return;
			}
			var pokerSeat = AllSeats[playerSeatIndex];
			if (pokerSeat.occupied || pokerSeat.connectedPlayer != null)
			{
				return;
			}
			Debug.Log($"player joined in index {playerSeatIndex}");
			pokerSeat.occupied = true;
			pokerSeat.connectedPlayer = connectedPlayer;
			PokerPlayerLeaveJoinMessage.SendToAll(this, playerSeatIndex, true, connectedPlayer);
			pokerSeat.availableCash = 100; //TODO: retrieve from player's wallet: all or fixed number? ask the player?
			if (currentPhase == PokerPhase.PreGame)
			{
				pokerSeat.allowed = true;
				currentPhase = PokerPhase.Preflop; //TODO: add some timer and or confirm button before the game actually starts?
				BeginNextPhase();
			}
		}

		public void TryLeave(int playerSeatIndex, ConnectedPlayer connectedPlayer)
		{
			if (AllSeats.Count <= playerSeatIndex)
			{
				return;
			}
			var pokerSeat = AllSeats[playerSeatIndex];
			if (pokerSeat.occupied && pokerSeat.connectedPlayer == connectedPlayer)
			{
				pokerSeat.occupied = false;
			}

			if (currentPhase == PokerPhase.PreGame)
			{
				//hand is not being played, free the seat instantly
				FreeSeat(pokerSeat);
			}
		}

		private void TryBet(int value, PokerSeat pokerSeat)
		{
			var playerWallet = CasinoManager.Instance.AllWallets[pokerSeat.connectedPlayer.networkConnection.connectionId];
			if (pokerSeat.availableCash > value)
			{
				totalPotBet += value;
				pokerSeat.currentBet += value;
				mainPotBet = pokerSeat.currentBet;
				pokerSeat.availableCash -= value;
				PokerChipsUpdate.Send(this, pokerSeat.currentBet, false, AllSeats.IndexOf(pokerSeat), pokerSeat.availableCash);
				PokerChipsUpdate.Send(this, totalPotBet, true);
			}
			else 
			{
				//TODO: all in bet pools
			}
		}

		
		//TODO: re-do in a single message with a serialization
		public void SendServerUpdate(ConnectedPlayer connectedPlayer)
		{
			foreach (var communityCard in communityCards)
			{
				PokerCommunityCardMessage.Send(this, communityCard, communityCards.IndexOf(communityCard), connectedPlayer);
			}

			foreach (var pokerSeat in AllSeats)
			{
				var pokerSeatIndex = AllSeats.IndexOf(pokerSeat);
				if (CurrentHandPlayers.Contains(pokerSeat))
				{
					for (int i = 0; i < 2; i++)
					{
						PokerPlayerCardMessage.Send(this, i, pokerSeatIndex, connectedPlayer);
					}
					PokerChipsUpdate.Send(this, pokerSeat.currentBet, false, pokerSeatIndex, pokerSeat.availableCash, connectedPlayer);
				}
				PokerPlayerLeaveJoinMessage.Send(this, pokerSeatIndex, pokerSeat.occupied, connectedPlayer);
			}
			PokerChipsUpdate.Send(this, totalPotBet, true, 0, 0, connectedPlayer);
			PokerGamePhaseMessage.Send(this, currentPhase);
			if (currentPlayerTurn != null)
			{
				PokerPlayerTurnUpdateMessage.Send(this, AllSeats.IndexOf(currentPlayerTurn));
			}
		}

	}


}