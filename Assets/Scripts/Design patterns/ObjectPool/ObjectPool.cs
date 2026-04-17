using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] [Required] private GameObject objectToPool;
    [SerializeField] [Required] private int poolSize;

    private List<GameObject> pool;
    private int currentSize = 0;

    private void Awake()
    {
        pool = new List<GameObject>();
        
        for (int i = 0; i <= poolSize; i++)
        {
            CreateNewObject();
        }
    }

    private GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(objectToPool);

        obj.SetActive(false);
        obj.transform.SetParent(this.transform);
        
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;

        obj.name += " " + currentSize.ToString();
        currentSize++;

        pool.Add(obj);

        return obj;
    }

    public GameObject GetObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        return CreateNewObject();
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void ReturnAllObjects()
    {
        foreach (GameObject obj in pool)
        {
            obj.SetActive(false);
        }
    }
}