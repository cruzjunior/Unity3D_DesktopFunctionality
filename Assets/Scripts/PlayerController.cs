using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /// The rotation of the camera object
    /// </summary>
    private float xRot = 0f;
    /// <summary>
    /// The direction the player is moving
    /// </summary>
    private Vector2 moveDir;
    /// <summary>
    /// The speed the player moves at
    /// </summary>
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float mouseSensitivity = 20f;

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

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        // Get the mouse movement
        float yRot = inputManager.GetLook().x * mouseSensitivity * Time.fixedDeltaTime;
        xRot -= inputManager.GetLook().y * mouseSensitivity * Time.fixedDeltaTime;
        // Clamp the camera rotation to prevent the player from looking too far up or down
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        // Rotate the player and camera objects based on the mouse movement
        transform.Rotate(Vector3.up * yRot);
        cameraObject.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);

        // Move the player based on the movement input
        moveDir = inputManager.GetMovement();
        Vector3 move = new Vector3(moveDir.x, 0f, moveDir.y);
        characterController.Move(transform.TransformDirection(move) * moveSpeed * Time.fixedDeltaTime);
    }
}
