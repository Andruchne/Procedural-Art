using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LODHandler : MonoBehaviour
{
    public static LODHandler Instance { get; private set; }

    [SerializeField] float distanceToTrigger = 500.0f;

    private List<LODReplacer> LODObjects = new List<LODReplacer>();

    private List<LODReplacer> removeList = new List<LODReplacer>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void Update()
    {
        CheckObjects();
    }

    public void AddLODObject(LODReplacer pObject)
    {
        LODObjects.Add(pObject);
    }

    private void CheckObjects()
    {
        foreach(LODReplacer obj in LODObjects)
        {
            //print((obj.transform.position - transform.position).magnitude);

            if (obj == null) 
            { 
                removeList.Add(obj);
                continue;
            }

            if (!obj.GetState() && (obj.transform.position - transform.position).magnitude > distanceToTrigger)
            {
                obj.ActivateLOD();
            }
            else if (obj.GetState() && (obj.transform.position - transform.position).magnitude <= distanceToTrigger)
            {
                obj.DeactivateLOD();
            }
        }

        // Remove any invalid entries
        if (removeList.Count > 0)
        {
            foreach (LODReplacer obj in removeList)
            {
                LODObjects.Remove(obj);
            }
            removeList.Clear();
        }
    }
}
