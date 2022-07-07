using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    [SerializeField] float rotationSpeedX = 0f;
    [SerializeField] float rotationSpeedY = 0f;
    [SerializeField] float rotationSpeedZ = 10f;
    [SerializeField] GameObject[] elements;

    void Update()
    {
        transform.Rotate(rotationSpeedX * Time.deltaTime, rotationSpeedY * Time.deltaTime, rotationSpeedZ * Time.deltaTime);
        foreach (var item in elements)
        {
            item.transform.Rotate(Random.Range(.5f, 10f) * Time.deltaTime, Random.Range(.5f, 10f) * Time.deltaTime, Random.Range(.5f, 10f) * Time.deltaTime);
        }
    }
}
