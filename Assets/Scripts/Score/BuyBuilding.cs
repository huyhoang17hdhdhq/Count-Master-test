using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyBuilding : MonoBehaviour
{
    [Header("Text hiển thị giá lần mua hiện tại")]
    public TMP_Text costText;

    [Header("Nút mua")]
    public Button buyButton;

    [Header("Danh sách tòa nhà cần mua")]
    public List<GameObject> buildings;

    [Header("Danh sách hiệu ứng FX tương ứng")]
    public List<ParticleSystem> fxEffects;

    [Header("Danh sách giá Diamond cho từng lần mua")]
    public List<int> diamondCosts;

    private int purchaseIndex = 0;

    [HideInInspector]
    public List<bool> buildingUnlocked = new List<bool>();

    private void Start()
    {
        LoadPurchasedBuildings();         // 🔹 Tải dữ liệu tòa nhà đã mua
        UpdateCostText();

        if (buyButton != null)
            buyButton.onClick.AddListener(AttemptPurchase);
    }

    private void UpdateCostText()
    {
        if (costText != null)
        {
            if (purchaseIndex < diamondCosts.Count)
                costText.text = diamondCosts[purchaseIndex].ToString();
            else
                costText.text = "Đã mua hết";
        }
    }

    private void AttemptPurchase()
    {
        if (purchaseIndex >= diamondCosts.Count)
        {
            Debug.Log("Đã mua hết tất cả tòa nhà.");
            return;
        }

        int cost = diamondCosts[purchaseIndex];
        if (CoinManager.Instance.GetDiamond() >= cost)
        {
            CoinManager.Instance.AddDiamond(-cost);

            if (purchaseIndex < buildings.Count && buildings[purchaseIndex] != null)
                buildings[purchaseIndex].SetActive(true);

            if (purchaseIndex < fxEffects.Count && fxEffects[purchaseIndex] != null)
                fxEffects[purchaseIndex].Play();

            // Đánh dấu đã mở building này
            if (purchaseIndex >= buildingUnlocked.Count)
            {
                for (int i = buildingUnlocked.Count; i <= purchaseIndex; i++)
                    buildingUnlocked.Add(false);
            }

            buildingUnlocked[purchaseIndex] = true;
            purchaseIndex++;

            SavePurchasedBuildings();     // 🔹 Lưu lại sau khi mua
            UpdateCostText();
        }
    }

    private void SavePurchasedBuildings()
    {
        List<string> flags = new List<string>();
        foreach (bool unlocked in buildingUnlocked)
        {
            flags.Add(unlocked ? "1" : "0");
        }

        string saveString = string.Join(",", flags);
        PlayerPrefs.SetString("BuildingsUnlocked", saveString);
        PlayerPrefs.Save();
    }

    private void LoadPurchasedBuildings()
    {
        string saved = PlayerPrefs.GetString("BuildingsUnlocked", "");

        if (!string.IsNullOrEmpty(saved))
        {
            string[] flags = saved.Split(',');
            buildingUnlocked.Clear();

            for (int i = 0; i < flags.Length; i++)
            {
                bool isUnlocked = flags[i] == "1";
                buildingUnlocked.Add(isUnlocked);

                if (i < buildings.Count && isUnlocked)
                    buildings[i].SetActive(true);
            }

            purchaseIndex = buildingUnlocked.LastIndexOf(true) + 1;
        }
        else
        {
            // Nếu chưa có gì được lưu, khởi tạo trạng thái mặc định
            for (int i = 0; i < buildings.Count; i++)
                buildingUnlocked.Add(false);
        }
    }
}
