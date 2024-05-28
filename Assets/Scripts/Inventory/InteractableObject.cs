using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string ItemName;
    public bool PlayerInRange;
    public bool isChoppabelTree;

    public string GetItemName()
    {
        return ItemName;
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) &&  PlayerInRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject)
        {
            if (!InventorySystem.Instance.CheckIfInventoryIsFull() && !gameObject.CompareTag("Water"))
            {
                InventorySystem.Instance.AddToInventory(ItemName);
                InventorySystem.Instance.pickedUpItems.Add(gameObject.name);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory is full!");
            }
        }
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerInRange = false;
        }
    }
}
