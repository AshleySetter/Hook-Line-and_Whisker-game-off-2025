using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public InputSystem_Actions playerInputActions;

    private void Awake()
    {
        Instance = this;

        playerInputActions = new InputSystem_Actions();
        playerInputActions.Player.Enable();
    }

    private void OnDestroy()
    {
        playerInputActions.Dispose();
    }

    public Vector2 GetMovementVectorNormalised()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        Debug.Log(inputVector);
        return inputVector.normalized;
    }
}