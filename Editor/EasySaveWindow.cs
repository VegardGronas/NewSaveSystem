using UnityEditor;
using UnityEngine;

public class EasySaveDebugWindow : EditorWindow
{
    [MenuItem("Tools/EasySave Debug Window")]
    public static void ShowWindow()
    {
        GetWindow<EasySaveDebugWindow>("EasySave Debug");
    }

    private void OnGUI()
    {
        GUILayout.Label("EasySave Debug Tools", EditorStyles.boldLabel);

        GUILayout.Space(10);

        if(GUILayout.Button("Delete Save"))
        {
            if (Application.isPlaying)
            {
                if (EasySaveManager.Instance != null)
                {
                    Debug.Log("Game save deleted (Editor Window)!");
                    EasySaveManager.Instance.DeleteSave();
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
                if (EasySaveManager.Instance != null)
                {
                    Debug.Log("Game saved (Editor Window)!");
                    EasySaveManager.Instance.SaveGame();
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
                if (EasySaveManager.Instance != null)
                {
                    EasySaveManager.Instance.StartCoroutine(EasySaveManager.Instance.LoadAsync());
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