using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DefaultNamespace;
using ScriptableObjectsScripts;
using UnityEngine;
using UnityEngine.AI;

public class CharacterColliderController : MonoBehaviour
{
    
    [SerializeField] private GameObject relicPrefab;
    [SerializeField] private LayerMask chestLayerMask;
    [SerializeField] private RelicScriptableObject relicScriptableObject;
    [SerializeField] private gameController gameController;

    [SerializeField] private float chestSearchRange;
    private Collider2D[] _chestHits;
    private int _waitSecondsBetweenChestOpen;
    private bool _canOpenChest;
    private bool _canOpenRuin;


    private void Awake()
    {
        _chestHits = new Collider2D[1];
        _canOpenChest = true;
        _canOpenRuin = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            int chestHitCount = Physics2D.OverlapCircleNonAlloc(transform.position, chestSearchRange, _chestHits, chestLayerMask);
            if (chestHitCount > 0)
            {
                if (_chestHits[0].CompareTag("chest"))
                {
                    Chest chest = _chestHits[0].GetComponent<Chest>();
                    int price = chest.GetPrice();
                
                    if (gameController.GetGold() >= price && _canOpenChest)
                    {
                        _canOpenChest = false;
                        StartCoroutine(WaitBetweenChests());
                        OpenChest(chest, price, _chestHits[0]);
                    
                    }
                }
                
                else if (_chestHits[0].CompareTag("Ruin"))
                {
                    RuinStatue ruinStatue = _chestHits[0].GetComponent<RuinStatue>();
                    GameObject ruinGameObject = _chestHits[0].gameObject;
                    if (_canOpenRuin)
                    {
                        _canOpenRuin = false;
                        TakeRuinOffer(ruinStatue);
                        StartCoroutine(WaitBetweenRuins(ruinGameObject));
                    }
                }
            }
        }
    }

    private void TakeRuinOffer(RuinStatue ruinStatue)
    {
        RuinTypes ruinType = ruinStatue.GetRuinEffect();
        EventManager.OnRuinEffectTaken(ruinType);
    }
    private void OpenChest(Chest chest,int price,Collider2D other)
    {
        EventManager.OnGoldAndExpChanged(-price,0);
        RelicTypes relictoinstantiate = chest.GetRandomRelic();
        
        GameObject newRelic = Instantiate(relicPrefab, other.transform.position, Quaternion.identity);
        Destroy(other.gameObject);
        relics relic = newRelic.GetComponent<relics>();
        relic.GetComponent<SpriteRenderer>().sprite = relicScriptableObject.GetPrefab(relictoinstantiate);
        relic.SetRelicType(relictoinstantiate);
    }

   

    private void OnColliderEnter2D(Collider2D other)
    {   
        
    }

    private IEnumerator WaitBetweenChests()
    {
        yield return new WaitForSeconds(1);
        _canOpenChest = true;
    }
    
    private IEnumerator WaitBetweenRuins(GameObject ruinGameObject)
    {
        yield return new WaitForSeconds(1);
        _canOpenRuin = true;
        Destroy(ruinGameObject);
    }
}
