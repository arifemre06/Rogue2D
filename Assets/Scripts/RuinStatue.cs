using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class RuinStatue : MonoBehaviour
{
    
    [SerializeField] private List<RuinTypes> ruinTypeList;
    [SerializeField] private RuinTextData ruinTextData;
    private RuinTypes _ruinType;

    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI ruinTypeText;
    [SerializeField] private TextMeshProUGUI ruinExplanation;

    [SerializeField] private float canvasVisibleRange;
    [SerializeField] private LayerMask heroLayerMask;
    private Collider2D[] _overlapHits;
    

    private void Start()
    {
        _ruinType = GetRandomEffect();
        ruinExplanation.text = ruinTextData.GetRuinExplanation(_ruinType);
        ruinTypeText.text = _ruinType.ToString();

        _overlapHits = new Collider2D[1];
        canvas.enabled = false;
    }

    private void Update()
    {   
        
        int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, canvasVisibleRange, _overlapHits, heroLayerMask);
        if (hitCount > 0)
        {
            CanvasVisible(true);
            
        }
        else
        {
            CanvasVisible(false);
        }
    }

    private RuinTypes GetRandomEffect()
    {
        int randomNumber = Random.Range(0, ruinTypeList.Count);
        return ruinTypeList[randomNumber];
    }

    public RuinTypes GetRuinEffect()
    {
        return _ruinType;
    }

    public void CanvasVisible(bool canvasEnable)
    {
        canvas.enabled = canvasEnable;
    }
}
