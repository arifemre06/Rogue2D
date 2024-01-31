using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectsScripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class RelicEffectControllerScript : MonoBehaviour
    {
        [SerializeField] private RelicScriptableObject relicScriptableObject;
        private float _mainAttackSpeedModifier = 0.1f;
        private float _mainAttackDamageModifier = 0.2f;
        private float _mainLifeStealModifier = 0.05f;
        private float _mainHealtRegenModifier = 0.02f;
        private float _mainMovementSpeedModifier = 0.1f;
        private float _mainShootingDistanceModifier = 0.1f;
        private float _mainDodgeChanceModifier = 0.05f;
        private float _mainDefenseModifier = 10;
        private float _mainCritModifier = 5;
        private float _mainAttackSpeedOnCritModifier = 0.3f;
        private float _attackSpeedOnCritDuration = 3;
        private float _moreLifeModifier = 20;
        private int _mainDamageProtectionAmount = 0;

        private Dictionary<RelicTypes, int> _relicAmountDictionary;

        private bool _collidedWithRelic;

        private void Awake()
        {
            _relicAmountDictionary = new Dictionary<RelicTypes, int>();

            List<RelicTypes> relicTypesList = relicScriptableObject.GetRelicTypesList();
            
            foreach (RelicTypes relicTypes in relicTypesList)
            {
                _relicAmountDictionary[relicTypes] = 0;
            }
            EventManager.RelicTaken += OnRelicCollected;
            EventManager.RuinGiveMeTrioTaken += OnGiveMeTrioTaken;
        }

        private void OnDestroy()
        {
            EventManager.RelicTaken -= OnRelicCollected;
            EventManager.RuinGiveMeTrioTaken -= OnGiveMeTrioTaken;
        }
        
        private void OnRelicCollected(RelicTypes relicTypes)
        {
            _relicAmountDictionary[relicTypes] += 1;
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
                case RelicTypes.Defense:
                    DefenseRelicCollected();
                    break;
                case RelicTypes.CritChance:
                    CritChanceRelicCollected();
                    break;
                case RelicTypes.AttackSpeedOnCrit:
                    AttackSpeedOnCritCollected();
                    break;
                case RelicTypes.MoreLife:
                    MoreLifeRelicCollected();
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
                EventManager.OnRelicTaken(relicString);
                OnRelicCollected(relicString);
                Destroy(other.gameObject);
            }
        }
        private void OnGiveMeTrioTaken()
        {
            for (int i = 0; i < 3; i++)
            {
                EventManager.OnRelicTaken(GetRandomRelic());
            }
        }
        
        private IEnumerator WaitBetweenRelicCollides()
        {
            yield return new WaitForSeconds(1);
            _collidedWithRelic = false;
        }
        private void AttackSpeedRelicCollected()
        {
            EventManager.OnAttackSpeedRelicCollected(_mainAttackSpeedModifier,_relicAmountDictionary[RelicTypes.AttackSpeed]);
            RaiseRelicCollected(RelicTypes.AttackSpeed);
        }

        private void AttackDamageRelicCollected()
        {
            EventManager.OnAttackDamageRelicCollected(_mainAttackDamageModifier,_relicAmountDictionary[RelicTypes.AttackDamage]);
            RaiseRelicCollected(RelicTypes.AttackDamage);
        }

        private void LifeStealRelicCollected()
        {
            EventManager.OnLifeStealRelicCollected(_mainLifeStealModifier,_relicAmountDictionary[RelicTypes.LifeSteal]);
            RaiseRelicCollected(RelicTypes.LifeSteal);
            
        }
        
        private void MovementSpeedRelicCollected()
        {
            EventManager.OnMovementSpeedRelicCollected(_mainMovementSpeedModifier,_relicAmountDictionary[RelicTypes.MovementSpeed]);
            RaiseRelicCollected(RelicTypes.MovementSpeed);
        }

        private void LifeRegenRelicCollected()
        {
            
            EventManager.OnLifeRegenRelicCollected(_mainHealtRegenModifier,_relicAmountDictionary[RelicTypes.LifeRegen]);
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
            EventManager.OnDodgeChanceRelicCollected(_mainDodgeChanceModifier,_relicAmountDictionary[RelicTypes.DodgeChance]);
            RaiseRelicCollected(RelicTypes.DodgeChance);
        }

        private void MoreShootingDistanceRelicCollected()
        {
            EventManager.OnMoreShootingDistanceRelicCollected(_mainShootingDistanceModifier,_relicAmountDictionary[RelicTypes.MoreShootingDistance]);
            RaiseRelicCollected(RelicTypes.MoreShootingDistance);
        }

        private void DefenseRelicCollected()
        {
            EventManager.OnDefenseRelicCollected(_mainDefenseModifier,_relicAmountDictionary[RelicTypes.Defense]);
            RaiseRelicCollected(RelicTypes.Defense);
        }
        
        private void CritChanceRelicCollected()
        {
            EventManager.OnCritChanceRelicCollected(_mainCritModifier,_relicAmountDictionary[RelicTypes.CritChance]);
            RaiseRelicCollected(RelicTypes.CritChance);
        }
        
        private void AttackSpeedOnCritCollected()
        {
            EventManager.OnAttackSpeedOnCritCollected(_mainAttackSpeedOnCritModifier,_attackSpeedOnCritDuration,_relicAmountDictionary[RelicTypes.AttackSpeedOnCrit]);
            RaiseRelicCollected(RelicTypes.AttackSpeedOnCrit);
        }
        
        private void MoreLifeRelicCollected()
        {
            EventManager.OnMoreLifeRelicCollected(_moreLifeModifier,_relicAmountDictionary[RelicTypes.MoreLife]);
            RaiseRelicCollected(RelicTypes.MoreLife);
        }
        
        
        private void RaiseRelicCollected(RelicTypes type)
        {
            Debug.Log("type "+type);
            Sprite sprite = relicScriptableObject.GetPrefab(type);
            string text = relicScriptableObject.GetRelicText(type);
            EventManager.OnRelicCollected(sprite,text);
        }
        
        
        
        public RelicTypes GetRandomRelic()
        {
            List<RelicTypes> relicTypesList = relicScriptableObject.GetRelicTypesList();
            int randomNumber = Random.Range(0, relicTypesList.Count);
            return relicTypesList[randomNumber];
        }
    }
}