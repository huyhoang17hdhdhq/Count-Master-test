using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBoss : MonoBehaviour
{
    [Header("Nguồn phát âm thanh")]
    public AudioSource audioSource;
    [Header("Âm thanh khi va chạm stair")]
    public AudioClip hitBoss;

    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();    
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("stop"))
        {
           
            animator.SetTrigger("Fight");
            Debug.Log("even ok");
        }
    }
    public void HitEvent()
    {
       audioSource.PlayOneShot(hitBoss);
        
        Debug.Log("HitEvent được gọi từ Animation.");
        // Gọi logic đánh, sát thương, hiệu ứng... ở đây
    }
}
