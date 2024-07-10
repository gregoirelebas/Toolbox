//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace StateMachineExamples
{
    public enum ExplosionState
    {
        Idle,
        Loading,
        Explosion,
        Cooldown
    }

    [System.Serializable]
    public class ExplodingCubeStateData
    {
        [SerializeField] private float m_time = 0.0f;
        [SerializeField] private Color m_color = Color.white;

        public Renderer Renderer { get; set; }

        public float Time => m_time;
        public Color Color => m_color;
    }

    public class ExplodingCube : MonoBehaviour
    {
        [System.Serializable]
        private struct StateDataHolder
        {
            public ExplosionState State;
            public ExplodingCubeStateData Data;
        }

        [SerializeField] private Renderer m_cubeRenderer = null;
        [SerializeField] private List<StateDataHolder> m_statesData = null;

        private StateManager<ExplosionState> m_stateManager = new StateManager<ExplosionState>();

        private void Awake()
        {
            Assert.IsNotNull(m_cubeRenderer);

            for (int i = 0; i < m_statesData.Count; i++)
            {
                m_statesData[i].Data.Renderer = m_cubeRenderer;
                m_stateManager.AddState(new ExplodingCubeState(m_statesData[i].State, m_statesData[i].Data));
            }
        }

        private void OnEnable()
        {
            m_stateManager.SetEntryState(ExplosionState.Idle);
        }

        private void Update()
        {
            m_stateManager.Update();
        }
    }
}