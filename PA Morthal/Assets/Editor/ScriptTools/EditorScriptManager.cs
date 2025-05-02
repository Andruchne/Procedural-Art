#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

/// <summary>
/// Handles hierarchy changes and creates unique material instances for each renderer's materials.
/// </summary>

[InitializeOnLoad]
public class GlobalMaterialTweakerEditor
{
    static GlobalMaterialTweakerEditor()
    {
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }

    private static void OnHierarchyChanged()
    {
        MaterialTweaker[] materialTweakers = GameObject.FindObjectsByType<MaterialTweaker>(FindObjectsSortMode.None);

        foreach (MaterialTweaker tweaker in materialTweakers)
        {
            HandleMaterials(tweaker);
        }
    }

    private static void HandleMaterials(MaterialTweaker tweaker)
    {
        Renderer rend = tweaker.GetComponent<Renderer>();

        if (rend != null && rend.sharedMaterials != null && rend.sharedMaterials.Length > 0)
        {
            Material[] mats = rend.sharedMaterials;

            for (int i = 0; i < mats.Length; i++)
            {
                if (i < tweaker.usedMaterials.Length && tweaker.usedMaterials[i] != null)
                {
                    if (EditorUtility.IsPersistent(tweaker.usedMaterials[i]))
                    {
                        Material newMat = new Material(tweaker.usedMaterials[i]);
                        mats[i] = newMat;
                    }

                    mats[i].mainTextureScale = tweaker.tiling;
                }
            }

            rend.sharedMaterials = mats;
        }
    }
}
#endif