using UnityEngine;

public class SlidingState : State
{
    public override void EnterState()
    {
        base.EnterState();
        player.anim.SetBool("Slide", true);
    }

    public override void ExitState()
    {
        base.ExitState();
        player.anim.SetBool("Slide", false);
    }

    public override void HandleCrouchInput(bool isPressed)
    {
        if(!isPressed)
        {
            player.ChangeState(player.runningState);
        }
    }

    public override void HandleJumpInput()
    {
        player.ChangeState(player.jumpingState);
    }
}
