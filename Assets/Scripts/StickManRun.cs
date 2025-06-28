using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickManRun : MonoBehaviour
{
    private Animator StickManAnimator;

    private void Start()
    {
        StickManAnimator = GetComponent<Animator>();
      

    }
    private void Update()
    {
        StickManAnimator.SetBool("run", true);
    }



}
