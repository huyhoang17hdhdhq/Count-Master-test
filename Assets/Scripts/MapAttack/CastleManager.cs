using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CastleManager : MonoBehaviour
{
    public static CastleManager instance;
    [Header("Máu lâu đài")]
    public int FillAmount = 15;
    public int maxHP = 15;

    [Header("UI Thanh máu")]
    public Image healthBar;

    [Header("Âm thanh khi bị tấn công")]
    public AudioSource audioSource;
    public AudioClip hitClip;

    public UIDropMover UIDropMover;
    public GameObject WinCastle;
   
    private int Diamond = 50;
    public bool rewardGiven = false;

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        UpdateHealthBar();
    }
    private void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("blue"))
        {
            Destroy(other.gameObject);

            FillAmount = Mathf.Max(FillAmount - 1, 0);
            UpdateHealthBar();

            if (audioSource != null && hitClip != null)
            {
                audioSource.PlayOneShot(hitClip);
            }

            Debug.Log("Castle bị tấn công! Máu còn lại: " + FillAmount);

            if (FillAmount == 0 && !rewardGiven)
            {
                GetComponent<Collider>().isTrigger = false;
                rewardGiven = true;
                CoinManager.Instance.AddDiamond(Diamond);
                if (UIDropMover != null)
                {

                    StartCoroutine(DelayMoveUICoroutine());
                    StartCoroutine(DelayUICoroutine());
                }





            }
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)FillAmount / maxHP;
        }
    }

    private IEnumerator DelayMoveUICoroutine()
    {
        yield return new WaitForSeconds(3f);
        if (UIDropMover != null)
        {
            UIDropMover.MoveDown();
        }
    }

    private IEnumerator DelayUICoroutine()
    {
        yield return new WaitForSeconds(2f);
        if (WinCastle != null)
        {
            WinCastle.SetActive(true);
           

        }
    }

    
}
