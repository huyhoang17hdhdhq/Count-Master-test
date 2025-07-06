using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HierarchySwapper : MonoBehaviour
{
    [Header("GameObject hiện đang là target")]
    public Transform target;

    [Header("Danh sách các GameObject để hoán đổi với target")]
    public List<Transform> swapTargets;

    [Header("Danh sách các nút tương ứng với từng index")]
    public List<Button> buttons;

    private void Start()
    {
        // Gắn sự kiện cho button như cũ
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => SwapWithIndex(index));
        }

        // 🔥 Load lại trạng thái đã swap nếu có
        if (PlayerPrefs.HasKey("LastSwapIndex"))
        {
            int savedIndex = PlayerPrefs.GetInt("LastSwapIndex");

            // Swap ngay khi scene bắt đầu (chỉ 1 lần)
            if (savedIndex >= 0 && savedIndex < swapTargets.Count)
            {
                Swap(target, swapTargets[savedIndex]);
                target = swapTargets[savedIndex];
            }
        }
    }


    /// <summary>
    /// Hoán đổi target với object tại index trong danh sách
    /// </summary>
    public void SwapWithIndex(int index)
    {
        if (target == null) return;
        if (index < 0 || index >= swapTargets.Count) return;

        Transform other = swapTargets[index];
        if (other == null) return;

        Swap(target, other);

        // Cập nhật target mới
        target = other;

        // 🔥 Lưu index vào PlayerPrefs
        PlayerPrefs.SetInt("LastSwapIndex", index);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Đổi vị trí, cha và sibling index giữa hai object
    /// </summary>
    private void Swap(Transform a, Transform b)
    {
        Transform parentA = a.parent;
        int siblingIndexA = a.GetSiblingIndex();
        Vector3 positionA = a.position;
        Quaternion rotationA = a.rotation;

        Transform parentB = b.parent;
        int siblingIndexB = b.GetSiblingIndex();
        Vector3 positionB = b.position;
        Quaternion rotationB = b.rotation;

        a.SetParent(null);
        b.SetParent(null);

        a.SetParent(parentB);
        a.position = positionB;
        a.rotation = rotationB;
        a.SetSiblingIndex(siblingIndexB);

        b.SetParent(parentA);
        b.position = positionA;
        b.rotation = rotationA;
        b.SetSiblingIndex(siblingIndexA);
    }
}
