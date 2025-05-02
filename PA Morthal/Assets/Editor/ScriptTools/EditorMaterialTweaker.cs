#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

/// <summary>
/// Handles creating unique instances per material when tweaking tiling in Editor.
/// </summary>

[CustomEditor(typeof(MaterialTweaker))]
[CanEditMultipleObjects]
public class MaterialTweakerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (!Application.isPlaying)
        {
            foreach (Object obj in targets)
            {
                MaterialTweaker tweaker = (MaterialTweaker)obj;
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
    }
}
#endif