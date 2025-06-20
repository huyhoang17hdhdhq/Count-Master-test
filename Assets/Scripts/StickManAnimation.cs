using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StickManAnimation : MonoBehaviour
{
    private Animator animator;
    public Button runButton;
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
        if (animator != null)
        {
            animator.SetBool("run", true); // Gọi animation "Run"
        }
    }

}
