using UnityEngine;
using UnityEngine.UI;

public class ButtonClickLogger : MonoBehaviour
{
    public Button[] buttons; // Gắn 9 button theo đúng thứ tự 1 -> 9

    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // tránh lỗi closure
            buttons[i].onClick.AddListener(() => OnButtonClicked(index));
            Debug.Log($"Đã gán sự kiện click cho button {index + 1}");
        }
    }

    void OnButtonClicked(int index)
    {
        Debug.Log($"[Click] Đã click vào Button {index + 1}");
    }
}
