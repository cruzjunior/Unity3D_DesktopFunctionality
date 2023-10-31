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
    /// The InputActionMap for the UI controls shortcuts
    /// </summary>
    private InputActionMap uiShortcut;
    /// <summary>
    /// Whether the interact key is pressed
    /// </summary>
    private bool isInteractPressed = false;
    /// <summary>
    /// Whether the player map is enabled
    /// </summary>
    private bool isPlayerMapEnabled = true;
    /// <summary>
    /// Whether the UI shortcuts are enabled
    /// </summary>
    private bool isUIShortcutMapEnabled = false;
    /// <summary>
    /// Whether the escape key is pressed
    /// </summary>
    private bool isEscPressed = false;
    
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
        uiShortcut = playerControls.MonitorUI;
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
    /// <summary>
    /// Gets whether the interact key is pressed
    /// </summary>
    /// <returns></returns>
    public bool GetIsInteractPressed()
    {
        return isInteractPressed;
    }
    /// <summary>
    /// Disables the player map
    /// </summary>
    public void DisablePlayerMap()
    {
        playerMap.Disable();
        isPlayerMapEnabled = false;
    }
    /// <summary>
    /// Enables the player map
    /// </summary>
    public void EnablePlayerMap()
    {
        playerMap.Enable();
        isPlayerMapEnabled = true;
    }
    /// <summary>
    /// Gets whether the player map is enabled
    /// </summary>
    /// <returns> returns true if the player map is enabled
    /// </returns>
    public bool GetIsPlayerMapEnabled()
    {
        return isPlayerMapEnabled;
    }
    /// <summary>
    /// Gets whether the UI shortcuts are enabled
    /// </summary>
    /// <returns> returns true if the UI shortcuts are enabled
    /// </returns>
    public bool GetIsUIShortcutMapEnabled()
    {
        return isUIShortcutMapEnabled;
    }
    /// <summary>
    /// Enables the UI shortcuts
    /// </summary>
    public void EnableUIShortcuts()
    {
        uiShortcut.Enable();
        isUIShortcutMapEnabled = true;
        // binds the escape InputAction to bool
        playerControls.MonitorUI.Escape.performed += ctx => isEscPressed = true;
        playerControls.MonitorUI.Escape.canceled += ctx => isEscPressed = false;
    }
    /// <summary>
    /// Disables the UI shortcuts
    /// </summary>
    public void DisableUIShortcuts()
    {
        uiShortcut.Disable();
        isUIShortcutMapEnabled = false;
    }
    /// <summary>
    /// Gets whether the escape key is pressed  
    /// </summary>
    /// <returns> returns true if the escape key is pressed
    /// </returns>
    public bool GetIsEscPressed()
    {
        return isEscPressed;
    }
}
