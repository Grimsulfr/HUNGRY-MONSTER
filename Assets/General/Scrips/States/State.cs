using UnityEngine;

public class State : MonoBehaviour
{
    protected PlayerMovement player;

    //Inicilizacion del estado
    public virtual void Init(PlayerMovement playerRef)
    {
        player = playerRef;
        enabled = false;
    }

    public virtual void EnterState()
    {
        enabled = true;
    }

    public virtual void ExitState()
    {
        enabled = false;
    }

    public virtual void HandleJumpInput() {}
    public virtual void HandleCrouchInput(bool isPressed) {}
}
