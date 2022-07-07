using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RouletteStatics
{
    public int totalNumbers;
    public int oneTo36TotalNumbers
    {
        get
        {
            return totalNumbers - totalZeroNumbers;
        }
    }

    //Black Red Vars
    private int totalRedNumbers;
    private int totalBlackNumbers;
    private int totalBlackRedNumbers;
    public int redPercent;
    public int blackPercent;

    //Odd Even Vars
    private int totalOddNumbers;
    private int totalEvenNumbers;
    private int totalOddEvenNumbers;
    public int oddPercent;
    public int evenPercent;

    //LowHightVars
    private int totalLowNumbers;
    private int totalHighNumbers;
    public int totalLowHighNumbers;
    public int lowPercent;
    public int highPercent;


    //Zero Vars
    public int totalZeroNumbers;
    public int numberOffBallSinceLastZero;


    public void AddNumber(int number)
    {
        ColorsStatics(number);

        OddEvenStatics(number);

        ZeroStatics(number);

        LowHighStatics(number);

        totalNumbers++;
    }

    private void LowHighStatics(int number)
    {
        if (number != 0)
        {
            if (number <= 18)
                totalLowNumbers++;
            else
                totalHighNumbers++;
            totalLowHighNumbers++;

            lowPercent = totalLowNumbers * 100 / totalLowHighNumbers;
            highPercent = totalHighNumbers * 100 / totalLowHighNumbers;
        }
    }

    private void ZeroStatics(int number)
    {
        if (number == 0)
        {
            numberOffBallSinceLastZero = 0;
            totalZeroNumbers++;
        }
        else
        {
            numberOffBallSinceLastZero++;
        }
    }

    private void OddEvenStatics(int number)
    {
        if (number != 0)
        {
            if (number % 2 == 0)
                totalEvenNumbers++;

            else
                totalOddNumbers++;

            totalOddEvenNumbers++;

            oddPercent = totalOddNumbers* 100 / totalOddEvenNumbers;
            evenPercent = totalEvenNumbers* 100 / totalOddEvenNumbers;

        }
    }

    private void ColorsStatics(int number)
    {
        var color = ColorsDict.GetStringColor(number);
        if (color == "Red")
        {
            totalRedNumbers++;
            totalBlackRedNumbers++;
        }
        else if (color == "Black")
        {
            totalBlackNumbers++;
            totalBlackRedNumbers++;
        }

        blackPercent = totalBlackNumbers * 100 / totalBlackRedNumbers;
        redPercent = totalRedNumbers * 100 / totalBlackRedNumbers;
    }
}
