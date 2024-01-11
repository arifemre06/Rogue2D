using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;




namespace DefaultNamespace
{
    public class InfoPanel : UIPanel
    {

        [SerializeField] private Transform relicInfoUITemplate;
        private List<Sprite> _relicImage;
        private bool _panelOpen;
        private List<Transform> _UITemplates;



        private void Awake()
        {
            _relicImage = new List<Sprite>();
            _UITemplates = new List<Transform>();

            EventManager.RelicCollected += OnRelicCollected;
        }

        private void Start()
        {
            relicInfoUITemplate.gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.K) && !_panelOpen)
            {
                Debug.Log("simdi acıyoz");
                _panelOpen = true;
                EventManager.OnInfoPanelOpened(true);
            }
            else if (!Input.GetKeyDown(KeyCode.K) && _panelOpen)
            {   
                Debug.Log("simdi kacıyoz");
                _panelOpen = false;
                EventManager.OnInfoPanelOpened(false);
            }
            
        }

        private void OnRelicCollected(Sprite arg1, string arg2)
        {
            _relicImage.Add(arg1);
            UpdateInfoPanel();
        }

        private void UpdateInfoPanel()
        {
            for (var i = 0; i < _UITemplates.Count; i++)
            {
                Destroy(_UITemplates[i].gameObject);
            }
            if (_UITemplates != null)
            {
                _UITemplates.Clear();
            }
            
            int _index = 0;
            int xindex = 0;
            foreach (Sprite relicSprite in _relicImage)
            {   
                Transform collectableUITransform =Instantiate(relicInfoUITemplate, transform);
                _UITemplates.Add(collectableUITransform);
                collectableUITransform.gameObject.SetActive(true);
                float offset = -160f;
                float xoffset = 120f;
                
                if (xindex % 5 == 0 && xindex != 0)
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
}
