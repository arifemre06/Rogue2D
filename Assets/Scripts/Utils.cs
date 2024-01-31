using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public static class Utils
    {
        public static float DamageReductionFormula(float damage,float Defense)
        {
            damage *= 1f - ((0.052f * Defense) / (20 + 0.048f * Mathf.Abs(Defense)));
            return damage;
        }
    }
}