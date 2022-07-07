using System;
using UnityEngine;
using Messages.Client;
using TMPro;
using UnityEngine.EventSystems;
namespace Casino.Poker
{
	public class JoinTableButton : MonoBehaviour
	{
		[SerializeField] private PokerInterface pokerInterface;
		[SerializeField] private PokerPlayerInterface pokerPlayerInterface;
		[SerializeField] private TextMeshProUGUI playString;
		private PokerSeatOccupiedStatus occupiedStatus;
		public void Awake()
		{
			//pokerInterface = GetComponentInParent<PokerInterface>();
			//pokerPlayerInterface = GetComponentInParent<PokerPlayerInterface>();
			//playString = GetComponentInChildren<TextMeshProUGUI>();
		}

		public void OnButtonJoinPress()
		{
			Debug.Log("poker interface : " + pokerInterface);
			Debug.Log("poker PLAYER interface : " + pokerPlayerInterface);
			var index = pokerInterface.playerInterfaces.IndexOf(pokerPlayerInterface);
			if (occupiedStatus == PokerSeatOccupiedStatus.empty)
			{
				PokerRequestLeaveJoinMessage.Send(pokerInterface.pokerTable, index, true);
			}
			else if (occupiedStatus == PokerSeatOccupiedStatus.owned)
			{
				PokerRequestLeaveJoinMessage.Send(pokerInterface.pokerTable, index, false);
			}
		}


		public void ChangeButton(PokerSeatOccupiedStatus value)
		{
			occupiedStatus = value;
			if (occupiedStatus == PokerSeatOccupiedStatus.occupied)
			{
				playString.text = "OCCUPIED";
			}
			else if (occupiedStatus == PokerSeatOccupiedStatus.empty)
			{
				playString.text = "PLAY";
			}
			else if (occupiedStatus == PokerSeatOccupiedStatus.owned)
			{
				playString.text = "LEAVE";
			}
		}

      
    }
}