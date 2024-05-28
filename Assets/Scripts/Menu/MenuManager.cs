using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; set; }

    public GameObject menuCanvas;
    public GameObject uiCanvas;
    public bool isMenuOpen;

    public GameObject menu;
    public GameObject saveMenu;
    public GameObject settings;

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

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !isMenuOpen) 
        {
            menuCanvas.SetActive(true);
            uiCanvas.SetActive(false);
            
            isMenuOpen = true;


            Cursor.lockState = CursorLockMode.None;   
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isMenuOpen)
        {
            menuCanvas.SetActive(false);
            uiCanvas.SetActive(true);
            isMenuOpen = false;

            saveMenu.SetActive(false);
            settings.SetActive(false);
            
            menu.SetActive(true);

            if (!InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
        }
    }

    public void TempSaveGame()
    {
        SaveManager.Instance.SaveGame();
    }
}
