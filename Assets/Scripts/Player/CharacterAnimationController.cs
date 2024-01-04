using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField] private List<Animator> charactersAnimatorList;

    public void PlayWalkAnimation()
    {
        SetBoolParameter("IsMoving",true);
    }

    public void PlayIdleAnimation()
    {
        SetBoolParameter("IsMoving",false);
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
        for (int i = 0; i < charactersAnimatorList.Count; i++)
        {
            charactersAnimatorList[i].SetBool(parameter,boolValue);
        }
    }
    
    
}
