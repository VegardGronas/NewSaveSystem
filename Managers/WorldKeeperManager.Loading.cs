using System.Collections;
using UnityEngine;
using System.IO;

namespace WorldKeeper
{
    public partial class WorldKeeperManager : MonoBehaviour
    {
        public IEnumerator LoadAsync()
        {
            OnBeforeLoad?.Invoke();

            float startTime = Time.realtimeSinceStartup;

            // 1. No file? Exit early.
            if (!File.Exists(savePath))
            {
                float loadTime = Time.realtimeSinceStartup - startTime;
                OnContentLoaded?.Invoke(new ContentLoadedEvent
                {
                    wasContentLoadedFromSave = false,
                    loadTime = loadTime
                });
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
                if (!IdentityTracker.Contains(saved.UniqueID))
                {
                    Identity prefab = Resources.Load<Identity>(saved.PrefabName);
                    if (prefab != null)
                    {
                        Identity instance = Instantiate(prefab);
                        instance.name = prefab.name;
                        instance.SetUniqueID(saved.UniqueID);
                        instance.RefreshSaveComponents();
                    }
                }

                processed++;
                if (processed % batchSize == 0)
                    yield return null;
            }

            //----------------------------------------------------------------------
            // STEP 2: Load data into existing identities
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
                        baseSave.LoadData(savedData.CustomDataJson);

                    processed++;
                    if (processed % batchSize == 0)
                        yield return null;
                }
            }

            //----------------------------------------------------------------------
            // DONE — measure load time
            //----------------------------------------------------------------------

            float finalLoadTime = Time.realtimeSinceStartup - startTime;

            OnContentLoaded?.Invoke(new ContentLoadedEvent
            {
                wasContentLoadedFromSave = true,
                loadTime = finalLoadTime
            });

            OnAfterLoad?.Invoke();
        }
    }
}