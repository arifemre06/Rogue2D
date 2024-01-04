using System.Collections;
using System.Collections.Generic;
using ScriptableObjectsScripts;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "RelicData", menuName = "Game/RelicData", order = 0)]
    public class RelicScriptableObject : ScriptableObject
    {
        [SerializeField] private List<RelicTypes> relicTypes;
        [SerializeField] private List<Sprite> sprites;
        [SerializeField] private List<string> itemDetailText;
        

        public Sprite GetPrefab(RelicTypes relicType)
        {
            for (int i = 0; i < relicTypes.Count; i++)
            {
                if (relicTypes[i].Equals(relicType))
                {
                    return sprites[i];
                }
            }

            return null;
        }

        public List<RelicTypes> GetRelicTypesList()
        {
            return relicTypes;
        }

        public string GetRelicText(RelicTypes relicType)
        {
            for (int i = 0; i < relicTypes.Count; i++)
            {
                if (relicTypes[i].Equals(relicType))
                {
                    return itemDetailText[i];
                }
            }

            return null;
        }


    }
} 
