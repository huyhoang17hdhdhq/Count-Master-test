using System.Collections.Generic;
using UnityEngine;

public class StickmanLineupManager : MonoBehaviour
{
    [Header("Danh sách các container chứa Stickman")]
    public List<Transform> parentContainers;

    [Header("Khoảng cách giữa các Stickman")]
    public float spacing = 1.5f;

    public void UpdateLineup()
    {
        for (int c = 0; c < parentContainers.Count; c++)
        {
            Transform parent = parentContainers[c];
            int count = parent.childCount;

            for (int i = 1; i < count; i++)
            {
                Transform prev = parent.GetChild(i - 1);
                Transform current = parent.GetChild(i);

                // Tính hướng ngược lại với hướng của stickman trước
                Vector3 backwardDir = -prev.forward.normalized;

                Vector3 targetPos = prev.position + backwardDir * spacing;
                current.position = targetPos;

                // Xoay cùng hướng với stickman trước
                current.rotation = prev.rotation;
            }
        }
    }

    // Gọi cập nhật cho 1 nhóm cụ thể theo index
    public void UpdateLineupAtIndex(int index)
    {
        if (index < 0 || index >= parentContainers.Count) return;

        Transform parent = parentContainers[index];
        int count = parent.childCount;

        for (int i = 1; i < count; i++)
        {
            Transform prev = parent.GetChild(i - 1);
            Transform current = parent.GetChild(i);

            Vector3 backwardDir = -prev.forward.normalized;
            Vector3 targetPos = prev.position + backwardDir * spacing;
            current.position = targetPos;
            current.rotation = prev.rotation;
        }
    }
}
