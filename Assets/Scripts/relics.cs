using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using ScriptableObjectsScripts;
using UnityEngine;

namespace DefaultNamespace
{
    public class relics : MonoBehaviour
    {
        private RelicTypes _relicType;

        [SerializeField] private RelicScriptableObject relicScriptableObject;

        private SpriteRenderer _sprite;
        

        public RelicTypes GetRelicString()
        {
            return _relicType;
        }

        public void SetRelicType(RelicTypes type)
        {
            Debug.Log("biz buraya ne gonderiyoz "+type);
            _relicType = type;
        }

    }
}
