using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using ScriptableObjectsScripts;
using UnityEngine;

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


    private void Awake()
    {
        _chestHits = new Collider2D[1];
        _canOpenChest = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, chestSearchRange, _chestHits, chestLayerMask);
            if (hitCount > 0)
            {
                Chest chest = _chestHits[0].GetComponent<Chest>();
                int price = chest.GetPrice();
                
                if (gameController.gold >= price && _canOpenChest)
                {
                    _canOpenChest = false;
                    StartCoroutine(WaitBetweenChests());
                    OpenChest(chest, price, _chestHits[0]);
                    
                }
                
            }
        }
    }

    private void OpenChest(Chest chest,int price,Collider2D other)
    {
        gameController.gold -= price;
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
}
