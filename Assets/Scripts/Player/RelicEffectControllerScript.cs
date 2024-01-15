using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectsScripts;
using UnityEngine;

namespace DefaultNamespace
{
    public class RelicEffectControllerScript : MonoBehaviour
    {
        [SerializeField] private RelicScriptableObject relicScriptableObject;
        private float _mainAttackSpeedModifier = 1;
        private float _mainAttackDamageModifier = 1;
        private float _mainLifeStealModifier = 0;
        private float _mainHealtRegenModifier = 0;
        private float _mainMovementSpeedModifier = 1;
        private float _mainShootingDistanceModifier = 1;
        private float _mainDodgeChanceModifier = 0;
        private int _mainDamageProtectionAmount = 0;

        private bool _collidedWithRelic;

        private void Awake()
        {
            EventManager.RelicTaken += OnRelicCollected;
        }

        private void OnDestroy()
        {
            EventManager.RelicTaken -= OnRelicCollected;
        }
        
        private void OnRelicCollected(RelicTypes relicTypes)
        {
            switch (relicTypes)
            {
                case RelicTypes.AttackSpeed:
                    AttackSpeedRelicCollected();
                    break;

                case RelicTypes.AttackDamage:
                    AttackDamageRelicCollected();
                    break;
                case RelicTypes.LifeSteal:
                    LifeStealRelicCollected();
                    break;
                case RelicTypes.LifeRegen:
                    LifeRegenRelicCollected();
                    break;
                case RelicTypes.MovementSpeed:
                    MovementSpeedRelicCollected();
                    break;
                case RelicTypes.MoreShootingDistance:
                    MoreShootingDistanceRelicCollected();
                    break;
                case RelicTypes.DodgeChance:
                    DodgeChanceRelicCollected();
                    break;
                case RelicTypes.DamageProtection:
                    DamageProtectionRelicCollected();
                    break;
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {   
            
            if (other.CompareTag("Relic") && !_collidedWithRelic)
            {
                _collidedWithRelic = true;
                StartCoroutine(WaitBetweenRelicCollides());
                RelicTypes relicString = other.GetComponent<relics>().GetRelicString();
                OnRelicCollected(relicString);
                Destroy(other.gameObject);
            }
        }

        

        private IEnumerator WaitBetweenRelicCollides()
        {
            yield return new WaitForSeconds(1);
            _collidedWithRelic = false;
        }
        private void AttackSpeedRelicCollected()
        {
            _mainAttackSpeedModifier -= 0.05f;
            EventManager.OnAttackSpeedRelicCollected(_mainAttackSpeedModifier);
            RaiseRelicCollected(RelicTypes.AttackSpeed);
        }

        private void AttackDamageRelicCollected()
        {
            _mainAttackDamageModifier += 0.1f;
            EventManager.OnAttackDamageRelicCollected(_mainAttackDamageModifier);
            RaiseRelicCollected(RelicTypes.AttackDamage);
        }

        private void LifeStealRelicCollected()
        {
            _mainLifeStealModifier += 0.05f;
            EventManager.OnLifeStealRelicCollected(_mainLifeStealModifier);
            RaiseRelicCollected(RelicTypes.LifeSteal);
            
        }
        
        private void MovementSpeedRelicCollected()
        {
            _mainMovementSpeedModifier += 0.05f;
            EventManager.OnMovementSpeedRelicCollected(_mainMovementSpeedModifier);
            RaiseRelicCollected(RelicTypes.MovementSpeed);
        }

        private void LifeRegenRelicCollected()
        {

            _mainHealtRegenModifier += 0.02f;
            EventManager.OnLifeRegenRelicCollected(_mainHealtRegenModifier);
            RaiseRelicCollected(RelicTypes.LifeRegen);
        }
        
        private void DamageProtectionRelicCollected()
        {
            _mainDamageProtectionAmount += 1;
            EventManager.OnDamageProtectionRelicCollected(_mainDamageProtectionAmount);
            RaiseRelicCollected(RelicTypes.DamageProtection);
        }

        private void DodgeChanceRelicCollected()
        {
            _mainDodgeChanceModifier += 0.05f;
            EventManager.OnDodgeChanceRelicCollected(_mainDodgeChanceModifier);
            RaiseRelicCollected(RelicTypes.DodgeChance);
        }

        private void MoreShootingDistanceRelicCollected()
        {
            _mainShootingDistanceModifier += 0.05f;
            EventManager.OnMoreShootingDistanceRelicCollected(_mainShootingDistanceModifier);
            RaiseRelicCollected(RelicTypes.MoreShootingDistance);
        }


        private void RaiseRelicCollected(RelicTypes type)
        {
            Debug.Log("type "+type);
            Sprite sprite = relicScriptableObject.GetPrefab(type);
            string text = relicScriptableObject.GetRelicText(type);
            EventManager.OnRelicCollected(sprite,text);
        }
    }
}