using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LakeSystem : MonoBehaviour
{
    public bool PlayerInRange;
    public GameObject interaction_Info_UI;
    public Text interaction_text;

    void Start()
    {
        interaction_text = interaction_Info_UI.GetComponent<Text>();
    }

    void Update()
    {
        if (PlayerInRange)
        {
            
            interaction_text.text = "Click to drink water";
            interaction_Info_UI.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.drinkWater);
                if (PlayerState.Instance.currentHydrationPercent + 30 > 100)
                    PlayerState.Instance.currentHydrationPercent = 100;
                else
                    PlayerState.Instance.currentHydrationPercent += 30;
                StartCoroutine(Delay());
            }
        }
        else
        {
            interaction_Info_UI.SetActive(false);
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = false;
        }
    }
}
