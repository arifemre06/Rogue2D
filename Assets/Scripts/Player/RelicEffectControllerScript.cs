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

        private bool _collidedWithRelic;

       
        private void OnTriggerEnter2D(Collider2D other)
        {   
            
            if (other.CompareTag("Relic") && !_collidedWithRelic)
            {
                _collidedWithRelic = true;
                StartCoroutine(WaitBetweenRelicCollides());
                RelicTypes relicString = other.GetComponent<relics>().GetRelicString();
                switch (relicString)
                {
                    case RelicTypes.AttackSpeed:
                        AttackSpeedRelicCollected();
                        break;

                    case RelicTypes.AttackDamage:
                        AttackDamageRelicCollected();
                        break;
                }

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

        private void RaiseRelicCollected(RelicTypes type)
        {
            Debug.Log("type "+type);
            Sprite sprite = relicScriptableObject.GetPrefab(type);
            string text = relicScriptableObject.GetRelicText(type);
            EventManager.OnRelicCollected(sprite,text);
        }
    }
}