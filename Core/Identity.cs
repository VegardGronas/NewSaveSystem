using System;
using UnityEngine;
using static EasySaveManager;

public class Identity : MonoBehaviour
{
    [SerializeField] private string uniqueID;
    public string UniqueID => uniqueID;
    public BaseSave[] SaveComponents;
    public bool wasLoadedFromSave { get; private set; } = false;

    private void Awake()
    {
        IdentityTracker.Register(this);
        AddSaveComponents();
    }

    private void OnValidate()
    {
        if(uniqueID == "")
            uniqueID = Guid.NewGuid().ToString();
    }

    private void OnEnable()
    {
        EasySaveManager.OnContentLoaded += OnGameLoaded;
    }

    private void OnDisable()
    {
        EasySaveManager.OnContentLoaded -= OnGameLoaded;
    }

    private void OnDestroy()
    {
        IdentityTracker.Unregister(this);
    }

    private void AddSaveComponents()
    {
        SaveComponents = GetComponents<BaseSave>();
    }

    public void OnGameLoaded(ContentLoadedEvent evnt)
    {
        if (wasLoadedFromSave)
        {
            Debug.Log("Was loaded from save: " + evnt.wasContentLoadedFromSave + " for " + gameObject.name);
            return;
        }
    }
}