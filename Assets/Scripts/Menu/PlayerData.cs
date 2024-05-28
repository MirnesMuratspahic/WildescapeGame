using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] playerStats;
    public float[] playerPositionAndRotation;
    public string[] inventoryItems;
    public string[] quickSlotItems;

    public PlayerData(float[] _playerStats, float[] _playerPositionAndRotation, string[] _inventoryItems, string[] _quickSlotItems)
    {
        playerStats = _playerStats;
        playerPositionAndRotation = _playerPositionAndRotation;
        inventoryItems = _inventoryItems;
        quickSlotItems = _quickSlotItems;
    }
}
