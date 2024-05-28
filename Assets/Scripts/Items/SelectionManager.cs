using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set; }

    public GameObject interaction_Info_UI;
    public Text interaction_text;
    public bool onTarget;
    public GameObject selectedObject;
    public bool isSelected;

    public GameObject selectedTree;
    public GameObject chopHolder;

    public GameObject selectedStorageBox;

    private void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<Text>();
    }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();

            if(choppableTree != null && choppableTree.playerInRange)
            {
                choppableTree.canBeChopped = true;
                selectedTree = choppableTree.gameObject;
            }
            else if(choppableTree != null)
            {
                selectedTree = choppableTree.gameObject;
                choppableTree.canBeChopped = false;
                selectedTree = null;

            }

            if (selectionTransform.GetComponent<InteractableObject>() && selectionTransform.GetComponent<InteractableObject>().PlayerInRange
                && !selectionTransform.GetComponent<InteractableObject>().isChoppabelTree)
            {
                selectedObject = selectionTransform.GetComponent<InteractableObject>().gameObject;
                onTarget = true;
                interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                interaction_Info_UI.SetActive(true);
                isSelected = true;
            }
            else
            {
                onTarget = false;
                interaction_Info_UI.SetActive(false);
                isSelected = false;
            }

            StorageBox storageBox = selectionTransform.GetComponent<StorageBox>();

            if(storageBox && storageBox.playerInRange && !PlacementSystem.Instance.inPlacementMode)
            {
                interaction_text.text = "Open";
                interaction_Info_UI.SetActive(true);

                selectedStorageBox = storageBox.gameObject;

                if(Input.GetMouseButtonDown(0))
                {
                    StorageManager.Instance.OpenBox(storageBox);
                }
            }
            else if(selectedStorageBox != null) 
            {
                selectedStorageBox = null;
            }

        }
        else
        {
            onTarget = false;
            interaction_Info_UI.SetActive(false);
            isSelected = false;
            chopHolder.gameObject.SetActive(false);
        }
    }

    public void EnableSelection()
    {
        interaction_Info_UI.SetActive(true);
        interaction_text.enabled = true;
    }

    public void DisableSelection()
    {
        interaction_Info_UI.SetActive(false);
        selectedObject = null;
        interaction_text.enabled = false;
        
    }
}
