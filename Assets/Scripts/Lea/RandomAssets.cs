using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class RandomAssets : MonoBehaviour
{

    public BoxCollider spawnVolume;
    public Vector3 bounds;
    private void Awake()
    {
        if(!spawnVolume)
        {
            Debug.Log("No spawnVolume (BoxCollider) attached.");
        }

        Debug.Log(spawnVolume.size);
        bounds = new Vector3(spawnVolume.size.x, spawnVolume.size.y, spawnVolume.size.z);
        UpdateBounds();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void UpdateBounds()
    {
        bounds = new Vector3(spawnVolume.size.x, spawnVolume.size.y, spawnVolume.size.z);
    }
}

