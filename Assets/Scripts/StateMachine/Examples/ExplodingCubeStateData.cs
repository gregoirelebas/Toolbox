//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using UnityEngine;

namespace StateMachineExamples
{
    [System.Serializable]
    public class ExplodingCubeStateData
    {
        [SerializeField] private float m_time = 0.0f;
        [SerializeField] private Color m_color = Color.white;

        public Renderer Renderer { get; set; }

        public float Time => m_time;
        public Color Color => m_color;
    }
}