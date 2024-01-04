using System;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class RelicMeta
    {
        [SerializeField] private Mesh mesh;

        [SerializeField] private MeshRenderer renderer;


        public Mesh Mesh => mesh;

        public MeshRenderer Renderer => renderer;

    }
}