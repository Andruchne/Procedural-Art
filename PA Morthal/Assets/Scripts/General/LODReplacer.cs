using System.Collections.Generic;
using UnityEngine;

public class LODReplacer : MonoBehaviour
{
    [SerializeField] GameObject replaceWith;
    [SerializeField] GameObject[] additionalInherents;

    List<MeshRenderer> inherents = new List<MeshRenderer>();
    bool isActive;

    private void Start()
    {
        inherents.Add(GetComponent<MeshRenderer>());

        for (int i = 0; i < additionalInherents.Length; i++)
        {
            inherents.Add(additionalInherents[i].GetComponent<MeshRenderer>());
        }

        if (inherents.Count == 0) { Destroy(gameObject); }
        else
        {
            LODHandler.Instance.AddLODObject(this);
        }
    }

    public void ActivateLOD()
    {
        if (!isActive)
        {
            foreach (MeshRenderer inh in inherents)
            {
                inh.enabled = false;
            }

            replaceWith.SetActive(true);
            isActive = true;
        }
    }

    public void DeactivateLOD()
    {
        if (isActive)
        {
            foreach (MeshRenderer inh in inherents)
            {
                inh.enabled = true;
            }
            
            replaceWith.SetActive(false);
            isActive = false;
        }
    }

    public bool GetState()
    {
        return isActive;
    }
}
