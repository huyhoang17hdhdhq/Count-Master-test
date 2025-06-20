using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stair
{
    public Transform stairTransform;
    public Collider stairCollider;
}

public class StairManager : MonoBehaviour
{
    [Header("Thiết lập bậc thang từ thấp đến cao")]
    public List<Stair> stairs = new List<Stair>();

    public float baseCoin = 50f;

    // Số bậc tổng cộng để chia coin (giả định luôn là 21 bậc)
    private int totalStairs = 21;

    // Đánh dấu những bậc đã được cộng coin
    private HashSet<int> claimedStairs = new HashSet<int>();

    public void OnStickmanEnter(GameObject stickman, int stairIndex)
    {
        if (!stickman.CompareTag("blue")) return;

        if (claimedStairs.Contains(stairIndex)) return;

        claimedStairs.Add(stairIndex);

        // Tính coin theo stairIndex
        float rawCoin = baseCoin + baseCoin * 0.1f * stairIndex;
        float coin = rawCoin / totalStairs;
        int finalCoin = Mathf.RoundToInt(coin);

        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.AddCoin(finalCoin);
        }

        Debug.Log($"[StairManager] Bậc {stairIndex + 1} → raw: {rawCoin} → chia {totalStairs} → +{finalCoin} coin");
    }
}
