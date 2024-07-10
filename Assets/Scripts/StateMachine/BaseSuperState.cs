//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

public abstract class BaseSuperState<StateKey> : BaseState<StateKey> where StateKey : System.Enum
{
    protected StateKey m_entryState;
    protected StateManager<StateKey> m_manager = new StateManager<StateKey>();

    public BaseSuperState(StateKey key, StateKey entryState) : base(key)
    {
        m_entryState = entryState;
    }

    public override void EnterState()
    {
        m_manager.SetEntryState(m_entryState);
    }

    public override void ExitState()
    {
    }

    public override StateKey GetNextState()
    {
        return Key;
    }

    /// <summary>
    /// Check for any subState modification. If none, update the current subState.
    /// </summary>
    public override void UpdateState()
    {
        m_manager.Update();
    }

    public void AddSubState(BaseState<StateKey> subState)
    {
        m_manager.AddState(subState);
    }
}