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
        [SerializeField] private List<relics.RelicRarity> rarity;
        [SerializeField] private List<RelicTypes> commonRelics;
        [SerializeField] private List<RelicTypes> rareRelics;
        [SerializeField] private List<RelicTypes> legendaryRelics;


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

        public relics.RelicRarity GetRarity(RelicTypes relicType)
        {
            for (int i = 0; i < relicTypes.Count; i++)
            {
                if (relicTypes[i].Equals(relicType))
                {
                    return rarity[i];
                }
            }
            return relics.RelicRarity.Common;
        }

        public List<RelicTypes> GetCommonRelics()
        {
            return commonRelics;
        }
        public List<RelicTypes> GetRareRelics()
        {
            return rareRelics;
        }
        public List<RelicTypes> GetLegendaryRelics()
        {
            return legendaryRelics;
        }


    }
} 
