using System;
using UnityEngine;

namespace WorldKeeper
{
    public class RememberTransform : BaseSave
    {
        public override string SaveData()
        {
            TransformData data = new TransformData
            {
                position = transform.position,
                rotation = transform.rotation,
                scale = transform.localScale
            };

            string json = JsonUtility.ToJson(data);
            return json;
        }

        public override void LoadData(string json)
        {
            if (string.IsNullOrEmpty(json))
                return;

            TransformData data = JsonUtility.FromJson<TransformData>(json);
            transform.position = data.position;
            transform.rotation = data.rotation;
            transform.localScale = data.scale;
        }
    }

    [Serializable]
    public class TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
    }
}