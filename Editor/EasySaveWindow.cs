using UnityEditor;
using UnityEngine;

namespace WorldKeeper
{
    public class EasySaveDebugWindow : EditorWindow
    {
        private string profileName = "Dev";

        [MenuItem("Tools/EasySave Debug Window")]
        public static void ShowWindow()
        {
            GetWindow<EasySaveDebugWindow>("EasySave Debug");
        }

        private void OnGUI()
        {
            GUILayout.Label("EasySave Debug Tools", EditorStyles.boldLabel);

            GUILayout.Space(10);

            profileName = EditorGUILayout.TextField("Profile name", profileName);

            if (GUILayout.Button("Set Profile"))
            {
                if (Application.isPlaying)
                {
                    if (SaveManager.Instance != null)
                    {
                        SaveManager.Instance.SetProfile(profileName);
                    }
                }
            }

            if (GUILayout.Button("Delete Save"))
            {
                if (Application.isPlaying)
                {
                    if (SaveManager.Instance != null)
                    {
                        Debug.Log("Game save deleted (Editor Window)!");
                        SaveManager.Instance.DeleteSave();
                    }
                    else
                    {
                        Debug.LogError("EasySaveManager instance not found in scene!");
                    }
                }
                else
                {
                    Debug.LogWarning("Enter Play Mode to save the game.");
                }
            }

            if (GUILayout.Button("Save Game"))
            {
                if (Application.isPlaying)
                {
                    if (SaveManager.Instance != null)
                    {
                        Debug.Log("Game saved (Editor Window)!");
                        SaveManager.Instance.SaveGame();
                    }
                    else
                    {
                        Debug.LogError("EasySaveManager instance not found in scene!");
                    }
                }
                else
                {
                    Debug.LogWarning("Enter Play Mode to save the game.");
                }
            }

            if (GUILayout.Button("Load Game"))
            {
                if (Application.isPlaying)
                {
                    if (SaveManager.Instance != null)
                    {
                        SaveManager.Instance.StartCoroutine(SaveManager.Instance.LoadAsync());
                        Debug.Log("Loading game (Editor Window)...");
                    }
                    else
                    {
                        Debug.LogError("EasySaveManager instance not found in scene!");
                    }
                }
                else
                {
                    Debug.LogWarning("Enter Play Mode to load the game.");
                }
            }

            GUILayout.Space(10);

            GUILayout.Label("Debug Info", EditorStyles.boldLabel);

            if (Application.isPlaying)
            {
                GUILayout.Label($"Identities in Scene: {IdentityTracker.GetIdentitiesAsArray().Length}");
            }
            else
            {
                GUILayout.Label("Enter Play Mode to see scene info.");
            }
        }
    }
}