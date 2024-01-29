using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tileset : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private void Awake()
    {
        EventManager.MainMenuButtonClicked += OnMainMenu;
        EventManager.PreGameStarted += OnPreGameStarted;
    }

    private void OnDestroy()
    {
        EventManager.MainMenuButtonClicked -= OnMainMenu;
        EventManager.PreGameStarted -= OnPreGameStarted;
    }

    private void OnPreGameStarted()
    {
        animator.SetBool("IsInMenu",false);
        transform.position = Vector3.zero;
    }

    private void OnMainMenu()
    {
        animator.SetBool("IsInMenu",true);
    }
}
