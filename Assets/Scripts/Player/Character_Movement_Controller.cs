using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using ScriptableObjectsScripts;
using Unity.Mathematics;
using UnityEngine;

public class Character_Movement_Controller : MonoBehaviour
{
    private float speed;
    [SerializeField] private float baseSpeed;

    [SerializeField] private List<GameObject> heroes;
    private List<GameObject> heroesInScene;
    private Rigidbody2D _rigidbody2D;

    private float _horizontal;
    private float _vertical;
    private bool _facingRight = true;
    private bool _facingUp = true;
    private bool _gameLostBefore = false;

    private void Awake()
    {
        EventManager.MovementSpeedRelicCollected += OnMovementSpeedRelicTaken;
        EventManager.PreGameStarted += OnPreGameStarted;
        EventManager.GameOver += OnGameOver;
        heroesInScene = new List<GameObject>();
    }

    private void OnDestroy()
    {
        EventManager.MovementSpeedRelicCollected -= OnMovementSpeedRelicTaken;
        EventManager.PreGameStarted -= OnPreGameStarted;
        EventManager.GameOver -= OnGameOver;
    }

    private void OnGameOver()
    {
        _gameLostBefore = true;
        transform.position = Vector3.zero;
        heroesInScene.Clear();
    }

    private void OnPreGameStarted()
    {
        int xIndex = 0;
        int yIndex = 0;
        float xOffset = 0.5f;
        float yOffset = 0.3f;
        
            for (var i = 0; i < heroes.Count; i++)
            {   
                if (xIndex % 2 == 0 && xIndex != 0)
                {
                    yIndex += 1;
                    xIndex = 0;
                }
                GameObject tempHero = Instantiate(heroes[i], new Vector2(xIndex * xOffset, yIndex * yOffset), quaternion.identity,transform);
                heroesInScene.Add(tempHero);
                xIndex++;
            }

        _facingRight = true;

    }
    private void OnMovementSpeedRelicTaken(float increaseModifier,int amount)
    {
        speed = baseSpeed + increaseModifier * amount;
    }
    private void Start()
    {
        speed = baseSpeed;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.gameState != GameState.InGamePanel) return;
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        transform.position += new Vector3(_horizontal*speed*Time.deltaTime, _vertical*speed*Time.deltaTime,0);
        
        if (_horizontal > 0 && !_facingRight)
        {
            // ... flip the player.
            FlipHorizontal();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (_horizontal < 0 && _facingRight)
        {
            // ... flip the player.
            FlipHorizontal();
        }


        /*
        if (_vertical > 0 && _facingUp)
        {
            // ... flip the player.
            FlipVertical();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (_vertical < 0 && !_facingUp)
        {
            // ... flip the player.
            FlipVertical();
        }
        */
    }
    public bool IsMoving()
    {
        if(_horizontal != 0 || _vertical != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    private void FlipHorizontal()
    {
        // Switch the way the player is labelled as facing.
        _facingRight = !_facingRight;
        // Multiply the player's x local scale by -1.
        foreach (GameObject baseHero in heroesInScene)
        {
            if (baseHero != null)
            {
                Vector3 theScale = baseHero.transform.localScale;
                theScale.x *= -1;
                baseHero.transform.localScale = theScale;
            }
        }
        /*
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        */
    }
    
    private void FlipVertical()
    {
        // Switch the way the player is labelled as facing.
        _facingUp = !_facingUp;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.y *= -1;
        transform.localScale = theScale;
    }

    public List<GameObject> GetHeroes()
    {
        return heroesInScene;
    }



}
