using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootChipController : MonoBehaviour
{
    public int chipValue;
    [SerializeField] private GameObject feedbackSelectedObject;
    public List<GameObject> childs = new List<GameObject>();
    public Vector2 padding;
    public Vector2 offset;

    private bool isCurrentChipSelected;
    public int currentActiveChilds { get; private set; }
    public Roulette roulette;
    private void Update()
    {
        if (roulette.currentBetChip == chipValue)
        {
            if (isCurrentChipSelected == false)
            {
                OnChipSelect();
            }
        }
        else
        {
            if (isCurrentChipSelected == true)
            {
                OnChipDeselect();
            }
        }
    }
    [ContextMenu("GetChilds")]
    public void GetChilds()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            childs.Add(transform.GetChild(i).gameObject);
        }
    }

    [ContextMenu("Order")]
    public void Order()
    {
        for (int i = 0; i < childs.Count; i++)
        {
            childs[i].transform.localPosition = padding;
        }
        var fixOffset = Vector3.zero;
        for (int i = 0; i < childs.Count; i++)
        {
            childs[i].transform.localPosition += fixOffset;
            fixOffset += (Vector3)offset;
        }
    }

    [ContextMenu("Add")]
    public void Add()
    {
        childs[currentActiveChilds].SetActive(true);
        currentActiveChilds++;
    }
    [ContextMenu("Remove")]
    public void Remove()
    {
        childs[currentActiveChilds].SetActive(false);
        currentActiveChilds--;
    }

    public void OnChipSelect()
    {
        isCurrentChipSelected = true;
        feedbackSelectedObject.SetActive(true);
        transform.localScale = Vector3.one * 1.2f;
    }
    public void OnChipDeselect()
    {
        isCurrentChipSelected = false;
        feedbackSelectedObject.SetActive(false);
        transform.localScale = Vector3.one;
    }
}
