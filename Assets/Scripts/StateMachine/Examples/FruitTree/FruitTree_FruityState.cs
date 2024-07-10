//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using UnityEngine;

public class FruitTree_FruityState : FruitTree_BaseState
{
    public FruitTree_FruityState(FruitTreeState key) : base(key)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        Debug.Log("Fruity state!");
    }

    public override FruitTreeState GetNextState()
    {
        if (ElapsedTime >= LifeTime)
            return FruitTreeState.Resting;

        return Key;
    }
}