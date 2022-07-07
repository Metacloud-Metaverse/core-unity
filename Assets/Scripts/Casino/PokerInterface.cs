using System;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using Messages.Client;
using TMPro;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Casino.Poker
{
	/// <summary>
	/// Receives messages from the server and displays it
	/// </summary>
	public class PokerInterface : MonoBehaviour //TODO: rename, PokerInterface and PokerPlayerInterface have too similar names, confusing
	{
		public Sprite backCardSprite;
		public List<Image> communityCards;
		public List<Image> othersPlayersCards;
		public List<RectTransform> comunityCardsTargetPos = new List<RectTransform>();
		public List<RectTransform> localPlayerCardPositions = new List<RectTransform>();
		public List<RectTransform> allOtherPlayerCardPositions = new List<RectTransform>();

		public List<Sprite> Clubs = new List<Sprite>();
		public List<Sprite> Diamonds = new List<Sprite>();
		public List<Sprite> Hearts = new List<Sprite>();
		public List<Sprite> Spades = new List<Sprite>();
		public Sprite upsideDownCard;

		public List<PokerPlayerInterface> playerInterfaces = new List<PokerPlayerInterface>();
		public TextMeshProUGUI mainBetPool;
		public TextMeshProUGUI winnerAnnounce;
		[Header("Animations Settings")] 
		public iTween.EaseType easeRot = iTween.EaseType.linear;
		public float timeRotation = 1;
		public float delayRot = 0.3f;
		public iTween.EaseType easeMove = iTween.EaseType.linear;
		public float timeMove = 1;

		public float delayPerDealerCardOnInit = 0.4f;
		public float delayPerMoveCard = 0.6f;
		public float delayPerMoveToBackDeck = 0.1f;
		
		private int currentMovementComunityCard = 0;
		private int indexMovementOthersCard = 0;
		private float currentOthersPlayersDealyTime;

		public PokerTable pokerTable;

		private List<PlayersCardsDataPoker> playersCardsData = new List<PlayersCardsDataPoker>();
		private PokerPlayerInterface currentTurn;


		private List<Vector3> initOthersPlayersPosition = new List<Vector3>();
		private List<Vector3> initComunityCardsPosition = new List<Vector3>();
		public void Start()
		{
			ClearAll();
			foreach (var item in othersPlayersCards)
			{
				initOthersPlayersPosition.Add(item.GetComponent<RectTransform>().position);
			}
			foreach (var item in communityCards)
			{
				initComunityCardsPosition.Add(item.GetComponent<RectTransform>().position);
			}
		}
		private void ClearAll()
		{
			ClearCards();
			foreach (var playerInterface in playerInterfaces)
			{
				playerInterface.ownBet.text = "";
				playerInterface.availableCash.text = "";
			}
			mainBetPool.text = "";
			winnerAnnounce.text = "";
		}

		public void HandEnded()
		{
			ClearCards();
			foreach (var playerInterface in playerInterfaces)
			{
				playerInterface.ownBet.text = "";
			}
			mainBetPool.text = "";
			winnerAnnounce.text = "";
		}
		
		public void ClearCards()
		{
			/*foreach (var communityCard in communityCards)
			{
				communityCard.color = Color.clear;
			}*/

			/*foreach (var playerInterface in playerInterfaces)
			{
				playerInterface.cards[0].color = Color.clear;
				playerInterface.cards[1].color = Color.clear;
			}*/
			winnerAnnounce.text = ""; // TODO: move this
		}

		public void AnnounceWinner(string value)
		{
			winnerAnnounce.text = value;
			playersCardsData.Clear();
			StartCoroutine(RotateCardsToBack());
		}

		private IEnumerator RotateCardsToBack()
        {
			for (int i = 0; i < 2; i++)
			{
				StartCoroutine(FlipAndChangeSpriteComunityCard(0, othersPlayersCards[i], backCardSprite));
				yield return Yielders.Seconds(0.1f);
			}
			for (int i = 0; i < communityCards.Count; i++)
			{
				StartCoroutine(FlipAndChangeSpriteComunityCard(0, communityCards[i], backCardSprite));
				yield return Yielders.Seconds(0.1f);
			}
			StartCoroutine(MoveAllCardsToDeck());
		}
		public IEnumerator MoveAllCardsToDeck()
		{
			var maxIndex = indexMovementOthersCard-1;
			for (int i = 0; i < pokerTable.CurrentHandPlayers.Count; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					var card = othersPlayersCards[maxIndex];
					var pos = initOthersPlayersPosition[maxIndex];
					maxIndex--;
					Debug.Log("MOVE TO DECK : "+card.name+", Position"+pos);
					var hash = iTween.Hash("position", pos, "time", timeMove,"easeType",easeMove,"isLocal",false);
					iTween.MoveTo(card.gameObject,hash);

					yield return Yielders.Seconds(delayPerMoveToBackDeck);
				}
			}

			for (int i = 0; i < communityCards.Count; i++)
			{
				var pos = initComunityCardsPosition[i];
				var hash = iTween.Hash("position", pos, "time", timeMove,"easeType",easeMove,"isLocal",false);
				iTween.MoveTo(communityCards[i].gameObject,hash);
				yield return Yielders.Seconds(delayPerMoveToBackDeck);
			}

			indexMovementOthersCard = 0;

			//	yield return new WaitUntil(() => pokerTable.CurrentHandPlayers.Count == playersCardsData.Count);
		/*	for (int i = 0; i < 2; i++)
			{
				var playerInterface = playerInterfaces[playersCardsData[i].playerIndex];
				var cardImage = GetCardSprite(playersCardsData[i].card);
				StartCoroutine( FlipAndChangeSpriteComunityCard(0, othersPlayersCards[i], cardImage));
			}*/

			//	currentOthersPlayersDealyTime -=delayPerMoveCard;
		}
		
		public void ChangeTurn(int playerIndex)
		{
			/*if (currentTurn != null)
			{
				currentTurn.turnMark.color = Color.clear;
			}*/
            for (int i = 0; i < playerInterfaces.Count; i++)
            {
                if (i != playerIndex)
                {
					playerInterfaces[i].OnSetPlayerWaitingForNextTurn();
				}
            }
			playerInterfaces[playerIndex].OnSetCurrentPlayerPlaying();

		//	currentTurn = playerInterfaces[playerIndex];
			//currentTurn.turnMark.color = Color.white;
		}

		public void AssignCommunityCard(int index, PokerCard pokerCard)
		{
			var cardImage = communityCards[index];
			var targetPos = comunityCardsTargetPos[index];
			var spriteTarget = GetCardSprite(pokerCard);

			if (currentMovementComunityCard > 0)
			{
				StartCoroutine(FlipAndChangeSpriteComunityCard(delayPerMoveCard,cardImage, spriteTarget));
				StartCoroutine(MoveComunityCard(delayPerMoveCard,targetPos,cardImage));
			}
			else
			{
				StartCoroutine(FlipAndChangeSpriteComunityCard(0f,cardImage, spriteTarget));
				StartCoroutine(MoveComunityCard(0f,targetPos,cardImage));
			}
			
			//	cardImage.color = Color.white;
		}

		private IEnumerator MoveComunityCard(float delay,RectTransform targetPos, Image targetImageUI)
		{
			currentMovementComunityCard++;
			yield return Yielders.Seconds(delay);

			var hash = iTween.Hash("position", targetPos.position, "time", timeMove,"easeType",easeMove,"isLocal",false);
			iTween.MoveTo(targetImageUI.gameObject,hash);
			yield return Yielders.Seconds(timeMove);
			currentMovementComunityCard--;
		}
		private IEnumerator FlipAndChangeSpriteComunityCard(float delay, Image targetImageUI, Sprite targetSprite)
		{
			yield return Yielders.Seconds(delay);
			var rTransform = targetImageUI.GetComponent<RectTransform>();
			yield return Yielders.Seconds(delayRot);
			var hash = iTween.Hash("y", 90,"time",timeRotation/2,"easeType",easeRot);
			iTween.RotateTo(targetImageUI.gameObject,hash);
			
			yield return Yielders.Seconds(timeRotation/2);
			targetImageUI.overrideSprite = targetSprite;
			hash = iTween.Hash("y", 360,"time",timeRotation/2,"easeType",easeRot);
			iTween.RotateTo(targetImageUI.gameObject,hash);
		}
		public void AssignPlayerCard(int cardIndex, int playerIndex, PokerCard card)
		{
			playersCardsData.Add( new PlayersCardsDataPoker(cardIndex,playerIndex,card));
			var playerInterface = playerInterfaces[playerIndex];
			//var cardImage = playerInterface.cards[cardIndex];
			//cardImage.sprite = GetCardSprite(card);
			//cardImage.color = Color.white;
			//if(card.suit == PokerSuit.UpsideDown)
			//	StartCoroutine(	MoveOtherPlayerCards());
		}
		public IEnumerator MoveOtherPlayerCards()
		{
			for (int i = 0; i < pokerTable.CurrentHandPlayers.Count; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					var card = othersPlayersCards[indexMovementOthersCard];
					var pos = allOtherPlayerCardPositions[indexMovementOthersCard];
					indexMovementOthersCard++;
					var hash = iTween.Hash("position", pos.position, "time", timeMove,"easeType",easeMove,"isLocal",false);
					iTween.MoveTo(card.gameObject,hash);

					yield return Yielders.Seconds(delayPerDealerCardOnInit);
				}
			}

		//	yield return new WaitUntil(() => pokerTable.CurrentHandPlayers.Count == playersCardsData.Count);
			for (int i = 0; i < 2; i++)
			{
				var playerInterface = playerInterfaces[playersCardsData[i].playerIndex];
				var cardImage = GetCardSprite(playersCardsData[i].card);
				StartCoroutine( FlipAndChangeSpriteComunityCard(0, othersPlayersCards[i], cardImage));
			}

			//	currentOthersPlayersDealyTime -=delayPerMoveCard;
		}
		
		public void AssignBetPoolChips(int value)
		{
			mainBetPool.text = value.ToString();
		}

		public void AssignPlayerChips(int value, int playerIndex, int availableCash)
		{
			var playerInterface = playerInterfaces[playerIndex];
			playerInterface.ownBet.text = value.ToString();
			playerInterface.availableCash.text = availableCash.ToString();
		}

		public void AssignPlayerSeat(int seatIndex, PokerSeatOccupiedStatus occupiedStatus)
		{
			var pokerPlayerInterface = playerInterfaces[seatIndex];
			pokerPlayerInterface.joinTableButton.ChangeButton(occupiedStatus);
		}

		private Sprite GetCardSprite(PokerCard card)
		{
			if (card.suit == PokerSuit.Clubs)
			{
				return Clubs[(int)card.number];
			}
			else if (card.suit == PokerSuit.Diamonds)
			{
				return Diamonds[(int)card.number];
			}
			else if (card.suit == PokerSuit.Hearts)
			{
				return Hearts[(int)card.number];
			}
			else if (card.suit == PokerSuit.Spades)
			{
				return Spades[(int)card.number];
			}
			return upsideDownCard;
		}
	}
}

public class PlayersCardsDataPoker
{
	public int cardIndex;
	public int playerIndex;
	public PokerCard card;

	public PlayersCardsDataPoker(int cardIndex, int playerIndex, PokerCard card)
	{
		this.cardIndex = cardIndex;
		this.playerIndex = playerIndex;
		this.card = card;
	}
}
