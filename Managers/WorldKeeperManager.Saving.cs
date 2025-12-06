using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace WorldKeeper
{
    public partial class WorldKeeperManager : MonoBehaviour
    {
        public void SaveGame()
        {
            OnBeforeSave?.Invoke();

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
            string finalJson = JsonUtility.ToJson(new SaveFile { Objects = allData, version = Version, timestamp = DateTime.UtcNow.ToString("o") }, true);
            File.WriteAllText(savePath, finalJson);

            OnAfterSave?.Invoke();

            Debug.Log("Saved to path: " + savePath);
        }

        public void DeleteSave()
        {
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
                Debug.Log("Save file deleted " + savePath);
            }
        }
    }
}