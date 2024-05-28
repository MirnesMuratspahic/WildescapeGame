using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance { get; set; }

    public GameObject ItemInfoUI;
    public GameObject inventoryScreenUI;
    public List<GameObject> slotList = new List<GameObject>();
    public List<string> itemList = new List<string>();
    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;
    public List<string> pickedUpItems;

    public bool isOpen;




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


    void Start()
    {
        Cursor.visible = false;
        isOpen = false;
        PopulateSlotList();
        RecalculateList();
    }

    public void PopulateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform) 
        {
            if(child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
        {

            Cursor.visible = true;
            inventoryScreenUI.SetActive(true);
            isOpen = true;
            Cursor.lockState = CursorLockMode.None;
            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            isOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
        }

    }

    public void AddToInventory(string itemName)
    {
         whatSlotToEquip = FindNextEmptySlot();
         itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
         itemToAdd.transform.SetParent(whatSlotToEquip.transform);
         itemList.Add(itemName);
        SoundManager.Instance.PlaySound(SoundManager.Instance.dropItemSound);
    }

    public GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in slotList)
        {
            if(slot.transform.childCount == 0)
            {
                return slot;
            }
        }

        return new GameObject();
    }

    public void RemoveItem(List<RequirementItem> items)
    {

        for(int i=0; i < items.Count; i++)
        {
            int number = (int)items[i].RequirementAmount;
            for (int j = slotList.Count - 1; j >= 0; j--)
            {
                if (slotList[j].transform.childCount > 0)
                {
                    if (slotList[j].transform.GetChild(0).name.ToLower() == (items[i].RequirementName + "(Clone)").ToLower() && number != 0)
                    {
                        DestroyImmediate(slotList[j].transform.GetChild(0).gameObject);
                        number -= 1;
                    }
                }
            }
        }
    }


    public void RecalculateList()
    {
        itemList.Clear();

        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");
                itemList.Add(result);
            }
        }
    }

    public bool CheckIfInventoryIsFull()
    {
        int counter = 0;

        foreach (GameObject slot in slotList)
        {
            if( slot.transform.childCount > 0)
            {
                counter++;
            }
        }

        if (counter == slotList.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}