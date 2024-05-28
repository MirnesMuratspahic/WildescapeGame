using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    // -- UI -- //
    public GameObject quickSlotsPanel;

    public List<GameObject> quickSlotsList = new List<GameObject>();
    public List<string> itemList = new List<string>();

    public List<GameObject> frameHolder = new List<GameObject>();
    public int selectedNumber = -1;
    public GameObject selectedItem;
    public GameObject toolHolder;
    public GameObject itemModel;

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


    private void Start()
    {
        PopulateSlotList();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectQuickSlot(1);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectQuickSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectQuickSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectQuickSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectQuickSlot(5);
        }
        else if( Input.GetKeyDown(KeyCode.X))
        {
            SelectQuickSlot(-1);
        }
    }

    private void SelectQuickSlot(int number)
    {
        if(number != -1)
        {
            if (CheckIfSlotIsFull(number))
            {
                if (selectedItem != null && itemModel != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                    
                }
                selectedNumber = number;
                selectedItem = quickSlotsList[number - 1].transform.GetChild(0).gameObject;
                selectedItem.GetComponent<InventoryItem>().isSelected = true;

                SetEquippedModel(selectedItem);
            }
            else if(itemModel != null) 
            {
               
                DestroyImmediate(itemModel.gameObject);
                itemModel = null;
               
            }


        }

        if (number == -1) 
        {
            for (int i = 0; i < frameHolder.Count; i++)
            {
                frameHolder[i].SetActive(false);
            }
            selectedNumber = -1;
            if(itemModel != null)
            {
                DestroyImmediate(itemModel.gameObject);
                itemModel = null;
            }
        }
        else
        {
            for (int i = 0; i < frameHolder.Count; i++)
            {
                frameHolder[i].SetActive(false);
            }
            selectedNumber = number;
            frameHolder[number - 1].SetActive(true);
        }
           

        
    }

    private void SetEquippedModel(GameObject selectedItem)
    {
        if (itemModel != null)
        {
            DestroyImmediate(itemModel.gameObject);
            itemModel = null;
        }
        string itemName = selectedItem.name.Replace("(Clone)", "");
        itemModel = Instantiate(Resources.Load<GameObject>(itemName + "_Model"), new Vector3(0.51f, 0.48f, 0.75f), Quaternion.Euler(-162.84f, -23.69f, -67.78f));
        itemModel.transform.localScale = new Vector3(3, 3, 3);
        itemModel.transform.SetParent(toolHolder.transform, false);
    }

    private bool CheckIfSlotIsFull(int number)
    {
        if (quickSlotsList[number - 1].transform.childCount > 0) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
            if(child.CompareTag("Frame"))
            {
                frameHolder.Add(child.gameObject);
            }
        }
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        // Find next free slot
        GameObject availableSlot = FindNextEmptySlot();
        // Set transform of our object
        itemToEquip.transform.SetParent(availableSlot.transform, false);
        // Getting clean name
        string cleanName = itemToEquip.name.Replace("(Clone)", "");
        // Adding item to list
        itemList.Add(cleanName);

        InventorySystem.Instance.RecalculateList();

    }


    public GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {

        int counter = 0;

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }

        if (counter == quickSlotsList.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}