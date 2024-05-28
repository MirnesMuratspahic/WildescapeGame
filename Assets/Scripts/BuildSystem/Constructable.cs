using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constructable : MonoBehaviour
{
    // Validation
    public bool isGrounded;
    public bool isOverlappingItems;
    public bool isValidToBeBuilt;


    public BoxCollider solidCollider;

    private void Start()
    {

    }
    void Update()
    {
        if (isGrounded && isOverlappingItems == false)
        {
            isValidToBeBuilt = true;
        }
        else
        {
            isValidToBeBuilt = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") && gameObject.CompareTag("activeConstructable"))
        {
            isGrounded = true;
        }

        if (other.CompareTag("Tree") || other.CompareTag("pickable") && gameObject.CompareTag("activeConstructable"))
        {

            isOverlappingItems = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground") && gameObject.CompareTag("activeConstructable"))
        {
            isGrounded = false;
        }

        if (other.CompareTag("Tree") || other.CompareTag("pickable") && gameObject.CompareTag("activeConstructable"))
        {
            isOverlappingItems = false;
        }

    }
}