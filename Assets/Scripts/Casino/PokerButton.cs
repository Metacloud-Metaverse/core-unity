using Messages.Client;
using UnityEngine;

namespace Casino.Poker
{
	public class PokerButton : MonoBehaviour
	{
		public PokerDecision buttonDecision;
		[SerializeField] private PokerTable pokerTable; 
		public void OnMouseDown()
		{
			PokerPlayHandMessage.Send(pokerTable, buttonDecision);
		}
	}
}