using UnityEngine;
using System.Collections;

public class WinCoin : MonoBehaviour
{
    private menuManager menuManager;
    public GameObject fxPrefab;
    private Animator chest;
    public GameObject WinStair;
    private bool hasOpened = false;
    private bool winTriggered = false;

    public void Win()
    {
        WinStair.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && !hasOpened)
        {

            chest = GetComponent<Animator>();
            PlayerManager.PlayerManagerInstance.gameState = false;



            if (chest != null)
            {
                hasOpened = true;
                chest.SetTrigger("Open");
            }


            if (fxPrefab != null)
            {
                fxPrefab.SetActive(true); // Bật hiệu ứng FX
            }


            if (CoinManager.Instance != null)
            {
                CoinManager.Instance.AddCoin(100);
                Debug.Log("Chạm vào wincoin → +100 coin");
                
                StartCoroutine(CallAfterDelay(3f, Win));


            }
        }

    }
    private IEnumerator CallAfterDelay(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
}
