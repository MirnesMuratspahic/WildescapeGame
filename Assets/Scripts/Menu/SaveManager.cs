using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{

    public static SaveManager Instance { get; set; }
    public bool isSavingToJason;
    public bool isLoading;
    public Canvas loadingScreen;
    public void Awake()
    {
        if (Instance != null && Instance == this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }


    #region || ----- General Section ----- ||

    #region || ----- Loading Section ----- ||


    public void ActivateLoadingScreen()
    {
        loadingScreen.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void DisableLoadingScreen()
    {
        loadingScreen.gameObject.SetActive(false);
    }

    public void LoadGameData()
    {
        SetPlayerData(LoadGameFromBinaryFile().playerData);
        SetEnvironmentData(LoadGameFromBinaryFile().environmentData);
        DisableLoadingScreen();
    }

    private void SetEnvironmentData(EnvironmentData environmentData)
    {
        foreach(Transform itemType in EnvironmentManager.Instance.allItems.transform)
        {
            foreach (Transform item in itemType.transform)
            {
                foreach (Transform childItem in item.transform)
                {
                    if (environmentData.pickedUpItems.Contains(childItem.name))
                    {
                        Destroy(childItem.gameObject);
                    }
                }
            }
        }

        GameObject[] items = GameObject.FindGameObjectsWithTag("Tree");

        foreach (GameObject item in items)
        {
            if (environmentData.pickedUpItems.Contains(item.name))
            {
                Destroy(item.transform.parent.transform.parent.gameObject);
                Debug.Log("Destroyed" + item.name);
                if(item.transform.parent.name == "Tree 9")
                {
                    Vector3 number = item.transform.position;
                    number.y += 1.5f;
                    GameObject brokenTree = Instantiate(Resources.Load<GameObject>("Stump"), number, Quaternion.Euler(0, 0, 0));
                }
                else if(item.transform.parent.name == "Tree 7")
                {
                    Vector3 number = item.transform.position;
                    number.y += 0.5f;
                    GameObject brokenTree = Instantiate(Resources.Load<GameObject>("Stump"), number, Quaternion.Euler(0, 3.5f, 0));
                }
            }
        }

        foreach(StorageData storage in environmentData.storageBoxes)
        {
            GameObject storageBox = Instantiate(Resources.Load<GameObject>("StorageBoxModel"), storage.position
                , Quaternion.Euler(storage.rotation.x, storage.rotation.y, storage.rotation.z));

            storageBox.GetComponent<StorageBox>().items = storage.items;
            storageBox.transform.SetParent(EnvironmentManager.Instance.allItems.transform);
        }

        InventorySystem.Instance.pickedUpItems = environmentData.pickedUpItems;
    }

    private void SetPlayerData(PlayerData playerData)
    {
        PlayerState.Instance.currentHealth = playerData.playerStats[0];
        PlayerState.Instance.currentCalories = playerData.playerStats[1];
        PlayerState.Instance.currentHydrationPercent = playerData.playerStats[2];

        Vector3 loadedPoistion;

        loadedPoistion.x = playerData.playerPositionAndRotation[0];
        loadedPoistion.y = playerData.playerPositionAndRotation[1];
        loadedPoistion.z = playerData.playerPositionAndRotation[2];

        PlayerState.Instance.player.transform.position = loadedPoistion;

        Vector3 loadedRotations;

        loadedRotations.x = playerData.playerPositionAndRotation[3];
        loadedRotations.y = playerData.playerPositionAndRotation[4];
        loadedRotations.z = playerData.playerPositionAndRotation[5];

        PlayerState.Instance.player.transform.rotation = Quaternion.Euler(loadedRotations);

        foreach(var item in playerData.inventoryItems)
        {
            InventorySystem.Instance.AddToInventory(item);
        }

        foreach(var item in playerData.quickSlotItems)
        {
            GameObject availableSlot = EquipSystem.Instance.FindNextEmptySlot();
            var itemToAdd = Instantiate(Resources.Load<GameObject>(item));
            itemToAdd.transform.SetParent(availableSlot.transform, false);
        }

        isLoading = false;
    }

    public void StartGame()
    {
        ActivateLoadingScreen();
        isLoading = true;
        SceneManager.LoadScene("GameScene");
        StartCoroutine(DelayedLoading());
        
    }

    private IEnumerator DelayedLoading()
    {
        yield return new WaitForSeconds(5f);
        LoadGameData();
    }


    #endregion

    #region || ----- Saving Section ----- ||

    public void SaveGame()
    {
        AllGameData data = new AllGameData();
        data.playerData = GetPlayerData();
        data.environmentData = GetEnvironmentData();
        SaveGameToBinaryFile(data);
   }

    private EnvironmentData GetEnvironmentData()
    {
        List<string> pickedUpItems = InventorySystem.Instance.pickedUpItems;

        List<StorageData> allStorage = new List<StorageData>();
        foreach(Transform placeable in EnvironmentManager.Instance.allItems.transform)
        {
            if(placeable.gameObject.transform.GetComponent<StorageBox>())
            {
                var sd = new StorageData();
                sd.items = placeable.gameObject.transform.GetComponent<StorageBox>().items;
                sd.position = placeable.position;
                sd.rotation = new Vector3(placeable.rotation.x, placeable.rotation.y, placeable.rotation.z);

                allStorage.Add(sd);

            }
        }


        return new EnvironmentData(pickedUpItems, allStorage);
    }

    private PlayerData GetPlayerData()
    {
        float[] playerStats = new float[3];
        playerStats[0] = PlayerState.Instance.currentHealth;
        playerStats[1] = PlayerState.Instance.currentCalories;
        playerStats[2] = PlayerState.Instance.currentHydrationPercent;

        float[] playerPositionAndRotation = new float[6];

        playerPositionAndRotation[0] = PlayerState.Instance.player.transform.position.x;
        playerPositionAndRotation[1] = PlayerState.Instance.player.transform.position.y;
        playerPositionAndRotation[2] = PlayerState.Instance.player.transform.position.z;

        playerPositionAndRotation[3] = PlayerState.Instance.player.transform.rotation.x;
        playerPositionAndRotation[4] = PlayerState.Instance.player.transform.rotation.y;
        playerPositionAndRotation[5] = PlayerState.Instance.player.transform.rotation.z;

        string[] inventoryItems = InventorySystem.Instance.itemList.ToArray();
        string[] quickSlotItems = GetQuickSlotItems();

        return new PlayerData(playerStats, playerPositionAndRotation, inventoryItems, quickSlotItems);

    }

    private string[] GetQuickSlotItems()
    {
        List<string> items = new List<string>();    
        foreach(GameObject slot in EquipSystem.Instance.quickSlotsList)
        {
            if(slot.transform.childCount != 0)
            {
                string name = slot.transform.GetChild(0).name;
                string cleanName = name.Replace("(Clone)", "");
                items.Add(cleanName);
            }
        }
        return items.ToArray(); 
    }
    #endregion
    #endregion

    #region || ----- Saving to binary ----- ||

    public void SaveGameToBinaryFile(AllGameData gameData)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/save_game.bin";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, gameData);
        stream.Close();
    }

    public AllGameData LoadGameFromBinaryFile()
    {
        string path = Application.persistentDataPath + "/save_game.bin";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            AllGameData data = formatter.Deserialize(stream) as AllGameData;
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }


    #endregion

    #region || ----- Settings Section ----- ||
    [System.Serializable]
    public class VolumeSettings
    {
        public float effects;
        public float master;
    }
    public void SaveVolumeSettings(float _effects, float _master)
    {
        var volumeSettings = new VolumeSettings
        {
            effects = _effects,
            master = _master
        };

        PlayerPrefs.SetString("Volume", JsonUtility.ToJson(volumeSettings));
        PlayerPrefs.Save();
    }

    public VolumeSettings LoadVolumeSettings()
    {
        return JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));
    }

    #endregion

}
