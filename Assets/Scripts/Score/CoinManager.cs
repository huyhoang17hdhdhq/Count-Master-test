using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public TMP_Text coinText;
    public TMP_Text diamondText;

    private int coin;
    private int diamond;

    private void Awake()
    {
        Instance = this; // Không cần DontDestroyOnLoad

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
        if (coin < 0) coin = 0;
        PlayerPrefs.SetInt("Coin", coin);
        UpdateUI();
    }

    public void AddDiamond(int amount)
    {
        diamond += amount;
        if (diamond < 0) diamond = 0;
        PlayerPrefs.SetInt("Diamond", diamond);
        UpdateUI();
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

}
