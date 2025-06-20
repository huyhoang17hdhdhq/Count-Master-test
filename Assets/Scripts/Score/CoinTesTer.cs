using UnityEngine;
using UnityEngine.UI;

public class CoinTesTer : MonoBehaviour
{
    [Header("Nút test cộng Coin và Diamond")]
    public Button addButton;

    private void Start()
    {
        if (addButton != null)
        {
            addButton.onClick.AddListener(AddTestCurrency);
        }
        else
        {
            Debug.LogWarning("Chưa gán Button test coin!");
        }
    }

    private void AddTestCurrency()
    {
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.AddCoin(50);
            CoinManager.Instance.AddDiamond(50);
            Debug.Log("Đã cộng +50 coin và +50 diamond!");
        }
        else
        {
            Debug.LogError("CoinManager chưa tồn tại trong scene.");
        }
    }
}
