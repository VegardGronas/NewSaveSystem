using System;
using UnityEngine;
using static WorldKeeper.WorldKeeperManager;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WorldKeeper
{
    public class Identity : MonoBehaviour
    {
        [SerializeField] private string uniqueID;
        [SerializeField] private string prefabPath;
        public string UniqueID => uniqueID;
        public string PrefabPath => prefabPath;

        public BaseSave[] SaveComponents;
        public bool WasLoadedFromSave { get; private set; } = false;

        private void Awake()
        {
            EnsureRuntimeID();
            IdentityTracker.Register(this);
            AddSaveComponents();
        }

        public void SetUniqueID(string id) => uniqueID = id;

        public void RefreshSaveComponents() =>
            SaveComponents = GetComponents<BaseSave>();


#if UNITY_EDITOR
        private void OnValidate()
        {
            // --- If editing a project prefab asset ---
            if (PrefabUtility.IsPartOfPrefabAsset(this))
            {
                uniqueID = ""; // prefabs should never have IDs

                // Try to find the Resources-relative path
                string assetPath = AssetDatabase.GetAssetPath(gameObject);

                if (assetPath.Contains("Resources/"))
                {
                    // Extract "folder/subfolder/name"
                    prefabPath = ExtractResourcesPath(assetPath);
                }
                else
                {
                    // Not in Resources, warn user
                    prefabPath = "";
                    // Debug.LogWarning($"{name} prefab is not inside a Resources folder. It cannot be loaded by EasySave.");
                }

                return;
            }

            // --- If editing a scene object ---
            if (string.IsNullOrEmpty(uniqueID))
            {
                uniqueID = Guid.NewGuid().ToString();
                EditorUtility.SetDirty(this);
            }
        }

        private string ExtractResourcesPath(string assetPath)
        {
            // Example:
            // assetPath = "Assets/MyGame/Resources/Buildings/Walls/WoodWall.prefab"

            int index = assetPath.IndexOf("Resources/") + "Resources/".Length;

            string subPath = assetPath.Substring(index); // "Buildings/Walls/WoodWall.prefab"
            subPath = subPath.Replace(".prefab", "");    // remove extension

            return subPath; // final result: "Buildings/Walls/WoodWall"
        }
#endif

        private void OnEnable()
        {
            OnContentLoaded += OnGameLoaded;
        }

        private void OnDisable()
        {
            OnContentLoaded -= OnGameLoaded;
        }

        private void OnDestroy()
        {
            IdentityTracker.Unregister(this);
        }

        private void AddSaveComponents()
        {
            SaveComponents = GetComponents<BaseSave>();
        }

        // Ensures runtime safety: if something spawned without an ID
        private void EnsureRuntimeID()
        {
            // If ID is missing, generate one
            if (string.IsNullOrEmpty(uniqueID))
            {
                uniqueID = Guid.NewGuid().ToString();
                return;
            }

            // If ID already exists in the tracker, this is a duplicated object -> give new ID
            if (IdentityTracker.Contains(uniqueID))
            {
                uniqueID = Guid.NewGuid().ToString();
            }
        }


        public void OnGameLoaded(ContentLoadedEvent evnt)
        {
            Debug.Log($"Was loaded from save: {evnt.wasContentLoadedFromSave} for {gameObject.name} Load time: {evnt.loadTime}");
        }
    }
}