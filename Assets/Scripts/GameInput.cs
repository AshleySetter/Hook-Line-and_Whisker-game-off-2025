using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public InputSystem_Actions playerInputActions;
    public event EventHandler OnInteractAction;
    public event EventHandler OnReelAction;
    public event EventHandler OnEscapeAction;

    private void Awake()
    {
        Instance = this;

        playerInputActions = new InputSystem_Actions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.Reel.performed += Reel_performed;
        playerInputActions.Player.Escape.performed += Escape_performed;
    }

    private void Escape_performed(InputAction.CallbackContext context)
    {
        OnEscapeAction?.Invoke(this, EventArgs.Empty);
    }

    private void Reel_performed(InputAction.CallbackContext context)
    {
        OnReelAction?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        playerInputActions.Dispose();
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.Reel.performed -= Reel_performed;
    }

    public Vector2 GetMovementVectorNormalised()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector.normalized;
    }

    private void Interact_performed(InputAction.CallbackContext context)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }
    
}