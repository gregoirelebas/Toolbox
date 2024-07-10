//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerState key, PlayerStateData data) : base(key, data)
    {
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