using UnityEngine;


	[CreateAssetMenu(fileName = "New RoulettePosition", menuName = "ScriptableObject/Casino/RoulettePosition")]
	public class RoulettePosition : ScriptableObject
	{
		public RewardMultiplier rewardMultiplier;
		public int[] includedNumbers;
		public int ID; //for networking
	}
	
	public enum RewardMultiplier
	{
		SINGLE = 36,
		SPLIT = 18,
		STREET = 12,
		CORNER = 9,
		SIXLINE = 6,
		COLUMN = 3,
		COLOR = 2,
		SNAKE
	}

