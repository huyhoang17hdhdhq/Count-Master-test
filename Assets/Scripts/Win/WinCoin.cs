using UnityEngine;

public class WinCoin : MonoBehaviour
{
    public GameObject fxPrefab;
    private Animator chest;
    private bool hasOpened = false;


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
            }
        }
      
    }
}
