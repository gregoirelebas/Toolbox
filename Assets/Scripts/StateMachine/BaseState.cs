//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using UnityEngine;

public abstract class BaseState<StateKey> where StateKey : System.Enum
{
    public BaseState(StateKey key)
    {
        Key = key;
    }

    public StateKey Key { get; private set; }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();

    public abstract StateKey GetNextState();

    public abstract void OnTriggerEnter(Collider other);
    public abstract void OnTriggerStay(Collider other);
    public abstract void OnTriggerExit(Collider other);
}