using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerWallet
{
    [Header("Data")]
    public int userID;
    public ConnectedPlayer connectedPlayer; // <- TODO: temp, remove
    public string color;
    [Header("Amounts")]
    public int totalFunds = 1000;

    public int availableFunds;
    public int bettedFounds;
    public int lastBetLosses;
    public int lastBetEarnings;
    
    public PlayerWallet(int value)
    {
        totalFunds = value;
        availableFunds = value;
    }
    public bool CanBet(int amountBet)
    {
        if (amountBet <= availableFunds)
        {
            return true;
        }
        return false;
    }
    public string GetWalletString()
    {
        return $"Funds: ${totalFunds} - Available: ${availableFunds} - Current Bet: ${bettedFounds}\nWon: ${lastBetEarnings} - Lost: ${lastBetLosses}";
    }
    
    public void OnBetWin(int amount)
    {
        lastBetEarnings = amount;
        totalFunds += amount;
        availableFunds += amount;
    }

    public void DiscountCurrentBet(int amount)
    {
        totalFunds -= amount;
        lastBetLosses += amount;
        availableFunds -= amount;
    }
    
    public void ClearRound()
    {
        lastBetLosses = 0;
        lastBetEarnings = 0;
        bettedFounds = 0;
        availableFunds = totalFunds;
    }
    
    public void AddBet(int chipValue)
    {        
        availableFunds -= chipValue;
        bettedFounds += chipValue;
    }
    public void RemoveBet(int chipValue)
    {
        bettedFounds -= chipValue;
        availableFunds += chipValue;
    }
}

public struct RouletteBet
{
    public int value;
    public RoulettePosition roulettePosition;
    public PlayerWallet playerWallet;
}
