using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using ScriptableObjectsScripts;
using UnityEngine;
using UnityEngine.UI;

public class ObtainedRelicShopDisplay : MonoBehaviour
{
        [SerializeField] private Transform relicInfoUITemplate;
        [SerializeField] private RelicScriptableObject _relicScriptableObject;
        private List<Sprite> _relicImage;
        private List<Transform> _UITemplates;
        
        private void Awake()
        {
            _relicImage = new List<Sprite>();
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
            UpdateInfoPanel();
        }

        private void Start()
        {
            relicInfoUITemplate.gameObject.SetActive(false);
            UpdateInfoPanel();
        }
        
        public void UpdateInfoPanel()
        {
            for (var i = 0; i < _UITemplates.Count; i++)
            {
                Destroy(_UITemplates[i].gameObject);
            }
            if (_UITemplates != null)
            {
                _UITemplates.Clear();
            }

            if (_relicImage != null)
            {
                _relicImage.Clear();
            }

            List<RelicTypes> takenRelics = new List<RelicTypes>();
            takenRelics = TakenRelics.TakenRelicsList;
            if (takenRelics == null)
            {
                Debug.Log("tam olarak burdan donmuyoz mu");
                return;
            }
            foreach (RelicTypes takenRelic in takenRelics)
            {
                _relicImage.Add(_relicScriptableObject.GetPrefab(takenRelic));
            }
            
            int _index = 0;
            int xindex = 0;
            foreach (Sprite relicSprite in _relicImage)
            {   
                Transform collectableUITransform =Instantiate(relicInfoUITemplate, transform);
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
                collectableUITransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(xindex * xoffset + 50, _index * offset -50);
                collectableUITransform.Find("Image").GetComponent<Image>().sprite = relicSprite;
                xindex++;
                

            }
        }
}
