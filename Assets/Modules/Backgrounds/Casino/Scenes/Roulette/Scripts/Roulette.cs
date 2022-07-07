using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Linq;
using Casino;
using Messages.Server;
using Mirror;
using TMPro;
using Debug = UnityEngine.Debug;

/*

1) hacer que el sistema cuando se pierde funcione bien
2) hacer los cold numbers

*/

public class Roulette : NetworkBehaviour
{
    public bool betsClosed;
    public RouletteStatics statics;
    private List<RouletteBet> currentBets = new List<RouletteBet>();
    private List<int> lastResults = new List<int>();
		
    public readonly float betsClosedTime = 7;
    public readonly float betsOpenTime = 15;
    
    private NetworkIdentity networkIdentity;
    [HideInInspector] public RouletteInterface rouletteInterface;
		
    private int DEBUGbetValue = 2;

    public bool isManualErasing;
    public int currentBetChip { get; private set; }

    public RouletteSoundsHolder soundsHolder;
    public SpinerResultAudio audioResult;
    private int timer = 30;

    public FeedbackPickedNumber UI;
    public RectUIMouseChecker mouse;
    public RouletteSpiner spinner;

    public List<RouletteCommand> commandBuffer = new List<RouletteCommand>();
    public Dictionary<int, RouletteInputUI> allTableInputs = new Dictionary<int, RouletteInputUI>();
    

    [Header("Pool Chips")]
    public RoulettePoolsHandler pools;
    
    public bool almosOneChipWon;
    private float _counterTimer;
    private void Awake()
    {
        almosOneChipWon = false;
        networkIdentity = GetComponent<NetworkIdentity>();
        rouletteInterface = GetComponent<RouletteInterface>();
        foreach (var rouletteInputUI in FindObjectsOfType<RouletteInputUI>())
        {
            if (rouletteInputUI.type == TypeInputRoulette.Number)
            {
                allTableInputs.Add(rouletteInputUI.roulettePosition.includedNumbers[0], rouletteInputUI);
            }
        }       
        #region Initializate Components
        
        UI.Init();
        pools.GeneratePools(this);
        #endregion
    }
    
    #region Buttons UI
    
    public void OnButtonManualEarsing()
    {
        if (spinner.isActive)
            return;
        isManualErasing = !isManualErasing;
    }
    public void OnButtonSelectAmountBeet(int number)
    {
        currentBetChip = number;
    }
    public void OnButtonSpin()
    {
        if (spinner.isActive)
            return;
    }
    public void OnButtonUndoBeet()
    {
        if (spinner.isActive)
            return;
        Undo();
    }
    public void OnButtonClear()
    {
        if (spinner.isActive)
            return;
        StartCoroutine(ClearCorrutine(false));
    }
    #endregion

    #region Command Pattern
    public void ProccesCommand(RouletteCommand command)
    {
        commandBuffer.Add(command);
        command.Execute();
    }

    public void Undo()
    {
        if (spinner.isActive)
            return;
        if (commandBuffer.Count == 0)
            return;        
        

        var command = commandBuffer[commandBuffer.Count-1];
        RouletteRemoveBetMessage.Send(this, command.inputUI.roulettePosition, command.chipValue);

        command.Undo();
        commandBuffer.Remove(command);
    }
    public void ManualUndo(RouletteCommand command)
    {
        RouletteRemoveBetMessage.Send(this, command.inputUI.roulettePosition, command.chipValue);
        command.Undo();
        commandBuffer.Remove(command);
    }
    private IEnumerator ClearCorrutine(bool gameClear)
    {
        for (int i = 0; i < commandBuffer.Count; i++)
        {
            if(gameClear == false)
                RouletteRemoveBetMessage.Send(this, commandBuffer[i].inputUI.roulettePosition, commandBuffer[i].chipValue);

            commandBuffer[i].Undo();
            yield return Yielders.Seconds(0.001f);
        }
        commandBuffer.Clear();
        yield return Yielders.Seconds(1.5f);
        
        currentBets.Clear();
    }
    
    #endregion

    
    public void OnGetNumberFromSpinner(int winnerNumber)
    {
        var rouletteInputUI = allTableInputs[winnerNumber];
        UI.SetFeedbackPickedNumberPosition(rouletteInputUI.transform);
        StartCoroutine(ClearRound());
    }

    private IEnumerator ClearRound()
    {
        yield return Yielders.Seconds(2.1f);
        spinner.isActive = false;
        StartCoroutine(ClearCorrutine(true));
        UI.SetFeedbackPickedNumberDisabled();
    }

    private void Update()
    {
        if (networkIdentity.isServer)
        {
            if (betsClosed)
            {
                if (_counterTimer <= 0)
                {
                    betsClosed = false;
                    _counterTimer = betsOpenTime;
                    RouletteBetsClosedMessage.Send(this, betsClosed);
                }
            }
            else
            {
                if (_counterTimer <= 0)
                {
                    SpinRoulette();
                    betsClosed = true;
                    _counterTimer = betsClosedTime;
                    almosOneChipWon = false;
                    RouletteBetsClosedMessage.Send(this, betsClosed);
                }
            }
            _counterTimer -= Time.deltaTime;
        }
    }

    
    public void SpinRoulette()
    {
        SoundControllerRoulette.Instance.PlayClip(soundsHolder.nomore);

        var result = Random.Range(0, 36);
        lastResults.Add(result);
        statics.AddNumber(result);
        rouletteInterface.UIStatics.UpdateTexts();
        if (lastResults.Count > 100)
        {
            lastResults.RemoveAt(0);
        }
			
        var playerList = new List<PlayerWallet>(); //TODO: gc
        foreach (var rouletteBet in currentBets)
        {
            var playerWallet = rouletteBet.playerWallet;

            if(rouletteBet.roulettePosition.includedNumbers.Contains(result))
            {
                almosOneChipWon = true;

                playerWallet.OnBetWin((rouletteBet.value * (int)rouletteBet.roulettePosition.rewardMultiplier));
            }
            if (playerList.Contains(playerWallet) == false)
            {
                playerList.Add(playerWallet);
                playerWallet.ClearRound();
            }
            playerWallet.DiscountCurrentBet(rouletteBet.value);
        }
        foreach (var playerWallet in playerList)
        {
            UpdateCasinoChipsMessage.Send(playerWallet.connectedPlayer, playerWallet);
        }
        RouletteResultNumberMessage.Send(this, result);
        RouletteLastWinningNumbersMessage.Send(this, lastResults);
    }

    public void ServerPlaceBet(ConnectedPlayer connectedPlayer, int value, int positionID)
    {
        var playerWallet = CasinoManager.Instance.AllWallets[connectedPlayer.networkConnection.connectionId];
        //Debug.Log("TEST LLEGO EL MSG");
        if (playerWallet.CanBet(value))
        {
            var rouletteBet = new RouletteBet();
            rouletteBet.value = value;
            rouletteBet.roulettePosition = CasinoManager.Instance.AllPositionsByID[positionID];
            rouletteBet.playerWallet = playerWallet;
            currentBets.Add(rouletteBet);

            //    playerWallet.funds -= value;
            playerWallet.AddBet(value);
            //rouletteInterface.walletInfo.text = playerWallet.GetWalletString();
            rouletteInterface.msgText.SetText("<color=white>Placed Bet: <b> -"+value.ToString() +"</b></color>");
            UpdateCasinoChipsMessage.Send(playerWallet.connectedPlayer, playerWallet);
            
        }
        else
        {
            rouletteInterface.msgText.SetText("<color=red>CANT PLACE BET: <b> "+value.ToString() + ",Funds: "+playerWallet.totalFunds+ "</b></color>");

        }
    } 
    public void ServerRemoveBet(ConnectedPlayer connectedPlayer, int value, int positionID)
    {
        var playerWallet = CasinoManager.Instance.AllWallets[connectedPlayer.networkConnection.connectionId];
        
       /* var rouletteBet = new RouletteBet();
        rouletteBet.value = value;
        rouletteBet.roulettePosition = CasinoManager.Instance.AllPositionsByID[positionID];
        rouletteBet.playerWallet = playerWallet;*/
        var r = currentBets.Find(x => x.value == value && x.playerWallet ==playerWallet);
        currentBets.Remove(r);

        
        playerWallet.RemoveBet(value);
        //rouletteInterface.walletInfo.text = playerWallet.GetWalletString();

        rouletteInterface.msgText.SetText("<color=white>Removed Bet: <b> +"+value.ToString() +"</b></color>");

 

        UpdateCasinoChipsMessage.Send(playerWallet.connectedPlayer, playerWallet);


    }
  
}
