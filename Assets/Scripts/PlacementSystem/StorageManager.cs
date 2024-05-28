using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public static StorageManager Instance { get; set; }

    [SerializeField] GameObject storageBoxSmallUI;
    [SerializeField] StorageBox selectedStorage;
    public bool storageUIOpen;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void OpenBox(StorageBox storage)
    {

        selectedStorage = storage;
        PopulateStorage(GetRelevantUI(selectedStorage));

        GetRelevantUI(selectedStorage).SetActive(true);
        storageUIOpen = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SelectionManager.Instance.DisableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
    }

    private void PopulateStorage(GameObject storageUI)
    {
        List<GameObject> uiSlots = new List<GameObject>();

        foreach (Transform child in storageUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                uiSlots.Add(child.gameObject);
            }
        }

        foreach (string name in selectedStorage.items)
        {
            foreach (GameObject slot in uiSlots)
            {
                if (slot.transform.childCount < 1)
                {
                    GameObject itemPrefab = Resources.Load<GameObject>(name);
                    
                    var itemToAdd = Instantiate(itemPrefab, slot.transform.position, slot.transform.rotation);
                    itemToAdd.name = name;
                    itemToAdd.transform.SetParent(slot.transform);
                    break;
                }
            }
        }
    }

    private void RecalculateStorage(GameObject storageUI)
    {
        List<GameObject> uiSlots = new List<GameObject>();

        foreach (Transform child in storageUI.transform)
        {
            uiSlots.Add(child.gameObject);
        }

        selectedStorage.items.Clear();

        List<GameObject> toBeDeleted = new List<GameObject>();

        foreach (GameObject slot in uiSlots)
        {
            if (slot.CompareTag("Slot"))
            {
                if (slot.transform.childCount > 0)
                {
                    string name = slot.transform.GetChild(0).name;
                    string result = name.Replace("(Clone)", "");

                    selectedStorage.items.Add(result);
                    toBeDeleted.Add(slot.transform.GetChild(0).gameObject);
                }
            }
        }

        foreach (GameObject obj in toBeDeleted)
        {
            Destroy(obj);
        }
    }

    public void CloseBox()
    {
        Debug.Log("Closing box: " + selectedStorage.name);
        RecalculateStorage(GetRelevantUI(selectedStorage));

        GetRelevantUI(selectedStorage).SetActive(false);
        storageUIOpen = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SelectionManager.Instance.EnableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;

        Debug.Log("Box closed and UI hidden.");
    }

    public void SetSelectedStorage(StorageBox storage)
    {
        selectedStorage = storage;
    }

    private GameObject GetRelevantUI(StorageBox storage)
    {
        // Create a switch for other types if needed
        return storageBoxSmallUI;
    }
}
