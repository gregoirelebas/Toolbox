//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

public class PlayerSuperState : BaseSuperState<PlayerState>
{
    protected PlayerStateData m_data = null;

    public PlayerSuperState(PlayerState key, PlayerState entryKey, PlayerStateData data) : base(key, entryKey)
    {
        m_data = data;
    }

    public override void EnterState()
    {
        base.EnterState();
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
        base.UpdateState();
    }
}