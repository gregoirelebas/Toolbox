//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerState key, PlayerStateData data) : base(key, data)
    {
    }

    public override void EnterState()
    {
        m_data.Animator.SetBool(m_data.IsWalkingHash, true);
        m_data.Animator.SetBool(m_data.IsRunningHash, false);
    }

    public override PlayerState GetNextState()
    {
        if (!m_data.IsMovement)
            return PlayerState.Idle;
        else if (m_data.IsRunning)
            return PlayerState.Run;

        return PlayerState.Walk;
    }
}