using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Jackpot : MonoBehaviour
{
    public List<JackpotSymbol> symbols = new List<JackpotSymbol>();
    public List<float> chanceCompleteLine = new List<float>();
    public float totalChanceCompleteLine;
    
    public int symbolsPerColumn = 7;
    
    public float chanceGetWild = 0.1f;

    public float probabilityNumberPerLine;
    public float probabilityNumberColumn;
    public float rtp;
    //Editor Vars
    public bool showGamePreview;
    public bool showGridPreview;
    public bool showSymbols;
    public bool showSimulate;
    public bool showGameLines;

    public JackpotSymbol[,] resultsGrid ;
    public List<LineDataPresset> linesPressetData = new List<LineDataPresset>();

    public LineDataPresset currentPresset;
    
    public void SimulateGame()
    {
        if(currentPresset == null)
        {
            Debug.Log("not current Presset");
            return;
        }
        chanceCompleteLine.Clear();
       /* totalChanceCompleteLine = 0;

        var comodinCount = 0;
        var comodinExist = symbols.Find(x => x.type == SymbolType.Wild);
        if (comodinExist)
        {
            comodinCount = comodinExist.frequency;
        }
        foreach (var currentSymbol in symbols)
        {
            var chance = ((float)currentSymbol.frequency + (float)comodinCount) / (float)100;
            Debug.Log("Symbol :" + chance);
            if (currentSymbol.type == SymbolType.Wild)
                chance = (float)currentSymbol.frequency / (float)100;

            var chanceWinLine = chance;

            for (int i = 0; i < currentPresset.cols - 1; i++)
            {
                chanceWinLine = chanceWinLine * (chance);
                // Debug.Log($"chance win{chanceWinLine}");
            }
            chanceCompleteLine.Add((chanceWinLine * 100f));
            totalChanceCompleteLine += chanceWinLine * 100f;
        }

        var lineChance = totalChanceCompleteLine;
        for (int i = 0; i < currentPresset.rows; i++)
        {
            lineChance = lineChance * (lineChance / 100);
        }*/

        resultsGrid = new JackpotSymbol[currentPresset.cols, currentPresset.rows];
        var listResult = new List<JackpotSymbol>();
        for (int x = 0; x < currentPresset.cols; x++)
        {
            for (int y = 0; y < currentPresset.rows; y++)
            {
                foreach (var item in symbols)
                {
                    for (int j = 0; j < item.frequency; j++)
                    {
                        listResult.Add(item);
                    }
                }
                var selectedSymbol = listResult[Random.Range(0, listResult.Count - 1)];
                resultsGrid[x, y] = selectedSymbol;
            }
        }
        CheckWin();


    }
    public List<ResultLinesData> ResultLine = new List<ResultLinesData>();

    public void CheckWin()
    {
        ResultLine.Clear();
        for (int i = 0; i < currentPresset.data.Count; i++)
        {
            ResultLine.Add(new ResultLinesData());
        }
        for (int i = 0; i < currentPresset.data.Count; i++)
        {
            var lineData = currentPresset.data[i];
            List<GridObject> mapLine = new List<GridObject>();
            int currentCheckSymbol = 0;
            
            //check line 
            for (int x = 0; x < currentPresset.cols; x++)
            {
                for (int y = 0; y < currentPresset.rows; y++)
                {
                    var gridObject = lineData.GetGridObject(x, y);
                    if (gridObject.value == 1)
                    {
                        mapLine.Add(gridObject);
                    }
                }
            }
            for (int j = 0; j < mapLine.Count; j++)
            {
                if (j == 0)
                {
                    ResultLine[i].ResultLine.Add(resultsGrid[mapLine[j].x, mapLine[j].y]);
                }
                else
                {

                    if (ResultLine[i].ResultLine[currentCheckSymbol].type == SymbolType.Wild)
                    {
                        currentCheckSymbol++;
                        ResultLine[i].ResultLine.Add(resultsGrid[mapLine[j].x, mapLine[j].y]);                        
                    }
                    else if (ResultLine[i].ResultLine[currentCheckSymbol].name == resultsGrid[mapLine[j].x, mapLine[j].y].name || resultsGrid[mapLine[j].x, mapLine[j].y].type == SymbolType.Wild)
                    {
                        ResultLine[i].ResultLine.Add(resultsGrid[mapLine[j].x, mapLine[j].y]);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (ResultLine[i].ResultLine.Count >= 3)
            {
                ResultLine[i].winLine = true;
                Debug.Log("Win On Line: " + i +", Matchs :"+ ResultLine[i].ResultLine.Count);
            }

        }
       
    }
}

[System.Serializable]
public class ResultLinesData
{
    public List<JackpotSymbol> ResultLine = new List<JackpotSymbol>();
    public bool winLine = false;
}