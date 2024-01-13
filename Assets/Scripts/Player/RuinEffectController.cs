using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class RuinEffectController : MonoBehaviour
    {
        private void Awake()
        {
            EventManager.RuinEffectTaken += OnRuinEffectTaken;
        }

        private void OnDestroy()
        {
            EventManager.RuinEffectTaken -= OnRuinEffectTaken;
        }

        private void OnRuinEffectTaken(RuinTypes obj)
        {   
            Debug.Log("ruin type "+obj +" buraya ulasabılıyoz mu");
            switch (obj)
            {
                case RuinTypes.GiveMeTrio:
                    EventManager.OnRuinGiveMeTrioTaken();
                    break;
                case RuinTypes.HighRiskHighReward:
                    EventManager.OnRuinHighRiskHighRewardTaken();
                    break;
            }
        }
    }
}