using Cinemachine.PostFX;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SaveManager;


public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; set; }

    public Slider masterSlider;
    public GameObject masterVolume;

    public Slider effectsSlider;
    public GameObject effectsVolume;

    public Button btnApply;

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

    public void Start()
    {
        btnApply.onClick.AddListener(() =>
        {
            SaveManager.Instance.SaveVolumeSettings(effectsSlider.value, masterSlider.value);
            Debug.Log("Saved");
        });

        StartCoroutine(LoadAndApplySettings());
    }

    private IEnumerator LoadAndApplySettings()
    {
        LoadAndSetVolume();

        yield return new WaitForSeconds(0.1f);
    }

    private void LoadAndSetVolume()
    {
        VolumeSettings volumeSettings = SaveManager.Instance.LoadVolumeSettings();
        if(volumeSettings != null)
        {
            masterSlider.value = volumeSettings.master;
            effectsSlider.value = volumeSettings.effects;
        }
        else
        {
            masterSlider.value = 0;
            effectsSlider.value = 0;
        }
        
    }
}
