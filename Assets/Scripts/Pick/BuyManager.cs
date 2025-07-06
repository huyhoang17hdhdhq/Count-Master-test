using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyManager : MonoBehaviour
{
    [Header("Danh sách giá vật phẩm")]
    public List<int> itemCosts;

    [Header("Nút Buy duy nhất")]
    public Button buyButton;

    [Header("Text hiển thị giá")]
    public TMP_Text priceText;

    [Header("Danh sách các object bị ẩn sau khi mua")]
    public List<GameObject> unlockObjects;

    private int currentIndex = 0;
    private const string PlayerPrefKey = "BuyLevelIndex";

    private void Start()
    {
        // Đọc index đã lưu nếu có
        currentIndex = PlayerPrefs.GetInt(PlayerPrefKey, 0);

        // Ẩn các Unlock đã được mua
        for (int i = 0; i < unlockObjects.Count; i++)
        {
            if (PlayerPrefs.GetInt("Unlock_" + i, 0) == 1)
            {
                if (unlockObjects[i] != null)
                    unlockObjects[i].SetActive(false);
            }
        }

        UpdateBuyButtonUI();
    }

    private void Update()
    {
        UpdateBuyButtonUI();
    }

    public void UpdateBuyButtonUI()
    {
        if (currentIndex >= itemCosts.Count)
        {
            
            buyButton.interactable = false;
            buyButton.GetComponent<Image>().color = new Color32(193, 193, 193, 150);
            return;
        }

        int cost = itemCosts[currentIndex];
        int currentCoin = CoinManager.Instance.GetCoin();

        priceText.text = cost.ToString();

        Image btnImage = buyButton.GetComponent<Image>();

        if (currentCoin >= cost)
        {
            btnImage.color = new Color32(255, 255, 255, 255);
            btnImage.raycastTarget = true;
            buyButton.interactable = true;
        }
        else
        {
            btnImage.color = new Color32(193, 193, 193, 150);
            btnImage.raycastTarget = false;
            buyButton.interactable = false;
        }
    }

    public void TryBuy()
    {
        if (currentIndex >= itemCosts.Count)
        {
            Debug.Log("Đã mua hết các vật phẩm.");
            return;
        }

        int cost = itemCosts[currentIndex];
        int currentCoin = CoinManager.Instance.GetCoin();

        if (currentCoin >= cost)
        {
            CoinManager.Instance.AddCoin(-cost);
            Debug.Log($"Mua thành công món {currentIndex} với giá {cost}");

            // Ẩn unlockObject tương ứng nếu có
            if (currentIndex < unlockObjects.Count && unlockObjects[currentIndex] != null)
            {
                unlockObjects[currentIndex].SetActive(false);

                // ✅ Lưu lại trạng thái đã ẩn
                PlayerPrefs.SetInt("Unlock_" + currentIndex, 1);
            }

            // Lưu lại index mới
            currentIndex++;
            PlayerPrefs.SetInt(PlayerPrefKey, currentIndex);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("Không đủ coin để mua.");
        }
    }
}
