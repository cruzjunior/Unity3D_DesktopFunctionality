using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    /// <summary>
    /// The PlayerControls script generated from the InputActionAsset
    /// </summary>
    private PlayerControls playerControls;
    /// <summary>
    /// The InputAction for movement
    /// </summary>
    private InputAction movement;
    /// <summary>
    /// The InputAction for looking
    /// </summary>
    private InputAction look;
    /// <summary>
    /// The InputActionMap for the player controls
    /// </summary>
    private InputActionMap playerMap;
    /// <summary>
    /// The InputAction for the UI controls shortcuts
    /// </summary>
    private InputAction uiShortcut;

    private bool isInteractPressed = false;
    
    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        // Enable the player action map
        playerMap = playerControls.Player;
        playerMap.Enable();
        // assigns the movement and look InputActions
        movement = playerControls.Player.Move;
        look = playerControls.Player.Look;
        // assigns the UI shortcut InputAction
        uiShortcut = playerControls.MonitorUI.Shortcuts;
        // binds the interact InputAction to bool
        playerControls.Player.Interact.performed += ctx => isInteractPressed = true;
        playerControls.Player.Interact.canceled += ctx => isInteractPressed = false;
    }
    /// <summary>
    /// Gets the movement vector from the InputAction
    /// </summary>
    /// <returns> 2D vector from movement key press </returns>
    public Vector2 GetMovement()
    {
        return movement.ReadValue<Vector2>();
    }
    /// <summary>
    /// Gets the look vector from the InputAction
    /// </summary>
    /// <returns> 2D vector of delta mouse location </returns>
    public Vector2 GetLook()
    {
        return look.ReadValue<Vector2>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
