using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelStickMan : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI LevelTxt;

    [Header("Danh sách nút gọi RefreshStickMan()")]
    [SerializeField] private List<Button> refreshButtons;

    private int currentTarget = 2;
    public int numberLevel;

    private void Start()
    {
        // Đọc level đã lưu
        numberLevel = PlayerPrefs.GetInt("NumberLevel", 1); // Mặc định là 1 nếu chưa có

        // Tạo stickman theo level đã lưu
        PlayerManager.PlayerManagerInstance.MakeStickMan(numberLevel);

        // Gán sự kiện click cho từng button trong danh sách
        for (int i = 0; i < refreshButtons.Count; i++)
        {
            int index = i; // giữ lại index nếu bạn cần sau này
           
            refreshButtons[i].onClick.AddListener(() => StartCoroutine(DelayedRefresh()));
        }
    }

    private void OnEnable()
    {
        // Lắng nghe sự kiện đổi skin
        PickSkin.OnSkinChanged += RefreshStickMan;
        Debug.Log("LevelStickMan: OnEnable chạy");
    }

    private void OnDisable()
    {
        // Hủy đăng ký sự kiện
        PickSkin.OnSkinChanged -= RefreshStickMan;
    }

    public void RefreshStickMan()
    {
        Debug.Log("RefreshStickMan(): Đang xóa và clone lại stickman");

        PlayerManager.PlayerManagerInstance.ClearCloneStickmans();

        Debug.Log("Sẽ clone lại bằng index: " + PlayerPrefs.GetInt("SelectedPlayerIndex", 0));

        PlayerManager.PlayerManagerInstance.MakeStickMan(numberLevel);
    }

    private IEnumerator DelayedRefresh()
    {
        yield return new WaitForSeconds(0.5f);
        RefreshStickMan();
    }

    public void levelStickMan()
    {
        // Tăng số lượng stickman
        PlayerManager.PlayerManagerInstance.MakeStickMan(currentTarget);
        currentTarget++;

        // Cập nhật và lưu số lượng level hiện tại
        numberLevel = PlayerManager.PlayerManagerInstance.numberOfStickmans;
        PlayerPrefs.SetInt("NumberLevel", numberLevel);
        PlayerPrefs.Save();
    }

    private void Update()
    {
        // Cập nhật hiển thị level
        LevelTxt.text = PlayerManager.PlayerManagerInstance.numberOfStickmans.ToString();
    }
}
