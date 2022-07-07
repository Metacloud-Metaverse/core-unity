using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LineData
{
    //public int[,] line;
    public List<GridObject> line = new List<GridObject>();

    public GridObject GetGridObject(int x, int y)
    {
        return line.Find(gList => gList.x == x && gList.y == y);
    }
    public bool show = true;
    public Color color = Color.green;
    public LineData(int x, int y)
    {
        //  line = new int[x, y];
        for (int _x = 0; _x < x; _x++)
        {
            for (int _y = 0; _y < y; _y++)
            {
                line.Add(new GridObject(_x, _y, 0));
            }
        }
    }
}

[System.Serializable]
public class GridObject
{
    public int x;
    public int y;
    public int value;
   
    public GridObject(int x,int y,int value)
    {
        this.x = x;
        this.y = y;
        this.value = value;
    }
}