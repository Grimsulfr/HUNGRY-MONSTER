using UnityEngine;

public class JumpingState : State
{
    public override void EnterState()
    {
        base.EnterState();
        player.rb.AddForce(Vector2.up * player.jump, ForceMode2D.Impulse);
        player.anim.SetTrigger("Jump");
    }
    public void OnGrounded()
    {
        player.ChangeState(player.runningState);    
    }
}
