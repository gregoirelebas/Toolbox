//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using UnityEngine;

public class FruitTree_RestState : FruitTree_BaseState
{
    public FruitTree_RestState(FruitTreeState key) : base(key)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        Debug.Log("Rest state!");
    }

    public override FruitTreeState GetNextState()
    {
        if (ElapsedTime >= LifeTime)
            return FruitTreeState.Budding;

        return Key;
    }
}