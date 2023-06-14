using UnityEngine;
using UnityEditor;

public class TransformPrefab : EditorWindow
{
    private GameObject buildingPrefab;

    [MenuItem("Tools/Transform Prefab")]
    public static void ShowWindow()
    {
        GetWindow<TransformPrefab>("Transform Prefab");
    }

    private void OnGUI()
    {
        ////GUILayout.Label("Prefab", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();

        buildingPrefab = (GameObject)EditorGUILayout.ObjectField("Prefab", buildingPrefab, typeof(GameObject), false);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(this, "Prefab Change");
        }

        if (GUILayout.Button("Magic!"))
        {
            Debug.Log("Pay 5$ plz");

            if (buildingPrefab == null)
                return;

            foreach(GameObject obj in Selection.gameObjects)
            {
                GameObject instantiatedPrefab = PrefabUtility.InstantiatePrefab(buildingPrefab) as GameObject;

                instantiatedPrefab.transform.parent = obj.transform.parent;

                instantiatedPrefab.transform.position = obj.transform.position;
                instantiatedPrefab.transform.rotation = obj.transform.rotation;
                instantiatedPrefab.transform.localScale = obj.transform.localScale;

                Undo.DestroyObjectImmediate(obj.gameObject);
                DestroyImmediate(obj.gameObject);

                Undo.RegisterCreatedObjectUndo(instantiatedPrefab, "Prefab Replacement");
                
            }
        }
    }
}