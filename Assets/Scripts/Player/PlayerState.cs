using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerState : MonoBehaviour
{

    public static PlayerState Instance { get; set; }

    public GameObject gameOverUI;

    //---------  Player Health  -----------
    public float currentHealth;
    public float maxHealth;


    //---------  Player Calories  -----------
    public float currentCalories;
    public float maxCalories;

    //---------  Player Hydration  -----------
    public float currentHydrationPercent;
    public float maxHydrationPercent;

    private float distanceTravelled = 0;
    private Vector3 lastPostion;
    public GameObject player;


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
        StartCoroutine(decreaseHydration());

    }

    private IEnumerator decreaseHydration()
    {
        while(true)
        {
            
            if(currentHydrationPercent > 0) 
            {
                currentHydrationPercent -= 1;
            }
            else if (currentCalories == 0 && currentHealth > 0)
            {
                currentHealth -= 2;
            }
            else if(currentHydrationPercent == 0 && currentHealth > 0)
            {
                currentHealth -= 2;
            }
            if(currentHealth <= 0)
            {
                gameOverUI.SetActive(true);
                Invoke("Restart", 3f);

            }
            yield return new WaitForSeconds(7);
        }
    }

    public void SetHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    public void SetCalories(float newCalories)
    {
        currentCalories = newCalories;
    }

    public void SetHydration(float newHydration)
    {
        currentHydrationPercent = newHydration;
    }

    private void Restart()
    {
        SceneManager.LoadScene("GameScene");
    }

    void Update()
    {
        distanceTravelled += Vector3.Distance(player.transform.position, lastPostion);
        lastPostion = player.transform.position;
        if(distanceTravelled >= 5)
        {
            if (currentCalories > 0)
            {
                distanceTravelled = 0;
                currentCalories -= 1;
            }
            else
            {
                currentCalories = 0;
            }
        }
        if (Input.GetKey(KeyCode.N))
        {
            currentHealth -= 10;
        }
    }
}
