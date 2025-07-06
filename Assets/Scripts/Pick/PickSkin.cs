using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickSkin : MonoBehaviour
{
    [Header("Danh sách player skin (nếu dùng)")]
    public List<GameObject> players;

    [Header("Danh sách player chính hiển thị khi chọn")]
    public List<GameObject> playerMain;

    [Header("Danh sách button tương ứng")]
    public List<Button> buttons;

    [Header("Danh sách dấu chọn (mỗi button có 1 mark sẵn)")]
    public List<GameObject> selectedMarks;

    private int selectedIndex = 0;
    private const string PlayerPrefKey = "SelectedPlayerIndex";

    private void Start()
    {
        // Gán sự kiện click cho từng button
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => OnButtonClicked(index));
        }

        // Lấy index đã lưu (mặc định là 0 nếu chưa có)
        selectedIndex = PlayerPrefs.GetInt(PlayerPrefKey, 0);

        // Hiện dấu chọn & player tương ứng
        UpdateSelectedMarks(selectedIndex);
        UpdatePlayerMain(selectedIndex);
    }

    private void OnButtonClicked(int index)
    {
        selectedIndex = index;

        // Lưu vào PlayerPrefs
        PlayerPrefs.SetInt(PlayerPrefKey, selectedIndex);
        PlayerPrefs.Save();

        Debug.Log("Đã chọn player index: " + selectedIndex);

        // Cập nhật dấu chọn và player
        UpdateSelectedMarks(index);
        UpdatePlayerMain(index);
    }

    private void UpdateSelectedMarks(int activeIndex)
    {
        for (int i = 0; i < selectedMarks.Count; i++)
        {
            if (selectedMarks[i] != null)
                selectedMarks[i].SetActive(i == activeIndex); // chỉ bật mark tại index được chọn
        }
    }

    private void UpdatePlayerMain(int activeIndex)
    {
        for (int i = 0; i < playerMain.Count; i++)
        {
            if (playerMain[i] != null)
                playerMain[i].SetActive(i == activeIndex); // chỉ bật playerMain tại index được chọn
        }
    }

    public int GetSelectedPlayerIndex()
    {
        return selectedIndex;
    }
}
