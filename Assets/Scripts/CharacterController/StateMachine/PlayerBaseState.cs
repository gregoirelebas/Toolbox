//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using UnityEngine;

public abstract class PlayerBaseState : BaseState<PlayerState>
{
    protected PlayerStateData m_data = null;

    public PlayerBaseState(PlayerState key, PlayerStateData data) : base(key)
    {
        m_data = data;
    }

    public abstract override void EnterState();

    public abstract override void ExitState();

    public abstract override PlayerState GetNextState();

    public abstract override void OnTriggerEnter(Collider other);

    public abstract override void OnTriggerExit(Collider other);

    public abstract override void OnTriggerStay(Collider other);

    public abstract override void UpdateState();

    public abstract void InitializeSubState();

    public abstract void SetSuperState();

    public abstract void SetSubState();
}