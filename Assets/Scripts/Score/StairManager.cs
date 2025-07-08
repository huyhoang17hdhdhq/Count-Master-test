using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Stair
{
    public Transform stairTransform;
    public Collider stairCollider;
}

public class StairManager : MonoBehaviour
{
    public static StairManager Instance;
    [Header("Thiết lập bậc thang từ thấp đến cao")]
    public List<Stair> stairs = new List<Stair>();

    public float baseCoin = 50f;
    public int finalCoin;
   
    

    // Số bậc tổng cộng để chia coin (giả định luôn là 21 bậc)
    private int totalStairs = 21;

    // Đánh dấu những bậc đã được cộng coin
    private HashSet<int> claimedStairs = new HashSet<int>();

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void OnStickmanEnter(GameObject stickman, int stairIndex)
    {
        if (!stickman.CompareTag("blue")) return;

        if (claimedStairs.Contains(stairIndex)) return;

        claimedStairs.Add(stairIndex);

        // Tính coin theo stairIndex
        float rawCoin = baseCoin + baseCoin * 0.1f * stairIndex;
        float coin = rawCoin / totalStairs;
        finalCoin = Mathf.RoundToInt(coin);






        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.AddCoin(finalCoin);

            if (LevelCoin.Instance != null)
            {
                LevelCoin.Instance.CalculateBonus(finalCoin);
            }
            CoinManager.Instance.AddCoin(LevelCoin.Instance.bonusCoinThisLevel);
        }


        Debug.Log($"[StairManager] Bậc {stairIndex + 1} → raw: {rawCoin} → chia {totalStairs} → +{finalCoin} coin");
    }
    
}
