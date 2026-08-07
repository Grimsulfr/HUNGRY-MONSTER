using UnityEngine;

public class DeadState : State
{
    public override void EnterState()
    {
        base.EnterState();
        if(player.anim != null)
        {
            player.anim.SetBool("Sleep", true);
        }

        player.rb.linearVelocity = Vector2.zero;

        if (GameManager.instance != null)
        {
            GameManager.instance.SetGameOver();
        }
    }
}
