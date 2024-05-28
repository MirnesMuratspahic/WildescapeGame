using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacebleItem : MonoBehaviour
{
    // Validation
    [SerializeField] bool isGrounded;
    [SerializeField] bool isOverlappingItems;
    public bool isValidToBeBuilt;

    [SerializeField] BoxCollider solidCollider;

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

        // Raycast from the box's position towards its center

        var boxHeight = transform.lossyScale.y;
        RaycastHit groundHit;
        if (Physics.Raycast(transform.position, Vector3.down, out groundHit, boxHeight * 0.5f, LayerMask.GetMask("Ground")))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

    }

    #region || --- On Triggers --- |
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") && PlacementSystem.Instance.inPlacementMode)
        {
            // Making sure the item is parallel to the ground
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                // Align the box's rotation with the ground normal
                Quaternion newRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                transform.rotation = newRotation;

                isGrounded = true;
            }
        }

        if (other.CompareTag("Tree") || other.CompareTag("pickable"))
        {
            isOverlappingItems = true;
        }
    }
    #endregion

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground") && PlacementSystem.Instance.inPlacementMode)
        {
            isGrounded = false;
        }

        if (other.CompareTag("Tree") || other.CompareTag("pickable") && PlacementSystem.Instance.inPlacementMode)
        {
            isOverlappingItems = false;
        }
    }

}