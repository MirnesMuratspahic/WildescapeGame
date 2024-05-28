using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ChoppableTree : MonoBehaviour
{
    public bool playerInRange;
    public bool canBeChopped;
    public Animator animator;

    public float treeMaxHealth;
    public float treeHealth;

    private GameObject selectedTree;
    private Quaternion treeRotation;




    void Start()
    {
        treeHealth = treeMaxHealth;
        animator = transform.parent.transform.parent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canBeChopped)
        {
            GlobalState.Instance.resourceHealth = treeHealth;
            GlobalState.Instance.resourceMaxHealth = treeMaxHealth;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void GetHit()
    {
        StartCoroutine(HitTree());
    }

    private IEnumerator HitTree()
    {
        yield return new WaitForSeconds(0.6f);
        treeHealth -= 1;
        PlayerState.Instance.currentCalories -= 5;
        SoundManager.Instance.PlaySound(SoundManager.Instance.choppingTree);
        Debug.Log(treeHealth);
        if (treeHealth <= 0)
        {
            selectedTree = SelectionManager.Instance.selectedTree;
            if(selectedTree.transform.parent.name != "Tree 5")
            {
                FallTree();
                Invoke("CompleteTreeFall", 3f);
            }
            else
            {
                CompleteTreeFall();
            }
        }
    }

    private void FallTree()
    {
        
        if(selectedTree.transform.parent.name == "Tree 9")
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.fallingTree);
            animator.SetTrigger("Fall");
        }
        else if(selectedTree.transform.parent.name == "Tree 7")
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.fallingTree);
            animator.SetTrigger("FallSmall");
        }
        treeRotation = transform.parent.transform.parent.transform.rotation;
    }

    private void CompleteTreeFall()
    {
        Vector3 treePosition = transform.position;
        Quaternion newTreeRotation = treeRotation;
        if(selectedTree.transform.parent.name == "Tree 9")
        {
             treePosition.y += 1.0f;
        }
        else if(selectedTree.transform.parent.name == "Tree 7")
        {
            treePosition.y -= 1.5f;
        }
        canBeChopped = false;

        
            string selectedTreeParentName = selectedTree.transform.parent.name;

            if (selectedTreeParentName == "Tree 9")
            {
                GameObject brokenTree = Instantiate(Resources.Load<GameObject>("BigChoppedTree"), treePosition, newTreeRotation);
                
            }
            else if (selectedTreeParentName == "Tree 7")
            {
                GameObject brokenTree = Instantiate(Resources.Load<GameObject>("SmallChoppedTree"), treePosition, newTreeRotation);
               
            }
            else if (selectedTreeParentName == "Tree 5")
            {
                GameObject brokenTree = Instantiate(Resources.Load<GameObject>("StickChoppedTree"), treePosition, newTreeRotation);
                
            }

        InventorySystem.Instance.pickedUpItems.Add(selectedTree.gameObject.name);
        Debug.Log(selectedTree.gameObject.name);
        Destroy(selectedTree.transform.parent.transform.parent.gameObject);
        selectedTree = null;
    }

    //private IEnumerator FallTree() 
    //{
    //    animator.SetTrigger("Fall");
    //    yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    //}
}
