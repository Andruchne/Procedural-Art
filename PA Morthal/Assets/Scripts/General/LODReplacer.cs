using System.Collections.Generic;
using UnityEngine;

public class LODReplacer : MonoBehaviour
{
    [SerializeField] GameObject replaceWith;
    [SerializeField] GameObject[] additionalInherents;

    [SerializeField] bool hideAllChildren;

    List<MeshRenderer> inherents = new List<MeshRenderer>();
    List<Transform> inherentTransform = new List<Transform>();
    bool isActive;

    bool firstSetup;

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        ResetValues();

        MeshRenderer mesh = GetComponent<MeshRenderer>();
        if (mesh != null) { inherents.Add(GetComponent<MeshRenderer>()); }

        if (hideAllChildren)
        {
            List<Transform> children = new List<Transform>();

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.gameObject != replaceWith) 
                {
                    MeshRenderer mr = child.GetComponent<MeshRenderer>();
                    if (mr != null) { inherents.Add(mr); }
                    else { inherentTransform.Add(child.transform); }
                }
            }
        }
        else
        {
            for (int i = 0; i < additionalInherents.Length; i++)
            {
                if (additionalInherents[i].gameObject != replaceWith)
                {
                    MeshRenderer mr = additionalInherents[i].GetComponent<MeshRenderer>();
                    if (mr != null) { inherents.Add(mr); }
                    else { inherentTransform.Add(additionalInherents[i].transform); }
                }
            }
        }

        if (inherents.Count == 0) { Destroy(gameObject); }
        else if (!firstSetup)
        {
            LODHandler.Instance.AddLODObject(this);
            firstSetup = true;
        }

        isActive = LODHandler.Instance.CheckState(transform.position);
    }

    private void OnDestroy()
    {
        if (this != null)
        {
            LODHandler.Instance.RemoveLODObject(this);
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

            foreach (Transform trans in inherentTransform)
            {
                trans.gameObject.SetActive(false);
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

            foreach (Transform trans in inherentTransform)
            {
                trans.gameObject.SetActive(true);
            }

            replaceWith.SetActive(false);
            isActive = false;
        }
    }

    public bool GetState()
    {
        return isActive;
    }

    public void SetReplacement(GameObject replacement)
    {
        replaceWith = replacement;
    }

    public void SetHideAllChildren(bool state)
    {
        hideAllChildren = state;
    }

    public void ResetValues()
    {
        inherents.Clear();
        inherentTransform.Clear();
    }
}
