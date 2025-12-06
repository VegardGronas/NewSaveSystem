using System;
using System.Collections.Generic;
using UnityEngine;

namespace WorldKeeper
{
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
        public string version;
        public string timestamp;
        public List<SavedObjectData> Objects;
    }
}