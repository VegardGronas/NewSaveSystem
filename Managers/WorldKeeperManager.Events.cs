using System;
using UnityEngine;

namespace WorldKeeper
{
    public partial class WorldKeeperManager : MonoBehaviour
    {
        public static event Action<ContentLoadedEvent> OnContentLoaded;
        public static event Action OnBeforeSave;
        public static event Action OnAfterSave;
        public static event Action OnBeforeLoad;
        public static event Action OnAfterLoad;

        public class ContentLoadedEvent
        {
            public bool wasContentLoadedFromSave = false;
            public float loadTime;
        }
    }
}