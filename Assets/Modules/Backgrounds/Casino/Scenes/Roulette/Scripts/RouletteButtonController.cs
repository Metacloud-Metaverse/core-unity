using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteButtonController : MonoBehaviour
{
    private bool prevSpinerStatus = true;
    [SerializeField] private UnityEngine.UI.Button button;
    public Roulette roulette;
    void Update()
    {
        if (roulette.spinner.isActive != prevSpinerStatus)
        {
            prevSpinerStatus = roulette.spinner.isActive;
            button.interactable = !roulette.spinner.isActive;
        }
    }
}
