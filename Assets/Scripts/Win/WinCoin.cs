using UnityEngine;

public class WinCoin : MonoBehaviour
{
    public GameObject fxPrefab;  
    private PlayerManager playerManager;
    private Animator chest;
    private bool hasOpened = false;


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player") && !hasOpened)
        {
           
            playerManager = other.GetComponent<PlayerManager>();
            chest = GetComponent<Animator>();

            if (playerManager != null)
            {
                
                playerManager.DisablePlayerManager();

                
                if (fxPrefab != null)
                {
                    
                    fxPrefab.SetActive(true); // Bật hiệu ứng FX
                }
            }
            if (chest != null)
            {
                hasOpened = true;
                chest.SetTrigger("Open");
            }
            if (CoinManager.Instance != null)
            {
                CoinManager.Instance.AddCoin(100);
                Debug.Log("Chạm vào wincoin → +100 coin");
            }
        }
        //if( other.CompareTag("blue") && !hasOpened)
        //{
        //    chest = GetComponent<Animator>();
        //    if (chest != null)
        //    {
        //        hasOpened = true;
        //        chest.SetTrigger("Open");
        //    }
           
        //}
       
    }
}
