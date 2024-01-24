using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class TakenRuins : MonoBehaviour
    {
        public static bool HighRiskHighRewardTaken;
        public static bool GiveMeTrioTaken;
        private void Awake()
        {
            EventManager.RuinHighRiskHighRewardTaken += OnHighRiskHighRewardTaken;
            EventManager.RuinGiveMeTrioTaken += OnGiveMeTrioTaken;
            EventManager.NextLevel += OnNextLevel;
        }

        private void OnDestroy()
        {
            EventManager.RuinHighRiskHighRewardTaken -= OnHighRiskHighRewardTaken;
            EventManager.RuinGiveMeTrioTaken -= OnGiveMeTrioTaken;
            EventManager.NextLevel -= OnNextLevel;
        }

        private void OnNextLevel(int obj)
        {
            ClearTakenRuins();
        }

        private void OnGiveMeTrioTaken()
        {
            GiveMeTrioTaken = true;
        }

        private void OnHighRiskHighRewardTaken()
        {
            HighRiskHighRewardTaken = true;
        }

        public static void ClearTakenRuins()
        {
            HighRiskHighRewardTaken = false;
            GiveMeTrioTaken = false;
        }
    }
}