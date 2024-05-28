using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LakeBehaviour : MonoBehaviour
{
    public bool playerInRange;

    void Start() { }

    void Update()
    {
        if (playerInRange)
        {
            SoundManager.Instance.walkingOnGrass.Stop();
            SoundManager.Instance.backgroundSound.Stop();
            SoundManager.Instance.PlaySound(SoundManager.Instance.underwater);
        }
        else
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.backgroundSound);
            SoundManager.Instance.underwater.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }


}
