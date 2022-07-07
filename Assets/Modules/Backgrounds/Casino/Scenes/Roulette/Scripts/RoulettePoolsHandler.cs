using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoulettePoolsHandler 
{
    public PoolList<Chip> chip10 = new PoolList<Chip>();
    public PoolList<Chip> chip25 = new PoolList<Chip>();
    public PoolList<Chip> chip50 = new PoolList<Chip>();
    public PoolList<Chip> chip100 = new PoolList<Chip>();
    public PoolList<Chip> chip500 = new PoolList<Chip>();
    public PoolList<Chip> chip1000 = new PoolList<Chip>();
    public Roulette roulette;
  
    public Chip GetSleepChip(int value)
    {
        var chipValue = value;
        if (chipValue == 10)
        {
            return chip10.FirstFree;
        }
        else if (chipValue == 25)
        {
            return chip25.FirstFree;
        }
        else if (chipValue == 50)
        {
            return chip50.FirstFree;
        }
        else if (chipValue == 100)
        {
            return chip100.FirstFree;
        }
        else if (chipValue == 500)
        {
            return chip500.FirstFree;
        }
        else if (chipValue == 1000)
        {
            return chip1000.FirstFree;
        }
        else
        {
            return null;
        }
    }
    public void GeneratePools(Roulette r)
    {
        roulette = r;
        chip10.GeneratePool();
        chip25.GeneratePool();
        chip50.GeneratePool();
        chip100.GeneratePool();
        chip500.GeneratePool();
        chip1000.GeneratePool();

        for (int i = 0; i < chip10.Count; i++)
        {
            //Debug.Log(chip10[i]);
            chip10[i].roulette = roulette;
        }
        for (int i = 0; i < chip25.Count; i++)
        {
            chip25[i].roulette = roulette;
        }
        for (int i = 0; i < chip50.Count; i++)
        {
            chip50[i].roulette = roulette;
        }
        for (int i = 0; i < chip100.Count; i++)
        {
            chip100[i].roulette = roulette;
        }
        for (int i = 0; i < chip500.Count; i++)
        {
            chip500[i].roulette = roulette;
        }
        for (int i = 0; i < chip1000.Count; i++)
        {
            chip1000[i].roulette = roulette;
        }
        
    }
}
