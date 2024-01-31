using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using ScriptableObjectsScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObtainedRelicShopDisplay : MonoBehaviour
{
        [SerializeField] private Transform relicInfoUITemplate;
        [SerializeField] private RelicScriptableObject _relicScriptableObject;
        private Dictionary<RelicTypes,int> _relicCountDictionary;
        private List<Transform> _UITemplates;
    
        
        private void Awake()
        {
            _relicCountDictionary = new Dictionary<RelicTypes, int>();
            _UITemplates = new List<Transform>();
            EventManager.RelicCollected += OnRelicCollected;
            EventManager.UpGradePanelOpened += OnUpgradePanelOpened;
        }

        private void OnDestroy()
        {
            EventManager.RelicCollected -= OnRelicCollected;
            EventManager.UpGradePanelOpened -= OnUpgradePanelOpened;
        }

        private void OnUpgradePanelOpened(int obj)
        {
            UpdateInfoPanel();
        }

        private void OnRelicCollected(Sprite arg1, string arg2)
        {
            StartCoroutine(WaitForTakenRelicUpdate());
            
        }

        private void Start()
        {
            UpdateInfoPanel();
        }

        IEnumerator WaitForTakenRelicUpdate()
        {
            yield return new WaitForSeconds(0.2f);
            UpdateInfoPanel();
        }
        
        public void UpdateInfoPanel()
        {
            for (var i = 0; i < _UITemplates.Count; i++)
            {
                Destroy(_UITemplates[i].gameObject);
            }

            _UITemplates?.Clear();

            _relicCountDictionary?.Clear();

            List<RelicTypes> takenRelics = new List<RelicTypes>();
            takenRelics = TakenRelics.TakenRelicsList;
            if (takenRelics == null)
            {
                Debug.Log("taken relics is null");
                return;
            }
            foreach (RelicTypes takenRelic in takenRelics)
            {
                if (_relicCountDictionary.ContainsKey(takenRelic))
                {
                    _relicCountDictionary[takenRelic] += 1;
                }
                else
                {
                    _relicCountDictionary.Add(takenRelic,1);
                }
                
            }
            
            int _index = 0;
            int xindex = 0;
            foreach (RelicTypes relicSprite in _relicCountDictionary.Keys)
            {   
                Transform collectableUITransform =(Transform)Instantiate(relicInfoUITemplate, transform);
                _UITemplates.Add(collectableUITransform);
                collectableUITransform.gameObject.SetActive(true);
                float offset = -160f;
                float xoffset = 100f;
                
                if (xindex % 10 == 0 && xindex != 0)
                {
                    _index += 1;
                    xindex = 0;
                }
                //baslangıc konumu ayarlamak icin sihirli sayılar kullanıldı.
                relicInfoUITemplate template = collectableUITransform.GetComponent<relicInfoUITemplate>();
                collectableUITransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(xindex * xoffset + 75, _index * offset -75);
                template.SetAmountAndSprite(_relicScriptableObject.GetPrefab(relicSprite),"x"+_relicCountDictionary[relicSprite].ToString());
                xindex++;
                
            }
        }
}
