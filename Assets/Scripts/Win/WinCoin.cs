using UnityEngine;

public class WinCoin : MonoBehaviour
{
    public GameObject fxPrefab;  
    private PlayerManager playerManager;
    private Animator targetAnimator;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
           
            playerManager = other.GetComponent<PlayerManager>();

            
            if (playerManager != null)
            {
                
                playerManager.DisablePlayerManager();

                
                if (fxPrefab != null)
                {
                    
                    fxPrefab.SetActive(true); // Bật hiệu ứng FX
                }
            }
        }
        if (other.CompareTag("blue"))
        {
            targetAnimator = other.GetComponent<Animator>();

            if (targetAnimator != null)
            {

                targetAnimator.SetBool("idle",true); // 
                Debug.Log("Animation changed to Idle");
            }
        }
    }
}
