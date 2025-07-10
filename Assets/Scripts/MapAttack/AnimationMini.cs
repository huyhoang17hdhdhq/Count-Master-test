using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMini : MonoBehaviour
{
  private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();    
        animator.SetBool("run",true);
    }

   
    void Update()
    {
        
    }
}
