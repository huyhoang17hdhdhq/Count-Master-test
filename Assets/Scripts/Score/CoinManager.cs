using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [Header("UI Text")]
    public TMP_Text coinText;
    public TMP_Text diamondText;

    [Header("Hiển thị số coin mới được cộng")]
    public TMP_Text recentCoinText;
    public TMP_Text recentDiamondText;

    private int coin;
    private int diamond;

    private int lastAddedCoin = 0;
    private int lastAddedDiamond = 0;

    private void Awake()
    {
        Instance = this;

        // Đọc dữ liệu từ PlayerPrefs
        coin = PlayerPrefs.GetInt("Coin", 0);
        diamond = PlayerPrefs.GetInt("Diamond", 0);
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddCoin(int amount)
    {
        coin += amount;
        lastAddedCoin += amount; // ✅ cộng dồn thay vì gán đè

        if (coin < 0) coin = 0;
        PlayerPrefs.SetInt("Coin", coin);
        UpdateUI();

        ShowLastAddedCoin(); // ✅ vẫn gọi cập nhật hiển thị
    }


    public void AddDiamond(int amount)
    {
        diamond += amount;
        if (diamond < 0) diamond = 0;
        lastAddedDiamond += amount;
        PlayerPrefs.SetInt("Diamond", diamond);
        UpdateUI();

        ShowLastAddedDiamond();
    }

    public int GetDiamond()
    {
        return diamond;
    }

    public int GetCoin()
    {
        return coin;
    }

    private void UpdateUI()
    {
        if (coinText != null)
            coinText.text = coin.ToString();

        if (diamondText != null)
            diamondText.text = diamond.ToString();
    }

    // ✅ Hàm mới để hiển thị coin mới cộng
    private void ShowLastAddedCoin()
    {
        if (recentCoinText != null)
            recentCoinText.text = "+" + lastAddedCoin.ToString();
       ;
    }
    private void ShowLastAddedDiamond()
    {
        if (recentDiamondText != null)
            recentDiamondText.text = "+" + lastAddedDiamond.ToString();
    }
}
