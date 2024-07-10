//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

public abstract class PlayerBaseState : BaseState<PlayerState>
{
    protected PlayerStateData m_data = null;

    public PlayerBaseState(PlayerState key, PlayerStateData data) : base(key)
    {
        m_data = data;
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    public override PlayerState GetNextState()
    {
        return Key;
    }

    public override void UpdateState()
    {
    }
}