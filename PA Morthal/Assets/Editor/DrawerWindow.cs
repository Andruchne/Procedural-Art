using UnityEngine;
using UnityEditor;
using Demo;

public class DrawerWindow : EditorWindow
{
    DrawerObjectData currentData;

    Texture2D selectedImage;
    bool drawActive = false;

    GameObject previewInstance;
    float rotationY = 0f;

    [MenuItem("Tools/Drawer")]
    public static void ShowWindow()
    {
        GetWindow<DrawerWindow>("Drawer");
    }

    private void OnEnable()
    {
        Setup();
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        if (previewInstance != null)
            DestroyImmediate(previewInstance);
    }

    private void OnGUI()
    {
        // Group section for horizontal centering
        GUILayout.BeginHorizontal();

        // To add space for centering 
        GUILayout.FlexibleSpace();

        // To align components vertically
        GUILayout.BeginVertical();

        GUIStyle centeredLabelStyle = new GUIStyle(EditorStyles.boldLabel);
        centeredLabelStyle.alignment = TextAnchor.MiddleCenter; 
        centeredLabelStyle.fontSize = 18;

        GUILayout.Space(30);
        GUILayout.Label("Draw Selection", centeredLabelStyle);
        GUILayout.Space(10);

        if (selectedImage != null)
        {
            if (GUILayout.Button(selectedImage, GUILayout.Width(256), GUILayout.Height(256)))
            {
                DrawerObjectPicker.Open(OnImageSelected);
            }
        }
        else
        {
            if (GUILayout.Button("Select Image", GUILayout.Width(256), GUILayout.Height(256)))
            {
                DrawerObjectPicker.Open(OnImageSelected);
            }
        }

        GUILayout.Space(10);
        bool previousDrawActive = drawActive;
        drawActive = EditorGUILayout.ToggleLeft("Enable Draw", drawActive);

        if (drawActive != previousDrawActive) { ToggleChanged(drawActive); }

        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (!drawActive || currentData == null || currentData.previewPrefab == null || currentData.spawnPrefab == null) { return; }

        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 spawnPosition = hit.point;

            // Create preview if not already doing so
            if (previewInstance == null)
            {
                previewInstance = (GameObject)PrefabUtility.InstantiatePrefab(currentData.previewPrefab);
                previewInstance.hideFlags = HideFlags.HideAndDontSave;
            }

            previewInstance.transform.position = spawnPosition;
            previewInstance.transform.rotation = Quaternion.Euler(0, rotationY, 0);

            // Update scene
            HandleUtility.Repaint();

            bool isShift = e.shift;
            bool isCtrl = e.control;

            // Rotation with Left/Right arrow keys
            if (e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.LeftArrow)
                {
                    rotationY -= 10f;
                    e.Use();
                }
                else if (e.keyCode == KeyCode.RightArrow)
                {
                    rotationY += 10f;
                    e.Use();
                }
            }

            // Place object
            if (!isShift && e.type == EventType.MouseDown && e.button == 0)
            {
                // Sort under parent
                GameObject parent = GameObject.Find("ManualToolingPlaced");
                if (parent == null)
                {
                    parent = new GameObject("ManualToolingPlaced");
                    Undo.RegisterCreatedObjectUndo(parent, "Create ManualToolingPlaced Parent");
                }

                GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(currentData.spawnPrefab, parent.transform);
                Undo.RegisterCreatedObjectUndo(obj, "Place Prefab");

                obj.transform.position = spawnPosition;
                obj.transform.rotation = Quaternion.Euler(0, rotationY, 0);

                Shape shape = obj.GetComponent<Shape>();
                if (shape != null) { shape.Generate(); }

                e.Use();
            }
        }
    }

    private void Setup()
    {
        string[] guids = AssetDatabase.FindAssets("t:DrawerObjectData", new[] { "Assets/ScriptableObjects/DrawerObjectData" });
        if (guids.Length > 0)
        {
            ScriptableObject drawerObject = ScriptableObject.CreateInstance("DrawerObjectData");

            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            drawerObject = AssetDatabase.LoadAssetAtPath<DrawerObjectData>(path);
            currentData = drawerObject as DrawerObjectData;

            selectedImage = currentData.previewImage;
        }
    }

    private void ToggleChanged(bool newState)
    {
        if (!newState && previewInstance != null)
        {
            DestroyImmediate(previewInstance);
            previewInstance = null;
        }
    }

    private void OnImageSelected(DrawerObjectData data)
    {
        currentData = data;
        selectedImage = data.previewImage;
        if (previewInstance != null)
        {
            DestroyImmediate(previewInstance);
            previewInstance = null;
        }
        // Updates Window
        Repaint();
    }
}