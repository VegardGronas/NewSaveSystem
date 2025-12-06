using UnityEditor;
using UnityEngine;

namespace WorldKeeper
{
    public class WorldKeeperDebugWindow : EditorWindow
    {
        private string profileName = "Dev";
        private Vector2 scrollPos;

        [MenuItem("Tools/WorldKeeper Debug Window")]
        public static void ShowWindow()
        {
            GetWindow<WorldKeeperDebugWindow>("WorldKeeper Debug");
        }

        private void OnGUI()
        {
            // Scrollable window
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            // Header
            EditorGUILayout.Space();
            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel) { fontSize = 14 };
            EditorGUILayout.LabelField("WorldKeeper Debug Tools", headerStyle);
            EditorGUILayout.Space();

            // Profile Section
            GUI.backgroundColor = new Color(0.8f, 0.85f, 1f); // light blue
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUI.backgroundColor = Color.white;

            EditorGUILayout.LabelField("Profile Settings", EditorStyles.boldLabel);
            profileName = EditorGUILayout.TextField("Profile name", profileName);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Set Profile"))
            {
                if (Application.isPlaying && WorldKeeperManager.Instance != null)
                    WorldKeeperManager.Instance.SetProfile(profileName);
            }
            if (GUILayout.Button("Delete Save"))
            {
                if (Application.isPlaying && WorldKeeperManager.Instance != null)
                    WorldKeeperManager.Instance.DeleteSave();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            // Game Save Section
            GUI.backgroundColor = new Color(0.9f, 1f, 0.9f); // light green
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUI.backgroundColor = Color.white;

            EditorGUILayout.LabelField("Game Save Controls", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Save Game"))
            {
                if (Application.isPlaying && WorldKeeperManager.Instance != null)
                    WorldKeeperManager.Instance.SaveGame();
            }
            if (GUILayout.Button("Load Game"))
            {
                if (Application.isPlaying && WorldKeeperManager.Instance != null)
                    WorldKeeperManager.Instance.StartCoroutine(WorldKeeperManager.Instance.LoadAsync());
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            // Debug Info Section
            GUI.backgroundColor = new Color(1f, 0.9f, 0.9f); // light red
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUI.backgroundColor = Color.white;

            EditorGUILayout.LabelField("Debug Info", EditorStyles.boldLabel);

            if (Application.isPlaying)
            {
                GUILayout.Label($"Identities in Scene: {IdentityTracker.GetIdentitiesAsArray().Length}");
            }
            else
            {
                GUILayout.Label("Enter Play Mode to see scene info.");
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndScrollView();
        }
    }
}
