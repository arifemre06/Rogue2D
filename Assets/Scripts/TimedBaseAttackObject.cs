using UnityEngine;

namespace DefaultNamespace
{
    public class TimedBaseAttackObject : BaseAttackObject
    {
        [SerializeField] private float disappearCooldown;
        
        protected override void Start()
        {
            base.Start();
            Destroy(gameObject,disappearCooldown);
        }
    }
}