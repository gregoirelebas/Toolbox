//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

public class PlayerGroundedState : BaseSuperState<PlayerState>
{
    private const float GROUND_GRAVITY = -0.5f;

    private PlayerStateData m_data = null;

    public PlayerGroundedState(PlayerState key, PlayerState entryState, PlayerStateData data) : base(key, entryState)
    {
        m_data = data;
    }

    public override void EnterState()
    {
        base.EnterState();

        m_data.SetGravity(GROUND_GRAVITY);
    }

    public override PlayerState GetNextState()
    {
        if (m_data.IsJumpInput)
            return PlayerState.Jump;

        return PlayerState.Grounded;
    }
}