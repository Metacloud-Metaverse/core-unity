using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jackpot_PressetLindaGameData", menuName = "ScriptableObject/Casino/Jackspot/New PressetLindaGameData")]
public class LineDataPresset : ScriptableObject
{
    public bool isCurrentPresset;
    public int cols;
    public int rows;
    public List<LineData> data = new List<LineData>();


    public void AddNewLine()
    {
        var newData = new LineData(cols, rows);
        data.Add(newData);
    
    }
    #region EditorVars
    public bool showOnEditor;
    #endregion
}
