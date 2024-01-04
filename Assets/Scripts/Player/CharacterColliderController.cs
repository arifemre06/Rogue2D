using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using ScriptableObjectsScripts;
using UnityEngine;

public class CharacterColliderController : MonoBehaviour
{
    private bool _canOpenChest = false;
    [SerializeField] private GameObject relicPrefab;
    
    [SerializeField] private RelicScriptableObject relicScriptableObject;
    [SerializeField] private gameController gameController;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("chest"))
        {
            
            Chest chest = other.GetComponent<Chest>();
            int price = chest.GetPrice();
            if (!_canOpenChest)
            {
                if (gameController.gold >= price)
                {
                    _canOpenChest = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.E) && _canOpenChest)
            {
                Debug.Log("peki buraya");
                OpenChest(chest, price, other);
            }
        }
    }


    private void OpenChest(Chest chest,int price,Collider2D other)
    {   
        gameController.gold -= price;
        RelicTypes relictoinstantiate = chest.GetRandomRelic();
        
        GameObject newRelic = Instantiate(relicPrefab, other.transform.position, Quaternion.identity);
        relics relic = newRelic.GetComponent<relics>();
        relic.GetComponent<SpriteRenderer>().sprite = relicScriptableObject.GetPrefab(relictoinstantiate);
        relic.SetRelicType(relictoinstantiate);
        Destroy(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {   
        
        if (other.CompareTag("chest"))
        {
            _canOpenChest = false;
            
        }
    }

    private void OnColliderEnter2D(Collider2D other)
    {   
        
    }
}
