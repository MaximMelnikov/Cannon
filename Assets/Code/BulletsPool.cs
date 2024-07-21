using System.Collections.Generic;
using UnityEngine;

public class BulletsPool : MonoBehaviour
{
    private Queue<GameObject> _pool = new Queue<GameObject>();

    private void Awake()
    {
        ServiceLocator.RegisterService(this);
    }

    public GameObject GetObject()
    {
        GameObject obj = null;
        if (_pool.TryDequeue(out obj))
        {
            obj.transform.SetParent(null);
            obj.SetActive(true);
        }
        
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        _pool.Enqueue(obj);
        obj.SetActive(false);
        obj.transform.SetParent(transform);
    }
}