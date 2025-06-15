using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor Script for pillar blocks, to make it easier to change type and look
/// </summary>

[CustomEditor(typeof(PillarBlock))]
public class PillarBlockEditor : Editor
{
    private SerializedProperty bigLeftPillar;
    private SerializedProperty bigRightPillar;
    private SerializedProperty smallLeftPillar;
    private SerializedProperty smallRightPillar;
    private SerializedProperty plankFence;
    private SerializedProperty ropeFence;

    private bool showSerializedFields = false;

    private int leftPillarSelection;
    private int rightPillarSelection;

    private void OnEnable()
    {
        bigLeftPillar = serializedObject.FindProperty("bigLeftPillar");
        bigRightPillar = serializedObject.FindProperty("bigRightPillar");
        smallLeftPillar = serializedObject.FindProperty("smallLeftPillar");
        smallRightPillar = serializedObject.FindProperty("smallRightPillar");
        plankFence = serializedObject.FindProperty("plankFence");
        ropeFence = serializedObject.FindProperty("ropeFence");

        PillarBlock pillarBlock = (PillarBlock)target;

        // Check what option is currently picked
        leftPillarSelection = pillarBlock.IsLeftPillarSmall() ? 0 : 1;
        rightPillarSelection = pillarBlock.IsRightPillarSmall() ? 0 : 1;
    }

    public override void OnInspectorGUI()
    {
        PillarBlock pillarBlock = (PillarBlock)target;

        // Toggle show/hide of SerializedFields
        showSerializedFields = EditorGUILayout.Foldout(showSerializedFields, "SerializedFields");
        if (showSerializedFields)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(bigLeftPillar);
            EditorGUILayout.PropertyField(bigRightPillar);
            EditorGUILayout.PropertyField(smallLeftPillar);
            EditorGUILayout.PropertyField(smallRightPillar);
            EditorGUILayout.PropertyField(plankFence);
            EditorGUILayout.PropertyField(ropeFence);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space(10);

        // Fence Section
        EditorGUILayout.LabelField("Fence", EditorStyles.boldLabel);
        if (GUILayout.Button("Plank"))
        {
            pillarBlock.SetFence(true, true);
        }

        if (GUILayout.Button("Rope"))
        {
            pillarBlock.SetFence(true, false);
        }

        if (GUILayout.Button("None"))
        {
            pillarBlock.SetFence(false);
        }

        EditorGUILayout.Space(10);

        // Pillar Section
        EditorGUILayout.LabelField("Pillar", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Left Pillar", GUILayout.Width(100));
        int newLeft = EditorGUILayout.Popup(leftPillarSelection, new string[] { "Small", "Big" });
        EditorGUILayout.EndHorizontal();

        if (newLeft != leftPillarSelection)
        {
            leftPillarSelection = newLeft;
            bool isSmall = (leftPillarSelection == 0);
            pillarBlock.SetPillarType(isSmall, pillarBlock.IsRightPillarSmall());
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Right Pillar", GUILayout.Width(100));
        int newRight = EditorGUILayout.Popup(rightPillarSelection, new string[] { "Small", "Big" });
        EditorGUILayout.EndHorizontal();

        if (newRight != rightPillarSelection)
        {
            rightPillarSelection = newRight;
            bool isSmall = (rightPillarSelection == 0);
            pillarBlock.SetPillarType(pillarBlock.IsLeftPillarSmall(), isSmall);
        }

        serializedObject.ApplyModifiedProperties();
    }
}