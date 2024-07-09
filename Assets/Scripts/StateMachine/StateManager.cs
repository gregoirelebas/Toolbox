//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<StateKey> : MonoBehaviour where StateKey : System.Enum
{
    protected Dictionary<StateKey, BaseState<StateKey>> m_states = new Dictionary<StateKey, BaseState<StateKey>>();

    protected BaseState<StateKey> m_currentState = null;
    protected bool m_isTransitioning = false;

    protected virtual void Start()
    {
        m_currentState.EnterState();
    }

    protected virtual void Update()
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

    protected virtual void OnTriggerEnter(Collider other)
    {
        m_currentState.OnTriggerEnter(other);
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        m_currentState.OnTriggerStay(other);
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        m_currentState.OnTriggerExit(other);
    }

    public void TransitionToState(StateKey key)
    {
        m_isTransitioning = true;

        m_currentState.ExitState();

        if (m_states.TryGetValue(key, out m_currentState))
            m_currentState.EnterState();
        else
            throw new System.NotImplementedException(string.Format("Couldn't find a state with key: {0}", key));

        m_isTransitioning = false;
    }
}