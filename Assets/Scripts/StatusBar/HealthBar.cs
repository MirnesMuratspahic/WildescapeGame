#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build.Content;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;

public class HelthBar : MonoBehaviour
{

    private Slider slider;
    public TextMeshProUGUI healthCounter;

    public GameObject playerState;

    private float currentHealth, maxHealth;

    void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = playerState.GetComponent<PlayerState>().currentHealth;
        maxHealth = playerState.GetComponent<PlayerState>().maxHealth;

        float fillValue = currentHealth / maxHealth;
        slider.value = fillValue;

        healthCounter.text = currentHealth + "/" + maxHealth;
    }
}
#endif
