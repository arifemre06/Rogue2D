using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    private List<Animator> _charactersAnimatorList;
    private bool _isMoving;

    private void Awake()
    {
        _charactersAnimatorList = new List<Animator>();
    }

    public void SetAnimators(List<Animator> animators )
    {
        if (_charactersAnimatorList != null)
        {
            _charactersAnimatorList.Clear();
        }
        foreach (Animator animator in animators)
        {
            _charactersAnimatorList.Add(animator);
        }
        
    }

    public void PlayWalkAnimation()
    {
        if (!_isMoving)
        {
            SetBoolParameter("IsMoving",true);
            _isMoving = true;
        }
    }

    public void PlayIdleAnimation()
    {
        if (_isMoving)
        {
            SetBoolParameter("IsMoving",false);
            _isMoving = false;
        }
    }

    public void PlayAttackAnimation()
    {
        SetBoolParameter("IsAttacking",true);
    }
    public void PlayHurtAnimation()
    {
        SetBoolParameter("IsHurt",true);
    }

    private void SetBoolParameter(string parameter,bool boolValue)
    {
        for (int i = 0; i < _charactersAnimatorList.Count; i++)
        {
            if (_charactersAnimatorList[i] != null)
            {
                _charactersAnimatorList[i].SetBool(parameter,boolValue);
            }
        }
    }
    
    
}
