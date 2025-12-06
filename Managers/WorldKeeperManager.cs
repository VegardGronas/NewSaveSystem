using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace WorldKeeper
{
    public partial class WorldKeeperManager : MonoBehaviour
    {
        public const string Version = "0.1.0";

        public static WorldKeeperManager Instance { get; private set; }

        [SerializeField] private string defaultProfile = "Dev";

        private string savePath;
        private string rootPath;
        private string rootFolder = "WorldKeeper";

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;

            // Create root folder for all saves
            rootPath = Path.Combine(Application.persistentDataPath, rootFolder);
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            // Set default profile AFTER rootPath exists
            SetProfile(defaultProfile);
        }

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(LoadAsync());
        
            Debug.Log("YOu have " + GetProfiles().Length + " Profiles");
        }

        public void SetProfile(string profile)
        {
            // Use Path.Combine to avoid platform path issues
            savePath = Path.Combine(rootPath, profile + ".json");
            Debug.Log("Profile set to: " + savePath);
        }

        public string[] GetProfiles()
        {
            if (!Directory.Exists(rootPath))
                return Array.Empty<string>();

            // Get only filenames without full path and without extension
            string[] files = Directory.GetFiles(rootPath, "*.json");
            for (int i = 0; i < files.Length; i++)
                files[i] = Path.GetFileNameWithoutExtension(files[i]);

            return files;
        }
    }
}