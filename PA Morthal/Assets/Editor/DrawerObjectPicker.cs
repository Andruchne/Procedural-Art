using UnityEngine;
using UnityEditor;
using System;

public class DrawerObjectPicker : EditorWindow
{
    static Action<DrawerObjectData> onImageSelected;
    Vector2 scrollPos;

    static DrawerObjectData[] drawerObjects;

    public static void Open(Action<DrawerObjectData> callback)
    {
        onImageSelected = callback;

        string[] guids = AssetDatabase.FindAssets("t:DrawerObjectData", new[] { "Assets/ScriptableObjects/DrawerObjectData" });
        drawerObjects = new DrawerObjectData[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            drawerObjects[i] = AssetDatabase.LoadAssetAtPath<DrawerObjectData>(path);
        }

        EditorWindow window = GetWindow<DrawerObjectPicker>("Select Object");
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 400, 400);
    }

    private void OnGUI()
    {
        if (drawerObjects == null || drawerObjects.Length == 0)
        {
            EditorGUILayout.LabelField("No DrawerObjectData found...");
            return;
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        int columns = 3;
        int rows = Mathf.CeilToInt(drawerObjects.Length / (float)columns);
        int index = 0;

        GUILayout.BeginVertical();
        GUILayout.Space(20);

        for (int y = 0; y < rows; y++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            for (int x = 0; x < columns; x++)
            {
                if (index >= drawerObjects.Length) break;
                if (GUILayout.Button(drawerObjects[index].previewImage, GUILayout.Width(128), GUILayout.Height(128)))
                {
                    onImageSelected?.Invoke(drawerObjects[index]);
                    Close(); // Close picker
                }
                index++;
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
    }
}