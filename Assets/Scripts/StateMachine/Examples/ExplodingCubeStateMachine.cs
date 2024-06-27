//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace StateMachineExamples
{
    public class ExplodingCubeStateMachine : StateManager<ExplosionState>
    {
        [System.Serializable]
        private struct StateDataHolder
        {
            public ExplosionState State;
            public ExplodingCubeStateData Data;
        }

        [SerializeField] private Renderer m_cubeRenderer = null;
        [SerializeField] private List<StateDataHolder> m_statesData = null;

        private MaterialPropertyBlock m_colorProperty = null;

        private void Awake()
        {
            Assert.IsNotNull(m_cubeRenderer);

            for (int i = 0; i < m_statesData.Count; i++)
            {
                m_statesData[i].Data.Renderer = m_cubeRenderer;
                m_states.Add(m_statesData[i].State, new ExplodingCubeState(m_statesData[i].State, m_statesData[i].Data));
            }

            m_currentState = m_states[0];
        }
    }
}