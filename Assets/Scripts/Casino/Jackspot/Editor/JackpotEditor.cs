using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Jackpot))]
public class JackpotEditor : Editor
{
    Jackpot myTarget;
    
    int lastButNum=-1;
    float lastClickTime=-99;
    const float D_CLICK_DELAY = 0.25f; // how long between d-click clicks
    
    private SerializedProperty listSymbols;
    private SerializedProperty tempListResults;


    private void OnEnable()
    {
        myTarget = (Jackpot)target;
        listSymbols = serializedObject.FindProperty("symbols");
        tempListResults = serializedObject.FindProperty("ResultLine");
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);


        if (GUILayout.Button("Game Preview"))
            myTarget.showGamePreview = !myTarget.showGamePreview;
        if (myTarget.showGamePreview)
            DrawGamePreview();
        if (GUILayout.Button("GameLines"))
            myTarget.showGameLines = !myTarget.showGameLines;
        if (myTarget.showGameLines)
            DrawGameLines();
        if (GUILayout.Button("Symbols"))
            myTarget.showSymbols = !myTarget.showSymbols;
        if (myTarget.showSymbols)
            DrawSymbols();
        if (GUILayout.Button("Simulate"))
            myTarget.showSimulate = !myTarget.showSimulate;
        if (myTarget.showSimulate)
            DrawSimulate();

        EditorGUILayout.EndVertical();
    }

    private void DrawGamePreview()
    {
        if(myTarget.currentPresset == null)
        {
            EditorGUILayout.HelpBox("Not default Current Presset Set.", MessageType.Error);
            return;
        }
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("GameLines : "+myTarget.currentPresset.data.Count);
        EditorGUILayout.LabelField("Symbols : "+myTarget.currentPresset.data.Count);
        #region Rows & Colls ints
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Rows: ",GUILayout.Width(40));
        EditorGUILayout.LabelField(myTarget.currentPresset.rows.ToString(), GUILayout.Width(20));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Columns: ", GUILayout.Width(60));
        EditorGUILayout.LabelField(myTarget.currentPresset.cols.ToString(), GUILayout.Width(20));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndHorizontal();
        #endregion


        if (GUILayout.Button("Show Jackpot Grid Preview"))
        {
            myTarget.showGridPreview = !myTarget.showGridPreview;
        }

       
        
        int indexLine = 0;
        if (myTarget.showGridPreview)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            for (int i = 0; i < myTarget.chanceCompleteLine.Count; i++)
            {
                EditorGUILayout.LabelField("Chance Complete Line: ["+i+"], " +myTarget.chanceCompleteLine[i], EditorStyles.helpBox);
            }
            
            EditorGUILayout.LabelField("Total Complete Line: " +myTarget.totalChanceCompleteLine, EditorStyles.helpBox);

            for (int y = 0; y < myTarget.currentPresset.rows ; y++)
            {
                
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                
                    for (int x = 0; x < myTarget.currentPresset.cols; x++)
                    {
                   
                        if (myTarget.resultsGrid != null && myTarget.resultsGrid.Length != 0)
                            EditorGUILayout.LabelField(x + "," + y + ":" + myTarget.resultsGrid[x, y].symbolName.ToString(),
                                EditorStyles.helpBox);
                        else
                            EditorGUILayout.LabelField(x + "," + y, EditorStyles.helpBox);

                        if (y == myTarget.currentPresset.rows - 1)
                        {
                           // EditorGUILayout.LabelField("I:"+indexLine+"CWL :" +myTarget.chanceLines[indexLine], EditorStyles.helpBox);
                            indexLine++;
                        }
                        
                    }
                
             
               
                
                EditorGUILayout.EndHorizontal();
            }

           
            EditorGUILayout.EndVertical();
        }
    

        EditorGUILayout.EndVertical();
    }

    private void DrawSymbols()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Symbos Count : " + myTarget.symbols.Count, EditorStyles.centeredGreyMiniLabel);
        if (GUILayout.Button("Add"))
        {
            myTarget.symbols.Add(null);
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        float totalFrecuencyCount = 0f;
        for (int i = 0; i < myTarget.symbols.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            myTarget.symbols[i] = (JackpotSymbol)EditorGUILayout.ObjectField(myTarget.symbols[i], typeof(JackpotSymbol), true);
            if (myTarget.symbols[i] != null)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Frecuency",GUILayout.Width(20));
                myTarget.symbols[i].frequency = EditorGUILayout.IntField(myTarget.symbols[i].frequency);
                EditorGUILayout.EndHorizontal();
                
                totalFrecuencyCount += myTarget.symbols[i].frequency;
                if (GUILayout.Button("Edit"))
                {
                    EditWindowSymb.Init(myTarget.symbols[i]);
                }

            }
            if (GUILayout.Button("X"))
            {
                myTarget.symbols.Remove(myTarget.symbols[i]);
            }
            EditorGUILayout.EndHorizontal();        
        }
        if (totalFrecuencyCount != 100)
        {
                EditorGUILayout.HelpBox("Frequency doesnt match with 100 percent", MessageType.Error);
        }
        EditorGUILayout.EndVertical();


        EditorGUILayout.EndVertical();

    }

    private void DrawSimulate()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
     /*   EditorGUILayout.LabelField("probabilityNumberPerLine: "+ myTarget.probabilityNumberPerLine.ToString("00.00"));
        EditorGUILayout.LabelField("probabilityNumberColumn: " + myTarget.probabilityNumberColumn.ToString("00.00"));
        EditorGUILayout.LabelField("RTP: " + myTarget.rtp.ToString("00.00"));
        myTarget.symbolsPerColumn = EditorGUILayout.IntField("Symbols Per Column: ", myTarget.symbolsPerColumn);*/
        EditorGUILayout.PropertyField(tempListResults);
        if (GUILayout.Button("Simulate"))
        {
            myTarget.SimulateGame();
        }
       
        EditorGUILayout.EndVertical();
    }

    private void DrawGameLines()
    {
        var previousColor = GUI.backgroundColor ;

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        #region TOP
        var style = StyleEditor.GetStyle(Color.white, TextAnchor.MiddleCenter);

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Game Presets : "+ myTarget.linesPressetData.Count, style);
        if (GUILayout.Button("Add Presset"))
        {
            myTarget.linesPressetData.Add(null);
        }
        #endregion
        EditorGUILayout.EndVertical();

        int horizontalShowMax = 5;
        for (int p = 0; p < myTarget.linesPressetData.Count; p++)
        {
            if(myTarget.linesPressetData[p] == null)
            {
                myTarget.linesPressetData[p] = (LineDataPresset)EditorGUILayout.ObjectField("Presset Data: ", myTarget.linesPressetData[p], typeof(LineDataPresset), true);
            }
            else
            {
                var alpha = 0.2f;
                if (p % 2 == 0)
                    alpha = 0.5f;

                EditorGUILayout.BeginVertical(StyleEditor.GetBoxWindow(Color.black, alpha));

                #region TOP
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("Lines: " + myTarget.linesPressetData[p].data.Count, style);
                EditorGUILayout.EndVertical();
                #endregion

                #region BODY
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                #region Buttons
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                #region BUTTON: Show Hide Delete
                EditorGUILayout.BeginHorizontal();
                GUI.backgroundColor = Color.yellow;
                if (myTarget.linesPressetData[p].showOnEditor == false)
                {
                    if (GUILayout.Button("Show Presset: (" + p + ") [" + myTarget.linesPressetData[p].cols + "," + myTarget.linesPressetData[p].rows + "]"))
                    {
                        myTarget.linesPressetData[p].showOnEditor = true;
                    }
                }
                else
                {
                    if (GUILayout.Button("Hide Presset: (" + p + ") [" + myTarget.linesPressetData[p].cols + "," + myTarget.linesPressetData[p].rows + "]"))
                    {
                        myTarget.linesPressetData[p].showOnEditor = false;
                    }
                }
                #endregion

                #region BUTTON:Set Current Preeset
                var colorCurrentButton = Color.gray;
                var stringCurrentButton = "SetCurrent";
                if (myTarget.linesPressetData[p].isCurrentPresset)
                {
                    colorCurrentButton = Color.green;
                    stringCurrentButton = "Current";
                }
                GUI.backgroundColor = colorCurrentButton;
                if (GUILayout.Button(stringCurrentButton))
                {
                    myTarget.linesPressetData[p].isCurrentPresset = true;
                    myTarget.currentPresset = myTarget.linesPressetData[p];
                    if (myTarget.linesPressetData[p].isCurrentPresset)
                    {
                        foreach (var item in myTarget.linesPressetData)
                        {
                            if (item != myTarget.linesPressetData[p])
                                item.isCurrentPresset = false;
                        }
                    }
                }
                GUI.backgroundColor = previousColor;
                #endregion

                #region BUTTON: Remove
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Remove"))
                {
                    myTarget.linesPressetData[p] = null;
                    myTarget.linesPressetData.RemoveAt(p);
                    return;
                }
                GUI.backgroundColor = previousColor;
                #endregion

                EditorGUILayout.EndVertical();
                #endregion

                EditorGUILayout.EndHorizontal();

              
                if(myTarget.linesPressetData[p].showOnEditor)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    #region DrawInternalData
                    myTarget.linesPressetData[p] = (LineDataPresset)EditorGUILayout.ObjectField("Presset Data: ", myTarget.linesPressetData[p], typeof(LineDataPresset), true);

                    myTarget.linesPressetData[p].cols = EditorGUILayout.IntField("Cols", myTarget.linesPressetData[p].cols);
                    myTarget.linesPressetData[p].rows = EditorGUILayout.IntField("Rows", myTarget.linesPressetData[p].rows);
                    bool enabled = myTarget.linesPressetData[p].data.Count <= 45;
                    GUI.enabled = enabled;
                    if (GUILayout.Button("Add GameLine"))
                    {
                        myTarget.linesPressetData[p].AddNewLine();
                    }
                    GUI.enabled = true;

                    DrawGameLinesData(p);


                    #endregion
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndVertical();
                #endregion

                EditorGUILayout.EndVertical();
            }

           

        }
       
    EditorGUILayout.EndVertical();
    }

    private void DrawGameLinesData(int p)
    {
        EditorGUILayout.BeginHorizontal();
        DrawLineDataSingle(p,0,4);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        DrawLineDataSingle(p,5,9);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        DrawLineDataSingle(p,10,14);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        DrawLineDataSingle(p,15,19);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        DrawLineDataSingle(p,20, 24);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        DrawLineDataSingle(p,25, 29);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        DrawLineDataSingle(p, 30, 34);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        DrawLineDataSingle(p, 35, 39);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        DrawLineDataSingle(p, 40, 44);
        EditorGUILayout.EndHorizontal();

    }

    private void DrawLineDataSingle(int p,int begin,int end)
    {

        for (int i = begin; i < myTarget.linesPressetData[p].data.Count; i++)
        {
            var previousColor = GUI.backgroundColor;

            var sizeWidth = 15 * myTarget.linesPressetData[p].cols;
            var sizeHeight = 10 * myTarget.linesPressetData[p].rows + 2;

            EditorGUILayout.BeginVertical(StyleEditor.GetBoxWindow(Color.black, 0.5f), GUILayout.Width(sizeWidth), GUILayout.Height(sizeHeight));
            #region Buttons Show / Delete

            EditorGUILayout.BeginHorizontal();
            if (myTarget.linesPressetData[p].data[i].show)
            {
                if (GUILayout.Button("[-]" + (i+1), GUILayout.Width(sizeWidth / 2)))
                    myTarget.linesPressetData[p].data[i].show = false;
            }
            else
            {
                if (GUILayout.Button("[+]" + (i+1), GUILayout.Width(sizeWidth / 2)))
                    myTarget.linesPressetData[p].data[i].show = true;
            }

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("X", GUILayout.Width(sizeWidth / 2)))
            {
                myTarget.linesPressetData[p].data.RemoveAt(i);
                return;
            }
            GUI.backgroundColor = previousColor;
            EditorGUILayout.EndHorizontal();
            #endregion
            if (myTarget.linesPressetData[p].data[i].show)
            {
                myTarget.linesPressetData[p].data[i].color = EditorGUILayout.ColorField(myTarget.linesPressetData[p].data[i].color, GUILayout.Width(sizeWidth));

           
                for (int y = 0; y < myTarget.linesPressetData[p].rows; y++)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int x = 0; x < myTarget.linesPressetData[p].cols; x++)
                    {
                        var line = myTarget.linesPressetData[p].data[i].line.Find(line => line.x == x && line.y == y);
                        if(line == null)
                        {
                            Debug.Log("Line Null");
                        }
                        else
                        {
                            if (line.value == 1)
                                GUI.backgroundColor = myTarget.linesPressetData[p].data[i].color;

                            if (GUILayout.Button("[" + x + "," + y + "]", GUILayout.Height(10), GUILayout.Width(10)))
                            {
                                if (line.value == 0)
                                    line.value = 1;
                                else
                                    line.value = 0;
                            }
                        }
                      
                        GUI.backgroundColor = previousColor;
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
            if (i == end)
                break;
        }
    }

    private void DrawLineDataSingle(int p, List<LineData> listTarget)
    {
        for (int i = 0; i < listTarget.Count; i++)
        {
            var previousColor = GUI.backgroundColor;

            var sizeWidth = 15 * myTarget.linesPressetData[p].cols;
            var sizeHeight = 10 * myTarget.linesPressetData[p].rows + 2;

            EditorGUILayout.BeginVertical(StyleEditor.GetBoxWindow(Color.black, 0.5f), GUILayout.Width(sizeWidth), GUILayout.Height(sizeHeight));
            #region Buttons Show / Delete

            EditorGUILayout.BeginHorizontal();
            if (listTarget[i].show)
            {
                if (GUILayout.Button("-" + i, GUILayout.Width(sizeWidth / 2)))
                    listTarget[i].show = false;
            }
            else
            {
                if (GUILayout.Button("+" + i, GUILayout.Width(sizeWidth / 2)))
                    listTarget[i].show = true;
            }

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("X", GUILayout.Width(sizeWidth / 2)))
            {
                listTarget.RemoveAt(i);
                return;
            }
            GUI.backgroundColor = previousColor;
            EditorGUILayout.EndHorizontal();
            #endregion
            if (listTarget[i].show)
            {
                listTarget[i].color = EditorGUILayout.ColorField(listTarget[i].color, GUILayout.Width(sizeWidth));

                for (int y = 0; y < myTarget.linesPressetData[p].rows; y++)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int x = 0; x < myTarget.linesPressetData[p].cols; x++)
                    {
                       
                        var line = myTarget.linesPressetData[p].data[i].line.Find(line => line.x == x && line.y == y);
                        if (line.value == 1)
                            GUI.backgroundColor = listTarget[i].color;

                        if (GUILayout.Button("[" + x + "," + y + "]", GUILayout.Height(10), GUILayout.Width(10)))
                        {
                            if (line.value == 0)
                                line.value = 1;
                            else
                                line.value = 0;
                        }
                        GUI.backgroundColor = previousColor;
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();

        }
    }
    private void OnDisable()
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(myTarget);
        foreach (var item in myTarget.linesPressetData)
        {
            UnityEditor.EditorUtility.SetDirty(item);
        }
        foreach (var item in myTarget.symbols)
        {
            UnityEditor.EditorUtility.SetDirty(item);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif
    }
}

public class EditWindowSymb: EditorWindow
{
    private static JackpotSymbol symbol;
    public static void Init(JackpotSymbol symbol)
    {
        EditWindowSymb.symbol = symbol;
        EditWindowSymb window = (EditWindowSymb)EditorWindow.GetWindow(typeof(EditWindowSymb));
      // var windowsPos = window.position;
      //  windowsPos.size = new Vector2(200, 200);
        //window.position = windowsPos;
        window.minSize = new Vector2(300, 300);
        window.maxSize = new Vector2(300, 700);
        window.Show();
    }

    void OnGUI()
    {
        JackpotSymbolEditor editor = (JackpotSymbolEditor)Editor.CreateEditor(symbol, typeof(JackpotSymbolEditor));        
        editor.OnInspectorGUI();

    }

}
