using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance { get;  set; }
    public AudioSource dropItemSound;
    public AudioSource choppingTree;
    public AudioSource backgroundSound;
    public AudioSource walkingOnGrass;
    public AudioSource fallingTree;
    public AudioSource eatingFood;
    public AudioSource underwater;
    public AudioSource drinkWater;





    void Start()
    {
        dropItemSound.volume = SettingsManager.Instance.effectsSlider.value / 100;
        choppingTree.volume = SettingsManager.Instance.effectsSlider.value / 100;
        walkingOnGrass.volume = SettingsManager.Instance.effectsSlider.value / 100;
        fallingTree.volume = SettingsManager.Instance.effectsSlider.value / 100;
        eatingFood.volume = SettingsManager.Instance.effectsSlider.value / 100;
        underwater.volume = SettingsManager.Instance.effectsSlider.value / 100;
        drinkWater.volume = SettingsManager.Instance.effectsSlider.value / 100;




        backgroundSound.volume = SettingsManager.Instance.masterSlider.value / 100;
    }

    void Update()
    {
        dropItemSound.volume = SettingsManager.Instance.effectsSlider.value / 100;
        choppingTree.volume = SettingsManager.Instance.effectsSlider.value / 100;
        walkingOnGrass.volume = SettingsManager.Instance.effectsSlider.value / 100;
        fallingTree.volume = SettingsManager.Instance.effectsSlider.value / 100;
        eatingFood.volume = SettingsManager.Instance.effectsSlider.value / 100;
        underwater.volume = SettingsManager.Instance.effectsSlider.value / 100;
        drinkWater.volume = SettingsManager.Instance.effectsSlider.value / 100;


        backgroundSound.volume = SettingsManager.Instance.masterSlider.value / 100;
    }

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

    public void PlaySound(AudioSource soundToPlay)
    {
        if(!soundToPlay.isPlaying) 
        {
            soundToPlay.Play();
        }
    }
}
