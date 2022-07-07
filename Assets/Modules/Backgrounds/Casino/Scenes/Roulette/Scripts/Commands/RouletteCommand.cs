using System.Collections;
using Messages.Server;
using UnityEngine;

public class RouletteCommand : ICommand
{
    public RouletteInputUI inputUI { get; private set; }
    public Chip chip { get; private set; }
    public int chipValue { get; private set; }
    public Roulette roulette { get; private set; }

    public RouletteCommand(RouletteInputUI inputUI, Chip chip, int chipValue, Roulette roulette)
    {
        this.roulette = roulette;
        this.inputUI = inputUI;
        this.chip = chip;
        this.chipValue = chipValue;
    }
    public void Execute()
    {
        chip.SetPositionToBoard();
    }
    public void Undo()
    {
        inputUI.RemoveBet(this);
        chip.BackToOriginalParent();
        

      //  roulette.RemoveBet(chipValue);
    }

  
}
