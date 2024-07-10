//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerState key, PlayerStateData data) : base(key, data)
    {
    }

    public override void EnterState()
    {
        m_data.Animator.SetBool(m_data.IsWalkingHash, true);
        m_data.Animator.SetBool(m_data.IsRunningHash, true);
    }

    public override PlayerState GetNextState()
    {
        if (!m_data.IsMovement)
            return PlayerState.Idle;
        else if (!m_data.IsRunning)
            return PlayerState.Walk;

        return PlayerState.Run;
    }
}