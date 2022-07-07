using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectUIMouseChecker : MonoBehaviour
{
    public List<RouletteInputUI> rectsTriggered = new List<RouletteInputUI>();
    public Canvas parentCanvas;
    public Transform rootSleepingChips;
    private RectTransform mouseRect;
    public Roulette roulette;

    private void Awake()
    {
    }

    void Start()
    {
        mouseRect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    public void Update()
    {
        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            Input.mousePosition, parentCanvas.worldCamera,
            out movePos);

        Vector3 mousePos = parentCanvas.transform.TransformPoint(movePos);

        //Set fake mouse Cursor
        transform.position = mousePos;

        TriggersMouse();
        if (Input.GetMouseButtonDown(0))
        {
            if (rectsTriggered.Count > 0)
            {
                foreach (var item in rectsTriggered)
                {
                    if (item.type == TypeInputRoulette.Number)
                    {
                        if (roulette.currentBetChip > 0)
                        {
                            var nameRootChip = "Chip" + roulette.currentBetChip;
                            var root = rootSleepingChips.Find(nameRootChip);
                            var chip = root.GetChild(0);

                         //   var command = new RouletteCommand(item,chip.gameObject, Roulette.Instance.currentBetChip,rootSleepingChips);
                       //     Roulette.Instance.ProccesCommand(command);
                        }
                       
                    }
                }
            }
        }
    }

    private void TriggersMouse()
    {
        foreach (var item in roulette.allTableInputs.Values)
        {
            if (item.type == TypeInputRoulette.Number)
            {
                if (item.rect.Overlap(mouseRect) && rectsTriggered.Contains(item) == false)
                {
                    rectsTriggered.Add(item);
                }
            }
            else
            {
                if (item.rect.Contains(mouseRect) && rectsTriggered.Contains(item) == false)
                {
                    rectsTriggered.Add(item);
                }
            }

        }
        for (int i = 0; i < rectsTriggered.Count; i++)
        {
            var item = rectsTriggered[i];
            if (item.type == TypeInputRoulette.Number)
            {
                if (item.rect.Overlap(mouseRect) == false && rectsTriggered.Contains(item))
                {
                    rectsTriggered.Remove(item);
                }
            }
            else
            {
                if (item.rect.Contains(mouseRect) == false && rectsTriggered.Contains(item))
                {
                    rectsTriggered.Remove(item);
                }
            }

        }
    }
}
