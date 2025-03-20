using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawerController : MonoBehaviour
{
    public Transform drawerTransform; // Assign the drawer part of the object that moves
    public Vector3 openPosition; // The position the drawer moves to when opened (relative)
    public Vector3 closedPosition; // The position when the drawer is closed (relative)
    public float openSpeed = 2f; // Speed at which the drawer opens and closes
    public float interactionDistance = 5f; // Max distance from the player to interact with the drawer

    private bool isOpen = false; // Track whether the drawer is open or closed
    private bool isMoving = false; // To prevent opening/closing while the drawer is already moving
    private Transform playerCamera; // Reference to the player's camera

    void Start()
    {
        playerCamera = Camera.main.transform; // Assume the main camera is the player's camera
        closedPosition = drawerTransform.localPosition; // Set the default closed position
    }

    void Update()
    {
        // Check if the player is looking at the drawer and within interaction distance
        if (IsPlayerLookingAtDrawer() && Input.GetMouseButtonDown(0)) // Left-click to interact
        {
            if (!isMoving) // Ensure the drawer isn't currently moving
            {
                StartCoroutine(ToggleDrawer());
            }
        }
    }

    // Coroutine to handle opening and closing of the drawer
    IEnumerator ToggleDrawer()
    {
        isMoving = true;

        Vector3 targetPosition = isOpen ? closedPosition : openPosition;
        while (Vector3.Distance(drawerTransform.localPosition, targetPosition) > 0.01f)
        {
            drawerTransform.localPosition = Vector3.Lerp(drawerTransform.localPosition, targetPosition, Time.deltaTime * openSpeed);
            yield return null;
        }

        // Set final position to avoid overshoot
        drawerTransform.localPosition = targetPosition;
        isOpen = !isOpen; // Toggle the open state
        isMoving = false;
    }

    // Check if the player is looking at the drawer and is close enough
    bool IsPlayerLookingAtDrawer()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Perform raycast to see if the player is looking at the drawer
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            return hit.transform == transform; // Return true if ray hits the drawer object
        }

        return false;
    }
}
