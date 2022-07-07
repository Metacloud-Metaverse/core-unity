using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorsDict
{
    public static Dictionary<int, Color> colorsRoulette = new Dictionary<int, Color>();

    public static Color red = Color.red;
    public static Color black = Color.black;
    public static Color green = Color.green;
    public static void Init()
    {
        CreateDict();
    }

    private static void CreateDict()
    {
        colorsRoulette.Add(0, green);
        colorsRoulette.Add(1, red);
        colorsRoulette.Add(2, black);
        colorsRoulette.Add(3, red);
        colorsRoulette.Add(4, black);
        colorsRoulette.Add(5, red);
        colorsRoulette.Add(6, black);
        colorsRoulette.Add(7, red);
        colorsRoulette.Add(8, black);
        colorsRoulette.Add(9, red);
        colorsRoulette.Add(10, black);
        colorsRoulette.Add(11, black);
        colorsRoulette.Add(12, red);
        colorsRoulette.Add(13, black);
        colorsRoulette.Add(14, red);
        colorsRoulette.Add(15, black);
        colorsRoulette.Add(16, red);
        colorsRoulette.Add(17, black);
        colorsRoulette.Add(18, red);
        colorsRoulette.Add(19, red);
        colorsRoulette.Add(20, black);
        colorsRoulette.Add(21, red);
        colorsRoulette.Add(22, black);
        colorsRoulette.Add(23, red);
        colorsRoulette.Add(24, black);
        colorsRoulette.Add(25, red);
        colorsRoulette.Add(26, black);
        colorsRoulette.Add(27, red);
        colorsRoulette.Add(28, black);
        colorsRoulette.Add(29, black);
        colorsRoulette.Add(30, red);
        colorsRoulette.Add(31, black);
        colorsRoulette.Add(32, red);
        colorsRoulette.Add(33, black);
        colorsRoulette.Add(34, red);
        colorsRoulette.Add(35, black);
        colorsRoulette.Add(36, red);
    }

    public static string GetHexColor(int number)
    {
        var c = GetColor(number);
        return  string.Format("#{0:X2}{1:X2}{2:X2}", ToByte(c.r), ToByte(c.g), ToByte(c.b));
    }
    private static byte ToByte(float f)
    {
        f = Mathf.Clamp01(f);
        return (byte)(f * 255);
    }
    public static Color GetColor(int number)
    {
        return colorsRoulette[number];
    }
    public static string GetStringColor(int number)
    {
        var color = colorsRoulette[number];
        if (color == red)
            return "Red";
        else if (color == black)
            return "Black";
        else
            return "Green";
    }
}
