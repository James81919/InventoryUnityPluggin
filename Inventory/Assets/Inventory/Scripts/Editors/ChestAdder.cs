using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChestAdder : EditorWindow
{
    // Collapsible Booleans
    private bool ShowTransform = false;

    public Transform ChestTransform;

    [MenuItem("InventorySystem/Chest Adder")]
    public static void ShowWindow()
    {
        GetWindow<ChestAdder>("Chest Adder");
    }

    public void Awake()
    {

    }

    private void OnGUI()
    {
        EditorGUILayout.Space();

        ShowTransform = EditorGUILayout.Foldout(ShowTransform, "Transform");
        if (ShowTransform)
        {
            GUILayout.Label("   Position", EditorStyles.boldLabel);
            //ChestTransform.position.Set(
            //ChestTransform.position.x = EditorGUILayout.TextField("    x: ", ChestTransform.position.x.ToString());
            //EditorGUILayout.TextField("    y: ", ChestTransform.position.y.ToString());
            //EditorGUILayout.TextField("    z: ", ChestTransform.position.z.ToString());


        }

        if (GUILayout.Button("Add"))
        {
            AddChest();
        }
    }

    private void AddChest()
    {

    }
}
