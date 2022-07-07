using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class PoolList<T> : IPoolList<T>
{
    public T objPool;
    public int AmountPool;
    private List<T> list = new List<T>();
    private Queue<T> aviableObjects = new Queue<T>();
    public Transform parent;
    public T this[int key]
    {
        get => list[key];
        set => list[key] = value;
    }
    public int Count
    {
        get {
            return list.Count;
        }
    }
    public T First
    {
        get
        {
            return list[0];
        }
    }

    public T FirstFree
    {
        get
        {
            for (int i = 0; i < list.Count; i++)
            {
                var obj = list[i] as Component;
                if (obj)
                {
                    if (obj != null && obj.gameObject.activeInHierarchy == false)
                        return list[i];
                }
                else
                {
                    var GObject = list[i] as GameObject;
                    if (GObject != null && GObject.activeInHierarchy == false)
                        return list[i];
                }

            }
            GeneratePool(parent);
            for (int i = 0; i < list.Count; i++)
            {
                var obj = list[i] as Component;
                if (obj != null && obj.gameObject.activeInHierarchy == false)
                    return list[i];
            }
            return default;
        }
    }
    public T Last
    {
        get
        {
            return list[list.Count];
        }
    }
    public void Add(T value)
    {
        throw new System.NotImplementedException();
    }

    public void Clear()
    {
        list.Clear();
    }

    public bool Contains(T value)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Equals(value))
            {
                return true;
            }
        }
        return false;
    }
    public void GeneratePool()
    {
        var CheckGObject = objPool as GameObject;
        if (CheckGObject)
        {
            Debug.LogError("Pool is generic and GameObject is a Interface, use transform instead");
            return;
        }
        var objectPoolasComponent = objPool as Component;
        if (objectPoolasComponent)
        {
            for (int i = 0; i < AmountPool; i++)
            {
                var ob = GameObject.Instantiate(objectPoolasComponent, parent);
                ob.gameObject.SetActive(false);
                list.Add(ob.GetComponent<T>());
            }
        }
    }
    public void GeneratePool(Transform parent)
    {
        var CheckGObject = objPool as GameObject;
        if (CheckGObject)
        {
            Debug.LogError("Pool is generic and GameObject is a Interface, use transform instead");
            return;
        }
        this.parent = parent;
        var objectPoolasComponent = objPool as Component;
        if (objectPoolasComponent)
        {
            for (int i = 0; i < AmountPool; i++)
            {
                var ob = GameObject.Instantiate(objectPoolasComponent, parent);
                ob.gameObject.SetActive(false);
                list.Add(ob.GetComponent<T>());
            }
        }
    }
    public void GeneratePool(T Object, Transform parent)
    {
        objPool = Object;
        var CheckGObject = objPool as GameObject;
        if (CheckGObject)
        {
            Debug.LogError("Pool is generic and GameObject is a Interface, use transform instead");
            return;
        }
        this.parent = parent;
        var objectPoolasComponent = objPool as Component;
        if (objectPoolasComponent)
        {
            for (int i = 0; i < AmountPool; i++)
            {
                var ob = GameObject.Instantiate(objectPoolasComponent, parent);
                ob.gameObject.SetActive(false);
                list.Add(ob.GetComponent<T>());
            }
        }
    }
    public void GeneratePool(Transform parent,int amount)
    {
        this.AmountPool = amount;
        GeneratePool(parent);
    }


    public T PoolInstantiate(Vector3 position, Quaternion rotation)
    {
        var free = FirstFree;
        if (free != null)
        {
            var obj = free as Component;
            if (obj != null)
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.gameObject.SetActive(true);
                return free;
            }
        }
        return default;
    }

    public void Remove(T value)
    {
        
        list.Remove(value);
    }

    public void Remove(int index)
    {
        list.RemoveAt(index);
    }

}
