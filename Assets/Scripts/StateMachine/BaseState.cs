//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

public abstract class BaseState<StateKey> where StateKey : System.Enum
{
    public StateKey Key { get; private set; }

    /// <summary>
    /// Create a new instance and set `Key`.
    /// </summary>
    public BaseState(StateKey key)
    {
        Key = key;
    }

    /// <summary>
    /// Enter this state. Make your allocations here.
    /// </summary>
    public abstract void EnterState();

    /// <summary>
    /// Exit this state. Don't forget to release any allocations.
    /// </summary>
    public abstract void ExitState();

    /// <summary>
    /// Get the next state. Return `Key` if you don't want to change.
    /// </summary>
    public abstract StateKey GetNextState();

    /// <summary>
    /// Update this state. Called every frame with Unity Update();
    /// </summary>
    public abstract void UpdateState();
}