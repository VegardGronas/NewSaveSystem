using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EasySaveManager : MonoBehaviour
{
    public static EasySaveManager Instance { get; private set; }

    public static event Action<ContentLoadedEvent> OnContentLoaded;

    private string savePath;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        savePath = Application.persistentDataPath + "/savefile.json";
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(LoadAsync());
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


    public IEnumerator LoadAsync()
    {
        if (!File.Exists(savePath))
        {
            OnContentLoaded?.Invoke(new ContentLoadedEvent { wasContentLoadedFromSave = false });

            yield break;
        }

        string json = File.ReadAllText(savePath);
        SaveFile saveFile = JsonUtility.FromJson<SaveFile>(json);

        Identity[] identities = IdentityTracker.GetIdentitiesAsArray();
        int processed = 0;
        int batchSize = 5;

        foreach (Identity identity in identities)
        {
            BaseSave[] saveComponents = identity.SaveComponents;

            foreach (BaseSave baseSave in saveComponents)
            {
                // Find saved data for this component
                SavedObjectData savedData = saveFile.Objects
                    .Find(d => d.UniqueID == identity.UniqueID && d.ComponentType == baseSave.GetType().Name);

                if (savedData != null)
                {
                    baseSave.LoadData(savedData.CustomDataJson);
                }

                processed++;
                if (processed % batchSize == 0)
                    yield return null;
            }
        }

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
    public string ComponentType;
    public string CustomDataJson;
}

[Serializable]
public class SaveFile
{
    public List<SavedObjectData> Objects;
}
