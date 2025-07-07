using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickManRun : MonoBehaviour
{
    private Animator StickManAnimator;

    private void Start()
    {
        StickManAnimator = GetComponent<Animator>();
        StickManAnimator.SetBool("run", true);


    }
    private void Update()
    {
       
    }



}
