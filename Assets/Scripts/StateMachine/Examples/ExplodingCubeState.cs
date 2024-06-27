//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

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
    public class ExplodingCubeState : BaseState<ExplosionState>
    {
        private ExplodingCubeStateData m_data = null;
        private MaterialPropertyBlock m_propertyBlock = new MaterialPropertyBlock();

        private float m_startTime = 0.0f;

        public ExplodingCubeState(ExplosionState key, ExplodingCubeStateData data) : base(key)
        {
            m_data = data;
        }

        public override void EnterState()
        {
            Assert.IsNotNull(m_data);

            m_data.Renderer.GetPropertyBlock(m_propertyBlock);
            m_propertyBlock.SetColor("_Color", m_data.Color);
            m_data.Renderer.SetPropertyBlock(m_propertyBlock);

            m_startTime = Time.timeSinceLevelLoad;
        }

        public override void ExitState()
        {
        }

        public override ExplosionState GetNextState()
        {
            if (Time.timeSinceLevelLoad - m_startTime < m_data.Time)
                return Key;

            switch (Key)
            {
                case ExplosionState.Idle:
                    return ExplosionState.Loading;

                case ExplosionState.Loading:
                    return ExplosionState.Explosion;

                case ExplosionState.Explosion:
                    return ExplosionState.Cooldown;

                case ExplosionState.Cooldown:
                    return ExplosionState.Idle;

                default:
                    throw new System.NotImplementedException();
            }
        }

        public override void OnTriggerEnter(Collider other)
        {
        }

        public override void OnTriggerExit(Collider other)
        {
        }

        public override void OnTriggerStay(Collider other)
        {
        }

        public override void UpdateState()
        {
        }
    }
}