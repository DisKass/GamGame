using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsPool
{
    List<GameObject> objectsPool;

    GameObject storedObject;
    GameObject Container;
    int maxCount = -1;
    public ObjectsPool(GameObject storedObject, int maxCount = -1)
    {
        this.maxCount = maxCount;
        Container = new GameObject();
        Container.name = storedObject.name + "Container";
        GameManager.Instance.MoveToScene(Container, "PlayerObjects");
        objectsPool = new List<GameObject>();
        this.storedObject = storedObject;
    }

    public GameObject GetElement()
    {
        if (maxCount >= 0 && GetActiveCount() >= maxCount) return null;
        if (storedObject == null)
        {
            Debug.LogError("Object Pool is not initialized");
            return null;
        }
        for (int i = 0; i < objectsPool.Count; i++)
        {
            if (!objectsPool[i].activeSelf)
            {
                objectsPool[i].SetActive(true);
                return objectsPool[i];
            }
        }

        objectsPool.Add(GameObject.Instantiate(storedObject));
        objectsPool[objectsPool.Count - 1].transform.SetParent(Container.transform);
        return objectsPool[objectsPool.Count - 1];
    }

    public int GetActiveCount()
    {
        int count = 0;
        for (int i = 0; i < objectsPool.Count; i++)
        {
            if (objectsPool[i].activeSelf) count++;
        }
        return count;
    }
}
