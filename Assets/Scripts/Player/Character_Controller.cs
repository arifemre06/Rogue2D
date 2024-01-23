using System;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;

    public class Character_Controller : MonoBehaviour
    {
        [SerializeField] private Character_Movement_Controller characterMovementController;
        [SerializeField] private CharacterColliderController characterColliderController;
        [SerializeField] private CharacterAnimationController characterAnimationController;
        private List<GameObject> _heroes;
        private bool _animatorSetted = false;

        private void Awake()
        {
            EventManager.GameStarted += OnGameStarted;
            _heroes = new List<GameObject>();
        }

        private void OnDestroy()
        {
            EventManager.GameStarted -= OnGameStarted;
        }

        private void Start()
        {
            
        }
        
        private void OnGameStarted()
        {
            SetAnimators();
        }

        private void SetAnimators()
        {
            
            _heroes = characterMovementController.GetHeroes();
            List<Animator> animators = new List<Animator>();
            foreach (GameObject baseHero in _heroes)
            {
                animators.Add(baseHero.GetComponent<Animator>());
            }
            characterAnimationController.SetAnimators(animators);
            _animatorSetted = true;
        }

        private void Update()
        {
            if (_animatorSetted)
            {
                if (characterMovementController.IsMoving())
                {
                    characterAnimationController.PlayWalkAnimation();
                }
                else
                {
                    characterAnimationController.PlayIdleAnimation();
                }
            }


        }
    }
