using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // để dùng Image

public class EventBoss : MonoBehaviour
{
    [Header("Nguồn phát âm thanh")]
    public AudioSource audioSource;

    [Header("Âm thanh khi danh boss")]
    public AudioClip hitBoss;
    public AudioClip DeadBoss;

    [Header("Thanh máu của Boss")]
    public Image bossHpFill;

    private Animator animator;

    private Transform playerTransform;
    private int currentIndex = 1;
    private const int killPerHit = 5;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            animator.SetTrigger("Fight");
            Debug.Log("even ok");
        }
    }

    public void HitEvent()
    {
        audioSource.PlayOneShot(hitBoss);
        Debug.Log("HitEvent được gọi từ Animation.");

        if (playerTransform == null) return;

        int killed = 0;

        for (int i = currentIndex; i < playerTransform.childCount && killed < killPerHit; i++)
        {
            Transform child = playerTransform.GetChild(i);

            if (child != null && child.gameObject.activeSelf)
            {
                Animator childAnim = child.GetComponent<Animator>();
                if (childAnim != null)
                    childAnim.SetTrigger("die");

                StartCoroutine(DestroyAfterDelay(child.gameObject, 2f));
                killed++;
            }
        }

        currentIndex += killed; // Cập nhật index cho lần tiếp theo

        // Giảm máu boss
        if (bossHpFill != null)
        {
            bossHpFill.fillAmount -= 1f / 3f;
            bossHpFill.fillAmount = Mathf.Clamp01(bossHpFill.fillAmount);

            if (bossHpFill.fillAmount <= 0f)
            {
                animator.SetTrigger("Dead");
                Debug.Log("Boss đã chết");
                PlayerManager.PlayerManagerInstance.WinBoss();
                audioSource.PlayOneShot(DeadBoss);

            }
        }
    }

    private IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj);
    }
}
