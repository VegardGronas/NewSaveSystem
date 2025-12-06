using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EasySaveManager : MonoBehaviour
{
    public static EasySaveManager Instance { get; private set; }

    public static event Action<ContentLoadedEvent> OnContentLoaded;

    [SerializeField] private string defaultProfile = "Dev";

    private string savePath;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        SetProfile(defaultProfile);
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(LoadAsync());
    }

    public void SetProfile(string profile)
    {
        savePath = Application.persistentDataPath + "/" + profile + ".json";
    }

    public void SaveGame()
    {
        Identity[] identities = IdentityTracker.GetIdentitiesAsArray();
        List<SavedObjectData> allData = new List<SavedObjectData>();

        foreach (Identity identity in identities)
        {
            BaseSave[] saveComponents = identity.SaveComponents;
            foreach (BaseSave baseSave in saveComponents)
            {
                string json = baseSave.SaveData(); // <-- each component returns its own JSON

                allData.Add(new SavedObjectData
                {
                    UniqueID = identity.UniqueID,
                    PrefabName = identity.PrefabPath,
                    ComponentType = baseSave.GetType().Name, // optional, for easier debugging or future-proofing
                    CustomDataJson = json
                });
            }
        }

        // Now you can serialize `allData` to disk, e.g., as JSON
        string finalJson = JsonUtility.ToJson(new SaveFile { Objects = allData }, true);
        File.WriteAllText(savePath, finalJson);

        Debug.Log("Saved to path: " + savePath);
    }

    public void DeleteSave()
    {
        if(File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Save file deleted " + savePath);
        }
    }

    public IEnumerator LoadAsync()
    {
        // 1. No file? Exit early.
        if (!File.Exists(savePath))
        {
            OnContentLoaded?.Invoke(new ContentLoadedEvent { wasContentLoadedFromSave = false });
            yield break;
        }

        // 2. Load save file
        string json = File.ReadAllText(savePath);
        SaveFile saveFile = JsonUtility.FromJson<SaveFile>(json);

        int processed = 0;
        int batchSize = 5;

        //----------------------------------------------------------------------
        // STEP 1: Restore any identities missing from the scene
        //----------------------------------------------------------------------

        foreach (var saved in saveFile.Objects)
        {
            if (IdentityTracker.Contains(saved.UniqueID))
            {
                // Already in the scene
                continue;
            }

            // Try to load the prefab
            Identity prefab = Resources.Load<Identity>(saved.PrefabName);
            if (prefab == null)
            {
                Debug.LogWarning(
                    $"[EasySave] Missing prefab '{saved.PrefabName}' for saved object '{saved.UniqueID}'."
                );
                continue;
            }

            // Spawn it
            Identity instance = Instantiate(prefab);
            instance.name = prefab.name; // optional: avoid "(Clone)"
            instance.SetUniqueID(saved.UniqueID); // safer than direct access

            // Force update of SaveComponents
            instance.RefreshSaveComponents();

            processed++;
            if (processed % batchSize == 0)
                yield return null;
        }

        //----------------------------------------------------------------------
        // STEP 2: Load component data into each identity
        //----------------------------------------------------------------------

        Identity[] identities = IdentityTracker.GetIdentitiesAsArray();

        foreach (Identity identity in identities)
        {
            foreach (BaseSave baseSave in identity.SaveComponents)
            {
                var savedData = saveFile.Objects.Find(d =>
                    d.UniqueID == identity.UniqueID &&
                    d.ComponentType == baseSave.GetType().Name
                );

                if (savedData != null)
                {
                    baseSave.LoadData(savedData.CustomDataJson);
                }

                processed++;
                if (processed % batchSize == 0)
                    yield return null;
            }
        }

        //----------------------------------------------------------------------
        // DONE
        //----------------------------------------------------------------------
        OnContentLoaded?.Invoke(new ContentLoadedEvent { wasContentLoadedFromSave = true });
    }

    public class ContentLoadedEvent
    {
        public bool wasContentLoadedFromSave = false;
    }
}

[Serializable]
public class SavedObjectData
{
    public string UniqueID;
    public string PrefabName;
    public string ComponentType;
    public string CustomDataJson;
}

[Serializable]
public class SaveFile
{
    public List<SavedObjectData> Objects;
}
