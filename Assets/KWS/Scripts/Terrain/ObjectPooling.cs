using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    private Queue<GameObject> plane_pooling_queue = new Queue<GameObject> ();

    public static ObjectPooling instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        for(int i=0; i<2; i++)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Plane);
            obj.SetActive(false);

            plane_pooling_queue.Enqueue(obj);
        }
    }
    public GameObject GetPlane()
    {
        GameObject obj;
        Debug.Log(plane_pooling_queue.Count);

        if (plane_pooling_queue.Count > 0)
        {
            obj = plane_pooling_queue.Dequeue();
        }
        else
        {
            obj = GameObject.CreatePrimitive(PrimitiveType.Plane);
        }
        if(obj != null)
            obj.SetActive(true);

        return obj;
    }
    private void OnDestroy()
    {
        Debug.Log ("Destroy");
    }
    public void ReturnPlane(GameObject obj)
    {
        plane_pooling_queue.Enqueue (obj);
        obj.SetActive(false);
    }
}
