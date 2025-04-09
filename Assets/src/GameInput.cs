using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    
    private PlayerInputActions playerInputActions;
    
    public static GameInput Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }
    
    public Vector2 GetMovementVector()
    {
        var inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

    public bool IsJump() => playerInputActions.Player.Jump.IsPressed();
    
    public bool IsGrabbingBox() => playerInputActions.Player.GrabBox.IsPressed();
    
    public bool IsPuttingBox() => playerInputActions.Player.PutBox.IsPressed();
}
   