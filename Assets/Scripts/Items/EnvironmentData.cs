using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentData
{
    public List<string> pickedUpItems;
    public List<StorageData> storageBoxes;

    public EnvironmentData(List<string> _pickedUpItems, List<StorageData> _storageBoxes)
    {
        pickedUpItems = _pickedUpItems;
        storageBoxes = _storageBoxes;
    }


}

[System.Serializable]
public class StorageData
{
    public List <string> items;
    public SerializableVector3 position;
    public SerializableVector3 rotation;
}

[Serializable]
public class SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3(float rX, float rY, float rZ)
    {
        x = rX;
        y = rY;
        z = rZ;
    }

    // Implicitna konverzija iz SerializableVector3 u Vector3
    public static implicit operator Vector3(SerializableVector3 rValue)
    {
        return new Vector3(rValue.x, rValue.y, rValue.z);
    }

    // Implicitna konverzija iz Vector3 u SerializableVector3
    public static implicit operator SerializableVector3(Vector3 rValue)
    {
        return new SerializableVector3(rValue.x, rValue.y, rValue.z);
    }
}
