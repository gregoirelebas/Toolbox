//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using UnityEngine;

public abstract class PlayerBaseState : BaseState<PlayerState>
{
    public PlayerBaseState(PlayerState key) : base(key)
    {
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

    public abstract void CheckSwitchState();
}