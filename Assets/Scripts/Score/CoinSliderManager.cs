using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinSliderManager : MonoBehaviour
{
    public Slider coinSlider;
    public float maxCoin = 300f;

    public float increaseSpeed = 100f; // tốc độ tăng mỗi giây
    private float currentCoin = 0f;
    private float targetCoin = 0f;
    public static CoinSliderManager Instance;



    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentCoin = PlayerPrefs.GetFloat("SavedCoinSlider", 0f);
        targetCoin = currentCoin;

        coinSlider.maxValue = maxCoin;
        coinSlider.value = currentCoin;
    }

    private void Update()
    {
        if (currentCoin < targetCoin)
        {
            currentCoin += increaseSpeed * Time.deltaTime;
            currentCoin = Mathf.Min(currentCoin, targetCoin);
            coinSlider.value = currentCoin;

            PlayerPrefs.SetFloat("SavedCoinSlider", currentCoin);
        }

        // 👉 Khi đầy thì cộng coin và reset
        if (currentCoin >= maxCoin)
        {
            CoinManager.Instance.AddCoin(100); // ✅ Cộng 100 coin
            currentCoin = 0f;
            targetCoin = 0f;
            coinSlider.value = 0f;

            PlayerPrefs.SetFloat("SavedCoinSlider", 0f); // ✅ Reset lưu mốc
        }
    }


    // ✅ Gọi từ class khác để tăng thêm coin và hiển thị text
    public void AddCoinToSlider(int amount)
    {
        if (currentCoin >= maxCoin) return;

        // ✅ Chỉ tăng thêm `amount`, không phải set max
        targetCoin = Mathf.Min(targetCoin + amount, maxCoin);

        Debug.Log("Tăng coin slider thêm: " + amount + ", new target = " + targetCoin);
    }

}
