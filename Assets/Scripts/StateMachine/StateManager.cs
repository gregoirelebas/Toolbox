//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using System.Collections.Generic;

public class StateManager<StateKey> where StateKey : System.Enum
{
    private const string KEY_NOT_FOUND_FORMAT = "Couldn't find a state with key: {0}";

    protected Dictionary<StateKey, BaseState<StateKey>> m_states = new Dictionary<StateKey, BaseState<StateKey>>();

    protected BaseState<StateKey> m_currentState = null;
    protected bool m_isTransitioning = false;

    /// <summary>
    /// Create a new instance.
    /// </summary>
    public StateManager()
    {
    }

    /// <summary>
    /// Set the starting state and call EnterState().
    /// </summary>
    public virtual void SetEntryState(StateKey key)
    {
        if (m_states.TryGetValue(key, out BaseState<StateKey> state))
            m_currentState = state;
        else
            throw new System.NotImplementedException(string.Format(KEY_NOT_FOUND_FORMAT, key));

        m_currentState.EnterState();
    }

    /// <summary>
    /// Check for any state modification. If none, update current state.
    /// </summary>
    public virtual void Update()
    {
        if (!m_isTransitioning)
        {
            StateKey newState = m_currentState.GetNextState();
            if (newState.Equals(m_currentState.Key))
                m_currentState.UpdateState();
            else
                TransitionToState(newState);
        }
    }

    /// <summary>
    /// Call ExitState() current state then EnterState() on the new state.
    /// </summary>
    public virtual void TransitionToState(StateKey key)
    {
        m_isTransitioning = true;

        m_currentState.ExitState();

        if (m_states.TryGetValue(key, out m_currentState))
            m_currentState.EnterState();
        else
            throw new System.NotImplementedException(string.Format(KEY_NOT_FOUND_FORMAT, key));

        m_isTransitioning = false;
    }

    /// <summary>
    /// Try to add a new state to the dictionary.
    /// </summary>
    public virtual void AddState(BaseState<StateKey> newState)
    {
        if (!m_states.ContainsKey(newState.Key))
            m_states.Add(newState.Key, newState);
        else
            throw new System.NotImplementedException(string.Format(KEY_NOT_FOUND_FORMAT, newState.Key));
    }
}