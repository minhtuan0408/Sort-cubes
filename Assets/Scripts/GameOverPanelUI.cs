using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanelUI : MonoBehaviour
{
    public Animator animator;

    private void OnEnable()
    {
        animator.SetTrigger("Show");
    }
}
