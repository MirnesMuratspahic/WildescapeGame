using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    public static PlacementSystem Instance { get; set; }

    public GameObject placementHoldingSpot; // Drag our construcionHoldingSpot or a new placementHoldingSpot
    public GameObject enviromentPlaceables;
    private float number1;

    public bool inPlacementMode;
    [SerializeField] bool isValidPlacement;

    [SerializeField] public GameObject itemToBePlaced;
    [SerializeField] public GameObject inventoryItemToDestory;

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

    public void ActivatePlacementMode(string itemToPlace)
    {
        
        GameObject item = Instantiate(Resources.Load<GameObject>(itemToPlace));

        // Changing the name of the gameobject so it will not be (clone)
        item.name = itemToPlace;

        // Setting the item to be a child of our placement holding spot
        item.transform.SetParent(placementHoldingSpot.transform, false);

        // Saving a reference to the item we want to place
        itemToBePlaced = item;

        

        // Actiavting Construction mode
        inPlacementMode = true;
    }



    private void Update()
    {



        if (itemToBePlaced != null && inPlacementMode)
        {
            if (IsCheckValidPlacement())
            {
                isValidPlacement = true;
            }
            else
            {
                isValidPlacement = false;
            }
        }

        if (Input.GetKey(KeyCode.Q) && inPlacementMode)
        {
            number1 += 0.1f;
            if (number1 < 30)
            {
                Vector3 newPosition = itemToBePlaced.transform.position;
                newPosition.y += 0.1f;
                itemToBePlaced.transform.position = newPosition;
            }


        }
        else if (Input.GetKey(KeyCode.E) && inPlacementMode)
        {

            number1 -= 0.1f;
            if (number1 > -30)
            {
                Vector3 newPosition = itemToBePlaced.transform.position;
                newPosition.y -= 0.1f;
                itemToBePlaced.transform.position = newPosition;
            }

        }

        // Left Mouse Click to Place item
        if (Input.GetMouseButtonDown(0) && inPlacementMode && isValidPlacement)
        {
            PlaceItemFreeStyle();
            DestroyItem(inventoryItemToDestory);
        }

        // Cancel Placement                     
        if (Input.GetKeyDown(KeyCode.Delete) && inPlacementMode)
        {
            inventoryItemToDestory.SetActive(true);
            inventoryItemToDestory = null;
            DestroyItem(itemToBePlaced);
            itemToBePlaced = null;
            inPlacementMode = false;
        }
    }

    private bool IsCheckValidPlacement()
    {
        if (itemToBePlaced != null)
        {
            return itemToBePlaced.GetComponent<PlacebleItem>().isValidToBeBuilt;
        }

        return false;
    }

    private void PlaceItemFreeStyle()
    {
        // Setting the parent to be the root of our scene
        itemToBePlaced.transform.SetParent(enviromentPlaceables.transform, true);

        // Setting the default color/material
        itemToBePlaced.GetComponent<PlacebleItem>().enabled = false;

        itemToBePlaced = null;

        inPlacementMode = false;
        StartCoroutine(Delay());
    }

    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        inPlacementMode = false;
    }

    private void DestroyItem(GameObject item)
    {
        DestroyImmediate(item);
        InventorySystem.Instance.RecalculateList();
        CraftingSystem.Instance.Refresh();
    }
}
