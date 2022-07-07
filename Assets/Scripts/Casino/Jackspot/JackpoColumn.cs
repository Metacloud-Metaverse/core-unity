using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackpoColumn : MonoBehaviour
{
    public bool isRollActive = true;
    public float speed = 1000;
    public List<JackpotSymbolUI> symbolsUI = new List<JackpotSymbolUI>();

    public RectTransform posUp;
    public RectTransform posToGoUp;
    public RectTransform firstPos;
    public RectTransform lastPos;

    public List<Vector3> initPositions = new List<Vector3>();


    [ContextMenu("Set Init Positions")]
    public void SetInitPositions()
    {
        for (int i = 0; i < symbolsUI.Count; i++)
        {
            initPositions.Add(symbolsUI[i].transform.position);
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
