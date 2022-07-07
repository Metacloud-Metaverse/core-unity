using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolList <T>
{
    //Cantidad total de elementos
    int Count { get; }
    //Primer elemento de la lista
    T First { get; }
    //
    //Primer elemento libre en la lista
    T FirstFree { get; }
    //Ultimo elemento de la lista
    T Last { get; }

    T PoolInstantiate(Vector3 position,Quaternion rotation);
    //Limpia la lista
    void Clear();

    void GeneratePool(Transform parent);
    //Agrega el valor al principio de la lista
    void Add(T value);
   

    void Remove(T value);

    void Remove(int index);

    //Pregunta si la lista contiene el valor
    bool Contains(T value);
}
