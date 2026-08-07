using UnityEngine;

public class RunningState : State
{
    public override void EnterState()
    {
        base.EnterState();
        player.anim.SetBool("Slide", false);
    }
    public override void HandleJumpInput()
    {
        player.ChangeState(player.jumpingState);
    }
    public override void HandleCrouchInput(bool isPressed)
    {
        if (isPressed)
        {
            player.ChangeState(player.slidingState);
        }
   
    }
}
