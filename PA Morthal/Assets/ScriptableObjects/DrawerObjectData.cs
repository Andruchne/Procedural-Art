using UnityEngine;

[CreateAssetMenu(fileName = "DrawerObjectData", menuName = "Scriptable Objects/DrawerObjectData")]
public class DrawerObjectData : ScriptableObject
{
    public GameObject spawnPrefab;

    public GameObject previewPrefab;
    public Texture2D previewImage;
}
