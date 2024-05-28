using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquippableItem : MonoBehaviour
{

    public Animator animator;
    public bool isAnimating;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) && !ConstructionManager.Instance.inConstructionMode) 
        {
            animator.SetTrigger("Idle");
        }
        else if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) && !ConstructionManager.Instance.inConstructionMode)
        {
            SoundManager.Instance.walkingOnGrass.Stop();
            animator.ResetTrigger("Idle");
            if (Input.GetMouseButtonDown(0) && !InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpen && !SelectionManager.Instance.isSelected &&
            !MenuManager.Instance.isMenuOpen && !isAnimating && !ConstructionManager.Instance.inConstructionMode)
            {
                GameObject selectedTree = SelectionManager.Instance.selectedTree;
                if (selectedTree != null)
                {
                    isAnimating = true;
                    selectedTree.GetComponent<ChoppableTree>().GetHit();
                    StartCoroutine(ResetIsAnimatingAfterDelay(1.7f));
                }
                animator.SetTrigger("Hit");

            }
        }
        

    }

    private IEnumerator ResetIsAnimatingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAnimating = false;
    }
}
