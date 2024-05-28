
using Assets.Scripts;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject craftingScreenUI;
    public List<string> inventoryItemList = new List<string>();
    public List<GameObject> craftingOptions = new List<GameObject>();
    public List<GameObject> textReq = new List<GameObject>();

    public List<RequirementItem> numberOfEachItemInInventory = new List<RequirementItem>()
    {
        new RequirementItem("Axe"), new RequirementItem("Stone"),
        new RequirementItem("Stick"), new RequirementItem("Rope"),
        new RequirementItem("SharpStone"), new RequirementItem("Hemp"),
        new RequirementItem("CampFire"), new RequirementItem("Foundation"),
        new RequirementItem("Wall") , new RequirementItem("Log"),
        new RequirementItem("StorageBox")
    };
   

    public Button btnNext;
    public Button btnPrevious;
    public Button btnAxe;
    public Button btnSharpStone;
    public Button btnRope;
    public Button btnCampFire;
    public Button btnFoundation;
    public Button btnWall;
    public Button btnStorageBox;




    public bool isOpen;
    public int fromOption;


    //All blueprints---------------

    public Blueprint AxeBlp = new Blueprint("Axe", new List<RequirementItem>() { new RequirementItem("SharpStone", 1), new RequirementItem("Stick", 1), new RequirementItem("Rope", 1) });
    public Blueprint SharpStoneBlp = new Blueprint("SharpStone", new List<RequirementItem>() { new RequirementItem("Stone", 1) });
    public Blueprint RopeBlp = new Blueprint("Rope", new List<RequirementItem>() { new RequirementItem("Hemp", 5) });
    public Blueprint CampFireBlp = new Blueprint("CampFire", new List<RequirementItem>() { new RequirementItem("Stone", 7), new RequirementItem("Stick", 5) });
    public Blueprint FoundationBlp = new Blueprint("Foundation", new List<RequirementItem>() { new RequirementItem("Log", 5), new RequirementItem("Rope", 3) });
    public Blueprint WallBlp = new Blueprint("Wall", new List<RequirementItem>() { new RequirementItem("Log", 5), new RequirementItem("Rope", 2) });
    public Blueprint StorageBoxBlp = new Blueprint("StorageBox", new List<RequirementItem>() { new RequirementItem("Log", 2), new RequirementItem("Stick", 2), new RequirementItem("Rope", 3) });




    public static CraftingSystem Instance {get; set;}

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
    }

    void Start()
    {
        Cursor.visible = false;
        fromOption = 0;
        isOpen = false;
        PopulateOptionsList();
        ShowFirstFourOptions();
        NumberOfEachItemInInventory();

        CanICraftItem(AxeBlp, btnAxe);
        CanICraftItem(SharpStoneBlp, btnSharpStone);
        CanICraftItem(RopeBlp, btnRope);
        CanICraftItem(CampFireBlp, btnCampFire);
        CanICraftItem(FoundationBlp, btnFoundation);
        CanICraftItem(WallBlp, btnWall);
        CanICraftItem(StorageBoxBlp, btnStorageBox);




        btnNext.interactable = true;
        btnPrevious.interactable = false;

        btnNext.onClick.AddListener(delegate { ShowNextOptions(); });
        btnPrevious.onClick.AddListener(delegate { ShowPreviousOptions(); });


        btnAxe.onClick.AddListener(delegate { CraftAnyItem(AxeBlp, btnAxe); });
        btnSharpStone.onClick.AddListener(delegate { CraftAnyItem(SharpStoneBlp, btnSharpStone); });
        btnRope.onClick.AddListener(delegate { CraftAnyItem(RopeBlp, btnRope); });
        btnCampFire.onClick.AddListener(delegate { CraftAnyItem(CampFireBlp, btnCampFire); });
        btnFoundation.onClick.AddListener(delegate { CraftAnyItem(FoundationBlp, btnFoundation); });
        btnWall.onClick.AddListener(delegate { CraftAnyItem(WallBlp, btnWall); });
        btnStorageBox.onClick.AddListener(delegate { CraftAnyItem(StorageBoxBlp, btnStorageBox); });



    }


    public void CraftAnyItem(Blueprint itemToCraft, Button button)
    {
        if (!InventorySystem.Instance.CheckIfInventoryIsFull())
        {
            CanICraftItem(itemToCraft, button);

            InventorySystem.Instance.AddToInventory(itemToCraft.Name);

            InventorySystem.Instance.RemoveItem(itemToCraft.Requirements);


            Invoke("Refresh", 0.1f);
        }
        else
        {
            Debug.Log("Inventory is full");
        }
    }

    public void NumberOfEachItemInInventory()
    {
        ResetNumberOfItems();
        InventorySystem.Instance.RecalculateList();
        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (var item in inventoryItemList)
        {
            for (int i = 0; i < numberOfEachItemInInventory.Count; i++)
            {
                if (item == numberOfEachItemInInventory[i].RequirementName)
                {
                    numberOfEachItemInInventory[i].RequirementAmount++;
                }
            }
            
        }
    }

    public void ResetNumberOfItems()
    {
        for (int i = 0; i < numberOfEachItemInInventory.Count; i++)
        {
            numberOfEachItemInInventory[i].RequirementAmount = 0;
        }
    }

    public void UpdateRequirements(string name)
    {

        textReq.Clear();
        Transform firstChild = craftingScreenUI.transform.Find(name);
        Debug.Log(name);
        foreach (Transform child in firstChild)
        {
            if (child.CompareTag("ReqText"))
            {
                textReq.Add(child.gameObject);
            }
        }

        if (textReq.Count != 0)
        {
            string originalText = string.Empty;
            string nameOfItem = string.Empty;
            string newText = string.Empty;

            for (int i = 0; i < textReq.Count; i++)
            {
                originalText = textReq[i].transform.GetComponent<TextMeshProUGUI>().text;
                nameOfItem = originalText.Replace(" ", "");
                nameOfItem = nameOfItem.Substring(1, nameOfItem.Length - 4).ToLower();
                for (int j = 0; j < numberOfEachItemInInventory.Count; j++)
                {
                    if (nameOfItem == numberOfEachItemInInventory[j].RequirementName.ToLower())
                    {
                        newText = Regex.Replace(originalText, @"\[\d+\]", $"[{numberOfEachItemInInventory[j].RequirementAmount}]");
                        textReq[i].GetComponent<TextMeshProUGUI>().text = newText;
                    }
                }
            }
        }
    }

    public void Refresh()
    {
        CanICraftItem(AxeBlp, btnAxe);
        CanICraftItem(SharpStoneBlp, btnSharpStone);
        CanICraftItem(RopeBlp, btnRope);
        CanICraftItem(CampFireBlp, btnCampFire);
        CanICraftItem(FoundationBlp, btnFoundation);
        CanICraftItem(WallBlp, btnWall);
        CanICraftItem(StorageBoxBlp, btnStorageBox);
    }

    public void CanICraftItem(Blueprint itemToCraft, Button button)
    {
        NumberOfEachItemInInventory();
        UpdateRequirements(itemToCraft.Name);

        int number = 0;
        for (int i = 0; i < itemToCraft.Requirements.Count; i++) 
        {
            for(int j = 0; j < numberOfEachItemInInventory.Count; j++)
            {
                if(itemToCraft.Requirements[i].RequirementName == numberOfEachItemInInventory[j].RequirementName
                    && itemToCraft.Requirements[i].RequirementAmount <= numberOfEachItemInInventory[j].RequirementAmount)
                {
                    number++;
                }
            }
        }

        if (number == itemToCraft.Requirements.Count)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }

    }

    public void PopulateOptionsList()
    {
        foreach (Transform child in craftingScreenUI.transform)
        {
            if (child.CompareTag("CraftingOption"))
            {
                craftingOptions.Add(child.gameObject);
            }
        }
    }

    public void ShowFirstFourOptions()
    {
        for (int i = 0; i < craftingOptions.Count; i++)
        {
            if(i < 4)
                craftingOptions[i].SetActive(true);
            else
                craftingOptions[i].SetActive(false);
        }
    }

    public void ShowNextOptions()
    {
        fromOption += 4;

        UpdateButtonsInteractivity();
        foreach (var craftingOption in craftingOptions)
        {
            craftingOption.SetActive(false);
        }

        for (int i = fromOption; i < fromOption + 4; i++) 
        {
            if (i < craftingOptions.Count) 
            {
                craftingOptions[i].SetActive(true);
            }
            else
            {
                return;
            }
        }

    }

    public void ShowPreviousOptions()
    {
        fromOption -= 4;
        UpdateButtonsInteractivity();
        foreach (var craftingOption in craftingOptions)
        {
            craftingOption.SetActive(false);
        }

        for (int i = fromOption; i < fromOption + 4; i++)
        {
            if (i < craftingOptions.Count)
            {
                craftingOptions[i].SetActive(true);
            }
            else
            {
                return;
            }
        }

    }

    void UpdateButtonsInteractivity()
    {
        
        if (fromOption + 4 >= craftingOptions.Count)
            btnNext.interactable = false; 
        else
            btnNext.interactable = true; 


        if (fromOption > 0)
            btnPrevious.interactable = true;
        else
            btnPrevious.interactable = false;
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
        {
            Cursor.visible = true;
            craftingScreenUI.SetActive(true);
            isOpen = true;
            Cursor.lockState = CursorLockMode.None;
            NumberOfEachItemInInventory();
            CanICraftItem(AxeBlp, btnAxe);
            CanICraftItem(SharpStoneBlp, btnSharpStone);
            CanICraftItem(RopeBlp, btnRope);
            CanICraftItem(CampFireBlp, btnCampFire);
            CanICraftItem(FoundationBlp, btnFoundation);
            CanICraftItem(WallBlp, btnWall);
            CanICraftItem(StorageBoxBlp, btnStorageBox);


            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            
            craftingScreenUI.SetActive(false);
            isOpen = false;


            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;

            
        }


    }
}

