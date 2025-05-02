using UnityEngine;

/// <summary>
/// This script allows changing the tiling of multiple materials on a Renderer.
/// Editortime: Instantiates new Materials for each object to allow unique tiling.
/// Playtime: Uses MaterialPropertyBlock for efficiency.
/// </summary>

[ExecuteAlways]
public class MaterialTweaker : MonoBehaviour
{
    public Vector2 tiling = Vector2.one;
    public Material[] usedMaterials;

    private Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        HandleMaterial();
    }

    private void OnEnable()
    {
        HandleMaterial();
    }

    private void OnValidate()
    {
        HandleMaterial();
    }

    private void Update()
    {
        if (Application.isPlaying)
        {
            HandleMaterial();
        }
    }

    private void HandleMaterial()
    {
        if (rend == null || rend.sharedMaterials == null || rend.sharedMaterials.Length == 0) { return; }

        if (Application.isPlaying)
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            rend.GetPropertyBlock(mpb);

            mpb.SetVector("_MainTex_ST", new Vector4(tiling.x, tiling.y, 0f, 0f));
            rend.SetPropertyBlock(mpb);

            usedMaterials = rend.sharedMaterials;
        }
    }
}