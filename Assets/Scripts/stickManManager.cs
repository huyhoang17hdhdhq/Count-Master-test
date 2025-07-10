using System;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class stickManManager : MonoBehaviour
{
    //[SerializeField] private ParticleSystem blood;
    private Animator StickManAnimator;
   

    [Header("Nguồn phát âm thanh")]
    public AudioSource audioSource;
    public AudioSource audioAttack;
    [Header("Âm thanh khi va chạm stair")]
    public AudioClip stairClip;
    public AudioClip attack;
    
   
    private Transform moveTarget;
    //public bool attackBoss;



    [Header("Audio All stickman")]
  
    // Lưu các stair đã chạm rồi để không phát lại
    private HashSet<GameObject> triggeredStairs = new HashSet<GameObject>();



    private void Start()
    {
        StickManAnimator = GetComponent<Animator>();

       

       

    }
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("red") && other.transform.parent.childCount > 0)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            
           

        }
        
        

        switch (other.tag)
        {
            case "red":
                if (other.transform.parent.childCount > 0)
                { Destroy(other.gameObject);
                    Destroy(gameObject);

                    if (audioAttack != null && attack != null)
                    {
                        audioAttack.PlayOneShot(attack);
                       
                    }

                }


                break;

            case "jump":

                transform.DOJump(transform.position, 1f, 1, 1f).SetEase(Ease.Flash).OnComplete(PlayerManager.PlayerManagerInstance.FormatStickMan);
               

                break;

            case "kill":

                Destroy(gameObject);
                PlayerManager.PlayerManagerInstance.FormatStickMan();
                if (audioAttack != null && attack != null)
                {
                    audioAttack.PlayOneShot(attack);
                   
                }



                break;
        }

        if (other.CompareTag("stair") && !triggeredStairs.Contains(other.gameObject))
        {
            transform.parent.parent = null; // for instance tower_0
            transform.parent = null; // stickman
            GetComponent<Rigidbody>().isKinematic = GetComponent<Collider>().isTrigger = false;
            StickManAnimator.SetBool("run", false);

            if (!PlayerManager.PlayerManagerInstance.moveTheCamera)
                PlayerManager.PlayerManagerInstance.moveTheCamera = true;

            if (PlayerManager.PlayerManagerInstance.player.transform.childCount == 2)
            {
                other.GetComponent<Renderer>().material.DOColor(new Color(0.4f, 0.98f, 0.65f), 0.5f).SetLoops(1000, LoopType.Yoyo)
                    .SetEase(Ease.Flash);
            }
            triggeredStairs.Add(other.gameObject);

            if (audioSource != null && stairClip != null)
            {
                audioSource.PlayOneShot(stairClip);
                
            }



        }
        if (other.CompareTag("end"))
        {
           
            GetComponent<Rigidbody>().isKinematic = false;
        }
        if (other.CompareTag("runcomplete"))
        {

            GetComponent<Rigidbody>().isKinematic = true;
        }
       
        if (other.CompareTag("Boss"))
        {
            GetComponent<Collider>().isTrigger = false;
            GetComponent<Rigidbody>().isKinematic = true;
            
        }



    }
    

   
}

