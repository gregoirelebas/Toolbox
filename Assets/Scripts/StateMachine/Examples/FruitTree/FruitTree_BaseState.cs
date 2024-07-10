//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using UnityEngine;

public abstract class FruitTree_BaseState : BaseState<FruitTreeState>
{
    private float m_startTime = 0.0f;
    private float m_elapsedTime = 0.0f;

    protected float ElapsedTime => m_elapsedTime;

    public float LifeTime { get; set; } = 0.0f;

    public FruitTree_BaseState(FruitTreeState key) : base(key)
    {
    }

    /// <summary>
    /// Register the start time of this state.
    /// </summary>
    public override void EnterState()
    {
        m_startTime = Time.timeSinceLevelLoad;
        m_elapsedTime = 0.0f;
    }

    public override void ExitState()
    {
    }

    public override FruitTreeState GetNextState()
    {
        return Key;
    }

    /// <summary>
    /// Compute the elapsed time since the start of this state.
    /// </summary>
    public override void UpdateState()
    {
        m_elapsedTime = Time.timeSinceLevelLoad - m_startTime;
    }
}