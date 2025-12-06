using UnityEngine;

public abstract class BaseSave : MonoBehaviour
{
    /// <summary>
    /// Return the json of the serialized class
    /// </summary>
    /// <returns></returns>
    public abstract string SaveData();
    public abstract void LoadData(string json);
}