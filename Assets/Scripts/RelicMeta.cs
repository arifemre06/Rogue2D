using System;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class RelicMeta
    {
        [SerializeField] private Sprite sprite;

        [SerializeField] private string description;


        public Sprite Sprite => sprite;

        public string Description => description;

    }
}