using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Identity))]
public class IdentityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Identity identity = (Identity)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("EasySave Debug Info", EditorStyles.boldLabel);

        // Is prefab or instance?
        bool isPrefab = PrefabUtility.IsPartOfPrefabAsset(identity);
        bool isInScene = !isPrefab;

        EditorGUILayout.LabelField("In Scene:", isInScene ? " Yes" : " No (Prefab)");

        if(Application.isPlaying)
        {
            // Registration status
            bool registered = IdentityTracker.Contains(identity);
            EditorGUILayout.LabelField("Registered:", registered ? " Yes" : " No");

            EditorGUILayout.Space(10);

            if (!registered && isInScene)
            {
                EditorGUILayout.HelpBox("This Identity is NOT registered. Is Awake disabled or object inactive?", MessageType.Warning);
            }
        }

        // Unique ID
        EditorGUILayout.LabelField("Unique ID:", identity.UniqueID);
    }
}
