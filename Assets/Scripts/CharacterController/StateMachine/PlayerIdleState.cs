//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerState key, PlayerStateData data) : base(key, data)
    {
    }

    public override void EnterState()
    {
        m_data.Animator.SetBool(m_data.IsWalkingHash, false);
        m_data.Animator.SetBool(m_data.IsRunningHash, false);
    }

    public override PlayerState GetNextState()
    {
        if (m_data.IsMovement)
        {
            if (m_data.IsRunning)
                return PlayerState.Run;
            else
                return PlayerState.Walk;
        }

        return PlayerState.Idle;
    }
}