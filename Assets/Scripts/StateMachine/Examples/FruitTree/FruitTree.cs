//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using System.Collections.Generic;
using UnityEngine;

public enum FruitTreeState
{
    Seed,
    Sapling,
    Adult,
    Budding,
    Blossoming,
    Fruity,
    Resting
}

public class FruitTree : MonoBehaviour
{
    [System.Serializable]
    private struct FruitTreeStateLifeTime
    {
        public FruitTreeState State;
        public float LifeTime;
    }

    [SerializeField] private List<FruitTreeStateLifeTime> m_statesLifeTime = new List<FruitTreeStateLifeTime>();

    private StateManager<FruitTreeState> m_manager = new StateManager<FruitTreeState>();

    private void Awake()
    {
        FruitTree_BaseState state = new FruitTree_SeedState(FruitTreeState.Seed);
        state.LifeTime = m_statesLifeTime.Find(x => x.State == FruitTreeState.Seed).LifeTime;
        m_manager.AddState(state);

        state = new FruitTree_SaplingState(FruitTreeState.Sapling);
        state.LifeTime = m_statesLifeTime.Find(x => x.State == FruitTreeState.Sapling).LifeTime;
        m_manager.AddState(state);

        BaseSuperState<FruitTreeState> adultState = new FruitTree_AdultState(FruitTreeState.Adult, FruitTreeState.Blossoming);
        m_manager.AddState(adultState);

        state = new FruitTree_BuddingState(FruitTreeState.Budding);
        state.LifeTime = m_statesLifeTime.Find(x => x.State == FruitTreeState.Budding).LifeTime;
        adultState.AddSubState(state);

        state = new FruitTree_BlossomingState(FruitTreeState.Blossoming);
        state.LifeTime = m_statesLifeTime.Find(x => x.State == FruitTreeState.Blossoming).LifeTime;
        adultState.AddSubState(state);

        state = new FruitTree_FruityState(FruitTreeState.Fruity);
        state.LifeTime = m_statesLifeTime.Find(x => x.State == FruitTreeState.Fruity).LifeTime;
        adultState.AddSubState(state);

        state = new FruitTree_RestState(FruitTreeState.Resting);
        state.LifeTime = m_statesLifeTime.Find(x => x.State == FruitTreeState.Resting).LifeTime;
        adultState.AddSubState(state);
    }

    private void OnEnable()
    {
        m_manager.SetEntryState(FruitTreeState.Seed);
    }

    private void Update()
    {
        m_manager.Update();
    }
}