using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "RuinTextData", menuName = "Game/RuinText", order = 0)]
    public class RuinTextData : ScriptableObject
    {
        [SerializeField] private List<string> ruinExplanation;
        [SerializeField] private List<RuinTypes> ruinTypes;

        public string GetRuinExplanation(RuinTypes ruinType)
        {
            for (var i = 0; i < ruinTypes.Count; i++)
            {
                if (ruinTypes[i] == ruinType)
                {
                    return ruinExplanation[i];
                }
            }

            return null;
        }
    }
}