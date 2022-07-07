using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RouletteUIStatics : MonoBehaviour
{
    [SerializeField] private GameObject canvasRoot;
    [SerializeField] private GameObject panelRoot;


    [SerializeField] private TextMeshProUGUI redPercentTMP;
    [SerializeField] private TextMeshProUGUI blackPercentTMP;

    [SerializeField] private TextMeshProUGUI oddPercentTMP;
    [SerializeField] private TextMeshProUGUI evenPercentTMP;

    [SerializeField] private TextMeshProUGUI lowPercentTMP;
    [SerializeField] private TextMeshProUGUI highPercentTMP;

    [SerializeField] private TextMeshProUGUI numberOfBallsSinceLastZero;

    [SerializeField] private TextMeshProUGUI zeroNumberCount;
    [SerializeField] private TextMeshProUGUI oneTo36Count;
    [SerializeField] private TextMeshProUGUI totalNumberCounts;



    [SerializeField] private Roulette roulette;


    public void UpdateTexts()
    {
        redPercentTMP.text = roulette.statics.redPercent.ToString("00.00") + "%";
        blackPercentTMP.text = roulette.statics.blackPercent.ToString("00.00") + "%";

        oddPercentTMP.text = roulette.statics.oddPercent.ToString("00.00") + "%";
        evenPercentTMP.text = roulette.statics.evenPercent.ToString("00.00") + "%";

        lowPercentTMP.text = roulette.statics.lowPercent.ToString("00.00") + "%";
        highPercentTMP.text = roulette.statics.highPercent.ToString("00.00") + "%";

        numberOfBallsSinceLastZero.text = roulette.statics.numberOffBallSinceLastZero.ToString("00");

        zeroNumberCount.text = "0: "+roulette.statics.totalZeroNumbers.ToString("00");

        oneTo36Count.text = "1/36: "+roulette.statics.oneTo36TotalNumbers.ToString("00");

        totalNumberCounts.text ="General: "+ roulette.statics.totalNumbers.ToString("00");


    }
    public void ButtonOpen()
    {
      
        canvasRoot.SetActive(true);
        panelRoot.SetActive(true);

    }

    public void ButtonClose()
    {
        canvasRoot.SetActive(false);
        panelRoot.SetActive(false);
    }

}
