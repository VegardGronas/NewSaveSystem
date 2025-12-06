using System;
using UnityEngine;

public class RememberSceneStatus : BaseSave
{
    public override string SaveData()
    {
        SceneStatusData data = new SceneStatusData();
        data.IsActive = gameObject.activeInHierarchy;

        string json = JsonUtility.ToJson(data);
        return json;
    }

    public override void LoadData(string json)
    {
        if (string.IsNullOrEmpty(json))
            return;

        SceneStatusData data = JsonUtility.FromJson<SceneStatusData>(json);
        gameObject.SetActive(data.IsActive);
    }
}

[Serializable]
public class SceneStatusData
{
    public bool IsActive = true;
}