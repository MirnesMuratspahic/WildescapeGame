using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : MonoBehaviour
{
    public static ConstructionManager Instance { get; set; }

    public GameObject itemToBeConstructed;
    public bool inConstructionMode = false;
    public GameObject constructionHoldingSpot;
    public GameObject itemToBeDestroyed;
    private Quaternion initialRotation;
    private float number1;

    public bool isValidPlacement;


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

    public void ActivateConstructionPlacement(string itemToConstruct)
    {
        GameObject item = Instantiate(Resources.Load<GameObject>(itemToConstruct));

        item.name = itemToConstruct;

        item.transform.SetParent(constructionHoldingSpot.transform, false);
        itemToBeConstructed = item;
        initialRotation = itemToBeConstructed.transform.rotation;
        itemToBeConstructed.gameObject.tag = "activeConstructable";


        itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = false;


        inConstructionMode = true;
    }


    private void Update()
    {

        if (itemToBeConstructed != null && inConstructionMode)
        {
            if (CheckValidConstructionPosition())
            {
                isValidPlacement = true;
            }
            else
            {
                isValidPlacement = false;
            }

        }


        if (Input.GetKey(KeyCode.Q) && inConstructionMode)
        {
            number1 += 0.1f;
            if (number1 < 30)
            {
                Vector3 newPosition = itemToBeConstructed.transform.position;
                newPosition.y += 0.1f;
                itemToBeConstructed.transform.position = newPosition;
            }


        }
        else if (Input.GetKey(KeyCode.E) && inConstructionMode)
        {

            number1 -= 0.1f;
            if (number1 > -30)
            {
                Vector3 newPosition = itemToBeConstructed.transform.position;
                newPosition.y -= 0.1f;
                itemToBeConstructed.transform.position = newPosition;
            }

        }

        if (Input.GetMouseButtonDown(0) && inConstructionMode)
        {
            if (isValidPlacement)
            {
                InventorySystem.Instance.pickedUpItems.Add(itemToBeConstructed.transform.name);
                PlaceItemFreeStyle2();
                DestroyItem(itemToBeDestroyed);
            }
        }             
        if (Input.GetKey(KeyCode.Delete) && inConstructionMode)
        {
            Debug.Log(itemToBeDestroyed.name);
            itemToBeDestroyed.SetActive(true);
            itemToBeDestroyed = null;

            DestroyItem(itemToBeConstructed);
            itemToBeConstructed = null;
            inConstructionMode = false;
        }
    }

    private void PlaceItemFreeStyle2()
    {
 
        GameObject terrain = GameObject.Find("Environment");

        if (terrain != null)
        {

            Transform objectsParent = terrain.transform.Find("Items");

            if (objectsParent != null)
            {

                itemToBeConstructed.transform.SetParent(objectsParent, true);

                itemToBeConstructed.tag = "placedFoundation";
                itemToBeConstructed.GetComponent<Constructable>().enabled = false;

                itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = true;

                itemToBeConstructed = null;
                inConstructionMode = false;
            }
            else
            {
                Debug.LogError("Objects parent not found under Terrain.");
            }
        }
        else
        {
            Debug.LogError("Terrain object not found.");
        }
    }

    private bool CheckValidConstructionPosition()
    {
        if (itemToBeConstructed != null)
        {
            return itemToBeConstructed.GetComponent<Constructable>().isValidToBeBuilt;
        }

        return false;
    }

    public void DestroyItem(GameObject item)
    {
        DestroyImmediate(item);
        InventorySystem.Instance.RecalculateList();
        CraftingSystem.Instance.Refresh();
    }
}