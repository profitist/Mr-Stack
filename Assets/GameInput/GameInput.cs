using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    
    public  PlayerInputActions playerInputActions {get; private set;}
    
    public static GameInput Instance { get; private set; }

    public bool GrabbingBox { get; private set; }
    public bool PuttingBox { get; private set; }
    public bool Jumping { get; private set; }
    
    public bool Tab {get; private set;}

    public static bool IsDead = false;

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
        GrabbingBox = playerInputActions.Player.GrabBox.IsPressed();
        PuttingBox = playerInputActions.Player.PutBox.IsPressed();
        Tab = playerInputActions.Player.Tab.IsPressed();
    }
}
   