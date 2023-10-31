using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{   
    /// <summary>
    /// The InputManager script attached to the player
    /// </summary>
    private InputManager inputManager;
    /// <summary>
    /// The CharacterController component attached to the player
    /// </summary>
    private CharacterController characterController;
    /// <summary>
    /// The camera object child of the player
    /// </summary>
    private GameObject cameraObject;
    /// <summary>
    /// The rotation of the camera object in the x-axis
    /// </summary>
    private float xRot = 0f;
    /// <summary>
    /// The rotation of the player object in the y-axis
    /// </summary>
    float yRot = 0f;
    /// <summary>
    /// The direction the player is moving
    /// </summary>
    private Vector2 moveDir;
    /// <summary>
    /// The speed the player moves at
    /// </summary>
    [SerializeField] private float moveSpeed = 5f;
    /// <summary>
    /// The sensitivity of the mouse
    /// </summary>
    [SerializeField] private float mouseSensitivity = 20f;
    /// <summary>
    /// The speed of the camera animation when entering and exiting a computer
    /// </summary>
    [SerializeField] private float animSpeed = 1.5f;
    /// <summary>
    /// The UI cursor object
    /// </summary>
    [SerializeField] private GameObject cursor;
    /// <summary>
    /// The canvas object
    /// </summary>
    [SerializeField] private Canvas myCanvas;
    /// <summary>
    /// The starting position of the camera when entering a computer
    /// </summary>
    private Vector3 cameraStartPos;

    // Start is called before the first frame update
    void Start()
    {
        // Hide the cursor and lock it to the center of the screen
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Get the InputManager, CharacterController, and camera object
        inputManager = GetComponent<InputManager>();
        characterController = GetComponent<CharacterController>();
        cameraObject = Camera.main.gameObject;
    }
    /// <summary>
    /// Animates the camera when entering and exiting a computer
    /// </summary>
    /// <param name="camStartPos"></param>
    /// <param name="camEndPos"></param>
    /// <param name="camStartRot"></param>
    /// <param name="playerStartRos"></param>
    /// <returns></returns>
    private IEnumerator CameraAnim(Vector3 camStartPos, Vector3 camEndPos, Quaternion camStartRot, Quaternion playerStartRos)
    { 
        float normalizedTime = 0;
        Vector3 target = new Vector3(0, 0, 0);
        // If the player is entering the computer then set the target rotation to be 0, 0, 0
        if(!inputManager.GetIsPlayerMapEnabled() && !inputManager.GetIsUIShortcutMapEnabled())
        {
            target = new Vector3(0, 0, 0);
        }
        // If the player is exiting the computer then set the target rotation to be the camera rotation
        else if(!inputManager.GetIsPlayerMapEnabled())
        {
            target = new Vector3(xRot, yRot, 0);
        }

        // Lerp the camera and player rotation and position while the normalized time is less than 1
        while (normalizedTime <= 1)
        {
            // Increment the normalized time based on the animation speed
            normalizedTime += Time.deltaTime/animSpeed;
            cameraObject.transform.position = Vector3.Lerp(camStartPos, camEndPos, normalizedTime);
            cameraObject.transform.rotation = Quaternion.Lerp(camStartRot, Quaternion.Euler(target.x, 0, 0), normalizedTime);
            transform.rotation = Quaternion.Lerp(playerStartRos, Quaternion.Euler(0, target.y, 0), normalizedTime);
            yield return null;
        }
        // If the player is entering the computer then disable the player map and enable the UI shortcuts
        if(!inputManager.GetIsPlayerMapEnabled() && !inputManager.GetIsUIShortcutMapEnabled())
        {
            inputManager.EnableUIShortcuts();
        }
        // If the player is exiting the computer then disable the UI shortcuts and enable the player map
        else if(!inputManager.GetIsPlayerMapEnabled())
        {
            inputManager.DisableUIShortcuts();
            inputManager.EnablePlayerMap();
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraObject.transform.position, cameraObject.transform.forward, out hit, 2f))
        {
            // If the player is pressing the interact key and the object has an Monitor tag then enter the computer
            if (inputManager.GetIsInteractPressed() && hit.collider.gameObject.tag == "Monitor")
            {
                // Get the position of the camera Position inside the monitor
                Vector3 monitorCamPos = hit.collider.gameObject.transform.GetChild(0).position;
                cameraStartPos = cameraObject.transform.position;
                inputManager.DisablePlayerMap();
                // Unlock the cursor and enable the UI shortcuts
                Cursor.lockState = CursorLockMode.None;
                StartCoroutine(CameraAnim(cameraStartPos, monitorCamPos, cameraObject.transform.rotation, transform.rotation));
            }
        }

        // Get the mouse movement
        yRot = inputManager.GetLook().x * mouseSensitivity * Time.fixedDeltaTime;
        xRot -= inputManager.GetLook().y * mouseSensitivity * Time.fixedDeltaTime;
        // Clamp the camera rotation to prevent the player from looking too far up or down
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        // Rotate the player and camera objects based on the mouse movement
        if(inputManager.GetIsPlayerMapEnabled()){
            transform.Rotate(Vector3.up * yRot);
            cameraObject.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        }
        else{
            Vector3 mousePos = Input.mousePosition;
            // Convert the mouse position to a position on the canvas
            RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, mousePos, myCanvas.worldCamera, out Vector2 pos);
            // Clamp the cursor position to the canvas
            pos.x = Mathf.Clamp(pos.x + 10f, -myCanvas.GetComponent<RectTransform>().rect.width/2, myCanvas.GetComponent<RectTransform>().rect.width/2);
            pos.y = Mathf.Clamp(pos.y - 10f, -myCanvas.GetComponent<RectTransform>().rect.height/2, myCanvas.GetComponent<RectTransform>().rect.height/2);
            // Set the cursor position to the clamp position
            cursor.transform.position = myCanvas.transform.TransformPoint(pos);
        }

        // Move the player based on the movement input
        moveDir = inputManager.GetMovement();
        Vector3 move = new Vector3(moveDir.x, -9.81f/moveSpeed, moveDir.y);
        characterController.Move(transform.TransformDirection(move) * moveSpeed * Time.fixedDeltaTime);
    }
    /// <summary>
    /// Exits the computer and returns the camera and movement to the player
    /// </summary>
    public void ExitComputer()
    {
        StartCoroutine(CameraAnim(cameraObject.transform.position, cameraStartPos, cameraObject.transform.rotation, transform.rotation));
    }
}
