using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Casino;
using Messages.Server;

public class RouletteInputUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public Roulette roulette;
    public RoulettePosition roulettePosition;
    public RectTransform rect;
    public Image img;
    public List<RouletteInputUI> affectedInputs = new List<RouletteInputUI>();
    public TypeInputRoulette type = TypeInputRoulette.Number;
    public List<RouletteCommand> chips = new List<RouletteCommand>();
    #region Unity Editor Helper

#if UNITY_EDITOR
    [ContextMenu("FindPositionAssetWithName")]
    public void FindPositionWithName()
    {
        string[] allPositions = System.IO.Directory.GetFiles(Application.dataPath, "*.asset", System.IO.SearchOption.AllDirectories);
        foreach (string matFile in allPositions)
        {
            string assetPath = "Assets" + matFile.Replace(Application.dataPath, "").Replace('\\', '/');
            RoulettePosition position = (RoulettePosition)UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(RoulettePosition));
            if (position)
            {
                Find(position);
            }
        }
    }

    private void Find(RoulettePosition objct)
    {
        if (affectedInputs.Count == objct.includedNumbers.Length)
        {
            for (int i = 0; i < affectedInputs.Count; i++)
            {
                var nameToInt = int.Parse(affectedInputs[i].name);
                if (objct.includedNumbers.Contains(nameToInt) == false)
                {
                    return;
                }
            }
        }
        else
        {
            return;
        }
        roulettePosition = objct;
    }

    [ContextMenu("GetAffectedInputsWithName")]
    public void GetAffectedsInputWithName()
    {
        affectedInputs.Clear();
        var strings = gameObject.name.Split("-");
        foreach (var item in strings)
        {
            affectedInputs.Add(GameObject.Find(item).GetComponent<RouletteInputUI>());
        }
    }
    [ContextMenu("GetComponents")]
    public void GetComponents()
    {
        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

#endif
    #endregion

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var item in affectedInputs)
        {
            var color = item.img.color;
            color.a = 0.5f;
            item.img.color = color;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (var item in affectedInputs)
        {
            var color = item.img.color;
            color.a = 0f;
            item.img.color = color;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (roulette.isManualErasing)
            return;
        if (roulette.spinner.isActive)
            return;
        if(roulette.currentBetChip <= 0)
        {
            roulette.rouletteInterface.msgText.SetText("<color=yellow>Need chose Chip first</color>");
            return;
        }
        if (CanBet(roulette.currentBetChip) == false)
        {
            roulette.rouletteInterface.msgText.SetText("<color=red>CANT PLACE BET: <b> "+ roulette.currentBetChip.ToString() + ",Funds: "+ CasinoManager.Instance.localPlayerWallet.availableFunds+ "</b></color>");
            return;
        }

        var chip = roulette.pools.GetSleepChip(roulette.currentBetChip);
        var command = new RouletteCommand(this,chip, roulette.currentBetChip, roulette);
        AddBet(command);
        //Debug.Log("Place Bet");
        RoulettePlaceBetMessage.Send(roulette, roulettePosition, roulette.currentBetChip);
    }
    public bool CanBet(int amountBet)
    {
        if (amountBet <= CasinoManager.Instance.localPlayerWallet.availableFunds)
        {
            return true;
        }
        return false;
    }
    private void AddBet(RouletteCommand command)
    {
        command.chip.SetChip(command);
        chips.Add(command);
        
        switch (command.chipValue)
        {
            case 10:
                var chips10Check = chips.Where(x => x.chipValue == 10).ToList();
                if (chips10Check.Count == 5)
                {
                    var replaceValue = 50;
                    ReplaceChips(chips10Check, replaceValue);
                    return;
                }                

                break;
            case 25:
                var chips25Check = chips.Where(x => x.chipValue == 25).ToList();
                if (chips25Check.Count == 2)
                {
                    int replaceValue = 50;
                    ReplaceChips(chips25Check, replaceValue);
                    return;
                }
                break;
            case 50:
                var chips50Check = chips.Where(x => x.chipValue == 50).ToList();
                if (chips50Check.Count == 2)
                {
                    int replaceValue = 100;
                    ReplaceChips(chips50Check, replaceValue);
                    return;
                }
                break;
            case 100:
                var chips100Check = chips.Where(x => x.chipValue == 100).ToList();
                if (chips100Check.Count == 5)
                {
                    int replaceValue = 500;
                    ReplaceChips(chips100Check, replaceValue);
                    return;
                }
                break;
            case 500:
                var chips500Check = chips.Where(x => x.chipValue == 500).ToList();
                if (chips500Check.Count == 2)
                {
                    int replaceValue = 1000;
                    ReplaceChips(chips500Check, replaceValue);
                    return;
                }
                break;
        }
        roulette.ProccesCommand(command);
        

    }

    private void ReplaceChips(List<RouletteCommand> chipsList, int replaceValue)
    {
        foreach (var item in chipsList)
        {
            roulette.ManualUndo(item);
        }
        var chipReplace = roulette.pools.GetSleepChip(replaceValue);
        var commandReplace = new RouletteCommand(this, chipReplace, replaceValue,roulette);
        chipReplace.SetChip(commandReplace);
        // roulette.wallet.AddBet(replaceValue);
        RoulettePlaceBetMessage.Send(roulette,roulettePosition,replaceValue);
        roulette.ProccesCommand(commandReplace);
    }

    public void RemoveBet(RouletteCommand chip)
    {
        chips.Remove(chip);
    }


}
