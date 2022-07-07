using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Casino;
using UnityEngine;
using TMPro;


public class RouletteInterface : MonoBehaviour
{
    [HideInInspector]
    public Roulette roulette;
    
    /// <summary>
    /// For display only, client countdown
    /// </summary>
    private float timer;

    public TextMeshProUGUI walletInfo;
    public TextMeshProUGUI announceText; //TODO: strip and reuse globally
    
    [SerializeField]private TextMeshProUGUI timerToPlayText;

    public TextMeshProUGUI hotNumbersText;
    public TextMeshProUGUI coldNumbersText;

    public List<UILastNumberSlot> lastNumbers;
    
    public RouletteUIMSGText msgText;

    public RouletteUIStatics UIStatics;

    public void Awake()
    {
        roulette = GetComponent<Roulette>();
        UIStatics.UpdateTexts();
        timer = 0;
    }

    public void Update()
    {
        if (roulette.spinner.isActive == false)
        {
            walletInfo.text = CasinoManager.Instance.localPlayerWallet.GetWalletString();
        }
        if (timer > 1)
        {
            timer -= Time.deltaTime;
            roulette.rouletteInterface.UpdateTimer((int)Mathf.Floor(timer));
        }
    }

    public void ToggleBets(bool betsClosed)
    {
        if (betsClosed)
        {
            SoundControllerRoulette.Instance.PlayClip(roulette.soundsHolder.nomore);
            roulette.rouletteInterface.UpdateTimer(0);
        }
        else
        {
            SoundControllerRoulette.Instance.PlayClip(roulette.soundsHolder.placeyourBets);
            timer = roulette.betsOpenTime;
        }
    }
    
    
    public void UpdateTimer(int time)
    {
        timerToPlayText.text = time.ToString("00");
    }

    public void UpdateDataText(string newText)
    {
        //walletInfo.text = $"Funds : $ {founds} - Available : $ {availableFounds} - Current Bet : $ {currentBetFounds}\n Win : $ {winBet} - Lose : $ {loseBet}";
        walletInfo.text = newText;
    }

    public void DisplayLastWinningNumbers(List<int> winningNumbers)
    {

        //Shows the last 4 or so numbers
        for (int i = 0; i < lastNumbers.Count; i++)
        {
            if (winningNumbers.Count <= i)
            {
                break;
            }
            var uiLastNumberSlot = lastNumbers[i];
            var wonNumber = winningNumbers[winningNumbers.Count - 1 - i];
            uiLastNumberSlot.Set(wonNumber);
        }
        
        //shows the the highest and lowest percentages on the entire sent list of the winning numbers
        var repeatedNumbers = new Dictionary<int, int>();
        foreach (var winningNumber in winningNumbers)
        {
            if (repeatedNumbers.ContainsKey(winningNumber))
            {
                repeatedNumbers[winningNumber] = repeatedNumbers[winningNumber] + 1;
            }
            else
            {
                repeatedNumbers.Add(winningNumber, 1);   
            }
        }
        
        var hots = repeatedNumbers.OrderByDescending(x => x.Value).Take(3).ToList();
        var colds = repeatedNumbers.OrderBy(x => x.Value).Take(3).ToList();

        SetHotColdTexts(hotNumbersText, hots, winningNumbers.Count);
        SetHotColdTexts(coldNumbersText, colds, winningNumbers.Count);
    }

    private void SetHotColdTexts(TextMeshProUGUI numbersText, List<KeyValuePair<int, int>> numbersToShow, float totalNumbers)
    {
        numbersText.text = "";
        foreach (var keyValuePair in numbersToShow)
        {
            numbersText.text += $"<size=18><color={ColorsDict.GetHexColor(keyValuePair.Key)}>";
            numbersText.text += $"{keyValuePair.Key}    </color></size>";
        }
        numbersText.text += "\n";
        foreach (var keyValuePair in numbersToShow)
        {
            var percentage = keyValuePair.Value / totalNumbers * 100; 
            numbersText.text += $"<size=11> {percentage:00.00} %  </size>";
        }
    }
}