using System;
using UnityEngine;
using DefaultNamespace;

    public class Character_Controller : MonoBehaviour
    {
        [SerializeField] private Character_Movement_Controll characterMovementController;
        [SerializeField] private CharacterColliderController characterColliderController;
        [SerializeField] private CharacterAnimationController characterAnimationController;


        private void Update()
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
