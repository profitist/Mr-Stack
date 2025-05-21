using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    
    public  PlayerInputActions playerInputActions {get; private set;}
    
    public static GameInput Instance { get; private set; }

    public bool GrabingBox { get; private set; }
    public bool PuttingBox { get; private set; }
    public bool Jumping { get; private set; }
    
    
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
    
    void Update()
    {
        Jumping = playerInputActions.Player.Jump.IsPressed();
        GrabingBox = playerInputActions.Player.GrabBox.IsPressed();
        PuttingBox = playerInputActions.Player.PutBox.IsPressed();
    }
}
   