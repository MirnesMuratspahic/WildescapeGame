using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button loadGameBtn;

    private void Start()
    {
        loadGameBtn.onClick.AddListener(() => { SaveManager.Instance.StartGame(); });
    }

   public void NewGame()
    {
        SceneManager.LoadScene("Intro");
    }

    public void ExitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}
