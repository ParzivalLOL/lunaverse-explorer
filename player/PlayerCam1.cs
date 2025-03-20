using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // For UI interaction detection

public class PlayerCam1 : MonoBehaviour
{
    public float outlineWidth;
    public float sensitivityX;
    public float sensitivityY;
    public Transform orientation;
    public Transform camHolder;
    public GameObject player;

    private float xRot;
    private float yRot;
    public Transform highlight;
    private RaycastHit raycastHit;

    public float SelectableDistance = 10f;
    public LayerMask selectableLayer;
    public LayerMask inspectableLayer;
    public Vector3 inspectionOffset;
    private Vector3 inspectionOriginalPos;
    private Transform inspectionObj;

    private bool canSelect;
    public float buttonSpeed = 0.02f;
    public float moveDist = 1f;
    private bool isInteractingWithUI = false; // To track UI interaction

    private Vector3 originalPos;

    // List of possible selectable tags
    public string[] selectableTags;

    // Reference to the UI Canvas
    public GameObject oxygenInteractionUI; // Drag your UI Canvas or Panel here in Unity
    public GameObject pwrFailUI;
    // public GameObject commsUI;
    // public GameObject commsPlayerWave;
    // public GameObject commsTargetWave;
    public Transform seat;
    private bool isSeated;

    // Variable to freeze player movement when UI is open
    private bool isOxyUIOpen = false;
    private bool isPwrUIOpen = false;
    private bool isInspecting = false;
    // private bool isCommsUIOpen = false;

    // Drawer pulling configuration
    public float drawerPullDistance = 0.5f; // Distance to pull the drawer
    public float drawerPullSpeed = 2f;      // Speed of pulling the drawer
    private bool isDrawerPulled = false;    // Track whether the drawer is pulled

    // Add camera movement variables
    public Transform oxygenTargetPosition;  // The camera target position for Oxygen UI
    public Transform powerTargetPosition;   // The camera target position for Power UI
    // public Transform commsTargetPosition;   // The camera target position for Comms UI
    public Transform seatTargetPosition;
    public float cameraMoveSpeed = 5f;      // Speed of camera movement
    public float sitSpeed = 5f;
    private bool isCameraMoving = false;    // Flag to indicate if the camera is moving
    private bool isPlayerMoving = false;
    private Transform targetPosition;       // The target position for the camera to move to
    private Transform targetPosSeat;
    private Vector3 currentPos;
    private Quaternion currentRot;

    void Start()
    {
        LockCursor(); // Initially lock the cursor for camera movement
        HideUI(oxygenInteractionUI); // Hide UI initially
        HideUI(pwrFailUI);
        // HideUI(commsUI);
        // HideUI(commsTargetWave);
        // HideUI(commsPlayerWave);
        isSeated = true;
    }

    void Update()
    {
        if (!isAnyUIOpen() && !isInteractingWithUI && !IsPointerOverGameObject()) // Only allow camera movement if not interacting with UI or over UI
        {
            HandleMouseMovement();
            DetectSelectableObjects();

        }

        // Handle unlocking cursor if a UI interaction is detected
        if (Input.GetKeyDown(KeyCode.Escape)) // Optionally, allow pressing Escape to unlock cursor
        {
            HideUI(oxygenInteractionUI);
            HideUI(pwrFailUI); // Hide the UI and re-enable player controls
            // HideUI(commsUI);
            // HideUI(commsTargetWave);
            // HideUI(commsPlayerWave);
            isCameraMoving = false; // Stop moving the camera
            transform.position = camHolder.position;
            
        }

        if(Input.GetKeyUp(KeyCode.N)) {
            isInspecting = false;
            //call to objects function to return to original position

        }

        // if(Input.GetKeyDown(KeyCode.E)) {
        //     Debug.Log("Sit");
        //     if(!isSeated) {
        //         sit();
        //     } else {
        //         stand();
        //     }
        // }

        // Forcefully unlock the cursor if UI is open
        if (isAnyUIOpen() && isInspecting)
        {
            UnlockCursor();
            player.GetComponent<Renderer>().enabled = false;
        } else {
            LockCursor();
            player.GetComponent<Renderer>().enabled = true;
        }

        // Move the camera if the flag is set
        if (isCameraMoving)
        {
            MoveCameraToTarget();
        }

    }
    // Mouse movement logic
    void HandleMouseMovement()
    {
        if(!isAnyUIOpen() && !isInspecting) {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

            yRot += mouseX;
            xRot -= mouseY;
            xRot = Mathf.Clamp(xRot, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRot, yRot, 0);
            orientation.rotation = Quaternion.Euler(0, yRot, 0);
        }
    }

    // Detect selectable objects
    void DetectSelectableObjects()
    {
        if (Physics.Raycast(transform.position, transform.forward, out raycastHit, SelectableDistance, selectableLayer))
        {
            highlight = raycastHit.transform;
            canSelect = true;

            if (highlight != null && IsSelectableTag(highlight.tag))
            {
                // If the left mouse button is clicked, start interaction
                if (Input.GetMouseButtonDown(0) && !IsPointerOverGameObject())
                {
                    originalPos = highlight.position;

                    if (highlight.CompareTag("OxygenPuzzleButton"))
                    {
                        ShowUI(oxygenInteractionUI);
                        SetCameraTarget(oxygenTargetPosition); // Set camera target for Oxygen UI
                        Debug.Log("Oxygen Open");
                    }
                    else if (highlight.CompareTag("PowerFailureButton"))
                    {
                        ShowUI(pwrFailUI);
                        SetCameraTarget(powerTargetPosition); // Set camera target for Power UI
                    }
                    // else if (highlight.CompareTag("CommsButton"))
                    // {
                    //     ShowUI(commsUI);
                    //     ShowUI(commsTargetWave);
                    //     ShowUI(commsPlayerWave);
                    //     SetCameraTarget(commsTargetPosition); // Set camera target for Comms UI
                    // }
                    else if (highlight.CompareTag("Seat")) {
                        sit();
                    }
                }
            }
        }
        if(Physics.Raycast(transform.position, transform.forward, out raycastHit, SelectableDistance, inspectableLayer)) {
            inspectionObj = raycastHit.transform;
            inspectionOriginalPos = raycastHit.transform.position;
            raycastHit.transform.position = transform.position + inspectionOffset;
            isInspecting = true;
            //replace with call to object's own function
        }
    }
    public void sit() {
        //code to sit down
        isSeated = true;
        currentPos = transform.position;
        currentRot = transform.rotation;
        transform.position = seat.position;
        transform.rotation = seat.rotation;
        player.GetComponent<Renderer>().enabled = false;
    }
    public void stand() {
        //code to get up
        isSeated = false;
        transform.position = camHolder.position;
        transform.rotation = camHolder.rotation; 
        player.GetComponent<Renderer>().enabled = true;
        
    }

    // Set the camera target and enable movement
    void SetCameraTarget(Transform target)
    {
        targetPosition = target;
        isCameraMoving = true;
    }

    // Move the camera smoothly to the target position
    void MoveCameraToTarget()
    {
        if (targetPosition != null)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition.position, cameraMoveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetPosition.rotation, cameraMoveSpeed * Time.deltaTime);


            // Check if the camera has reached the target position
            if (Vector3.Distance(transform.position, targetPosition.position) < 0.01f && Quaternion.Angle(transform.rotation, targetPosition.rotation) < 0.1f)
            {
                isCameraMoving = false; // Stop moving the camera when target is reached
                transform.position = targetPosition.position;
                transform.rotation = targetPosition.rotation;
                if (targetPosition == seatTargetPosition){
                    LockCursor(); // Lock the cursor again for free movement
                    Debug.Log("Seated, free movement enabled");
                }
            }
        }
    }


    // Check if the object's tag is one of the selectable tags
    bool IsSelectableTag(string objectTag)
    {
        foreach (string tag in selectableTags)
        {
            if (objectTag.Equals(tag))
            {
                return true;
            }
        }
        return false;
    }

    // Show and hide UI functions
    void ShowUI(GameObject ui)
    {
        if (ui != null)
        {
            ui.SetActive(true);
            if(ui == oxygenInteractionUI) {
                isOxyUIOpen = true; // Oxygen UI open
            }
            if(ui == pwrFailUI) {
                isPwrUIOpen = true; // Power Failure UI open
            }
            // if(ui == commsUI || ui == commsPlayerWave || ui==commsTargetWave) {
            //     isCommsUIOpen = true;
            // }
        }
        Debug.Log("ShowUI Has been run");
    }

    void HideUI(GameObject ui)
    {
        if (ui != null)
        {
            ui.SetActive(false);
            LockCursor(); // Lock the cursor again for camera movement
            if(ui == oxygenInteractionUI) {
                isOxyUIOpen = false; // Close Oxygen UI
            }
            if(ui == pwrFailUI) {
                isPwrUIOpen = false; // Close Power Failure UI
            }
            // if(ui == commsUI || ui == commsPlayerWave || ui==commsTargetWave) {
            //     isCommsUIOpen = false;
            // }
        }
    }

    bool isAnyUIOpen() 
    {
        return isOxyUIOpen || isPwrUIOpen; //|| isCommsUIOpen;
    }

    // Lock and unlock cursor functions
    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isInteractingWithUI = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isInteractingWithUI = true;
    }

    // Check if the mouse is over a UI element
    bool IsPointerOverGameObject()
    {
        // if(oxygenInteractionUI == null || pwrFailUI == null || commsUI == null || commsPlayerWave == null || commsTargetWave == null) {
        //     return false;
        // } 
        return EventSystem.current.IsPointerOverGameObject(); // This is the correct method to detect if the pointer is over a UI element
    }
}
