using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTest : MonoBehaviour
{
    public PoolList<Transform> pool = new PoolList<Transform>();


    private void Awake()
    {
        GeneratePool();
    }
    [ContextMenu("GeneratePool")]
    public void GeneratePool()
    {
        pool.GeneratePool(null);
    }

    public void UsePool()
    {
        pool.FirstFree.gameObject.SetActive(true);
    }
}
