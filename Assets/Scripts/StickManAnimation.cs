using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StickManAnimation : MonoBehaviour
{
    private Animator animator;
    public Button runButton;
    public AudioSource runAudio;

    [Header("Nguồn phát âm thanh")]
    public AudioSource audioAll;
    public AudioClip winClip;
    private StickManRun stickManRun;

    private void Start()
    {
       
        animator = GetComponent<Animator>();
        if (runButton != null)
        {
            runButton.onClick.AddListener(OnRunButtonClicked);
        }
    }
    void OnRunButtonClicked()
    {
       animator.SetBool("run",true);
        runAudio.Play();
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WinCoin"))
        {
            animator.SetBool("run", false);
            if (audioAll != null && winClip != null)
            {
                audioAll.PlayOneShot(winClip);
                Debug.Log("Đã phát âm thanh wwin từ: " + other.name);
            }
        }
        if (other.CompareTag("Finish")) 
        {
          runAudio.Pause();
        }


    }
}
