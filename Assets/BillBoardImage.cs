using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardImage : MonoBehaviour
{

    public GameObject boardBase;
    public Material imageMaterial;
    public Texture2D image;
    public bool hasBase = false;



    private void Awake()
    {
        boardBase.SetActive(false);
        

        if (hasBase)
        {
            boardBase.SetActive(true);
        }
    }

    private void Start()
    {
        imageMaterial.SetTexture("_BaseMap", image);
    }
}
