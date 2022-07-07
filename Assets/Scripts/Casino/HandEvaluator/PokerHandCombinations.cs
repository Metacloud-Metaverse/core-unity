using System;
using System.Collections.Generic;
using UnityEngine;

public class HandCombinations
{
	public virtual bool IsPossible(PokerCard[] cardsList)
	{
		return false;
	}

	public virtual HandCombinationRank GetRank()
	{
		return HandCombinationRank.None;
	}
	
	/// <summary>
	/// Using kickers, determines the exact rank of this hand
	/// </summary>
	public static int DeterminePerfectRank(PokerCard[] cardsList)
	{

		return 0;
	}

	/// <summary>
	/// For pairs, two pairs and three of a kind. Returns the number found if it fits the parementers.
	/// </summary>
	public PokerNumbers FindRepeatedNumbers(PokerCard[] cardsList, int amount, PokerNumbers ignoreNumber = PokerNumbers.None)
	{
		for (int i = 0; i < cardsList.Length; i++)
		{
			var count = 1;
			var cardA = cardsList[i];
			if (cardA.number == ignoreNumber)
			{
				continue;
			}
			for (int j = i + 1; j < cardsList.Length; j++)
			{
				var cardB = cardsList[j];
				if (cardA.number == cardB.number)
				{
					count++;
					if (count == amount)
					{
						return cardA.number;
					}
				}
			}
		}
		return PokerNumbers.None;
	}
}

public struct PokerCard
{
	public PokerSuit suit;
	public PokerNumbers number;

}

public enum PokerSuit
{
	Hearts,
	Clubs,
	Spades,
	Diamonds,
	UpsideDown
}

public enum PokerNumbers
{
	Two,
	Three,
	Four,
	Five,
	Six,
	Seven,
	Eight,
	Nine,
	Ten,
	Jack,
	Queen,
	King,
	Ace,
	None,
}

public enum HandCombinationRank
{
	None,
	HighCard,
	OnePair,
	TwoPair,
	ThreeOfAKind,
	Straight,
	Flush,
	FullHouse,
	FourOfAKind,
	StraightFlush,
	RoyalFlush
}

public class RoyalFlush : HandCombinations
{
	public override HandCombinationRank GetRank()
	{
		return HandCombinationRank.RoyalFlush;
	}
}

public class StraightFlush : HandCombinations
{
	public override HandCombinationRank GetRank()
	{
		return HandCombinationRank.StraightFlush;
	}
}

public class FourOfAKind : HandCombinations
{ 
	
	public override bool IsPossible(PokerCard[] cardsList)
	{
		var pairType = FindRepeatedNumbers(cardsList, 4);
		if (pairType != PokerNumbers.None)
		{
			return true;
		}
		return false;
	}
	
	public override HandCombinationRank GetRank()
	{
		return HandCombinationRank.FourOfAKind;
	}
}

public class FullHouse : HandCombinations
{
	public override bool IsPossible(PokerCard[] cardsList)
	{
		var pairType = FindRepeatedNumbers(cardsList, 3);
		if (pairType != PokerNumbers.None)
		{
			pairType = FindRepeatedNumbers(cardsList, 2, pairType);
			if (pairType != PokerNumbers.None)
			{
				return true;
			}
		}
		return false;
	}
	
	public override HandCombinationRank GetRank()
	{
		return HandCombinationRank.FullHouse;
	}
}

public class Flush : HandCombinations
{
	public override bool IsPossible(PokerCard[] cardsList)
	{
		for (int i = 0; i < 4; i++)
		{
			var suitChecked = (PokerSuit) i;
			var count = 0;
			foreach (var card in cardsList)
			{
				if (card.suit == suitChecked)
				{
					count++;
					if (count == 5)
					{
						return true;
					}
				}
			}
		}

		return false;
	}
	
	public override HandCombinationRank GetRank()
	{
		return HandCombinationRank.Flush;
	}
}

public class Straight : HandCombinations
{
	public override bool IsPossible(PokerCard[] cardsList)
	{ 
		//TODO: Ace not implemented
		for (int i = (int)PokerNumbers.Ten - 1; i >= 0; i--)
		{
			if (ContainsNumber(cardsList, (PokerNumbers) i))
			{
				var possible = true;
				for (int j = 1; j < 4; j++)
				{
					if(ContainsNumber(cardsList, (PokerNumbers) i+j) == false)
					{
						possible = false;
						break;
					}
				}

				if (possible)
				{
					return true;
				}
			}
			else
			{
				i -= 4; //
			}
		}
		return false;
	}
	public override HandCombinationRank GetRank()
	{
		return HandCombinationRank.Straight;
	}

	public static bool ContainsNumber(PokerCard[] cardsList, PokerNumbers number)
	{
		foreach (var card in cardsList)
		{
			if(number == card.number)
			{
				return true;
			}
		}
		return false;
	}
}

public class ThreeOfAKind : HandCombinations
{
	public override bool IsPossible(PokerCard[] cardsList)
	{
		var pairType = FindRepeatedNumbers(cardsList, 3);
		if (pairType != PokerNumbers.None)
		{
			return true;
		}
		return false;
	}
	
	public override HandCombinationRank GetRank()
	{
		return HandCombinationRank.ThreeOfAKind;
	}
}

public class TwoPair : HandCombinations
{
	public override bool IsPossible(PokerCard[] cardsList)
	{
		var pairType = FindRepeatedNumbers(cardsList, 2);
		if (pairType != PokerNumbers.None)
		{
			pairType = FindRepeatedNumbers(cardsList, 2, pairType);
			if (pairType != PokerNumbers.None)
			{
				return true;
			}
		}
		return false;
	}
	
	public override HandCombinationRank GetRank()
	{
		return HandCombinationRank.TwoPair;
	}
}

public class OnePair : HandCombinations
{
	public override bool IsPossible(PokerCard[] cardsList)
	{
		var pairType = FindRepeatedNumbers(cardsList, 2);
		if (pairType != PokerNumbers.None)
		{
			return true;
		}
		return false;
	}
	
	public override HandCombinationRank GetRank()
	{
		return HandCombinationRank.OnePair;
	}
}

public class HighCard : HandCombinations
{
	public override bool IsPossible(PokerCard[] cardsList)
	{
		var cardType = PokerNumbers.None;
		foreach (var card in cardsList)
		{
			if (card.number > cardType)
			{
				cardType = card.number;
			}
		}

		return false;
	}
	
	public override HandCombinationRank GetRank()
	{
		return HandCombinationRank.HighCard;
	}
}