using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointSelectionManager : MonoBehaviour
{
    public Button[] pointButtons;           // Gắn đúng thứ tự từ 1 đến 9
    public Transform[] pointPositions;      // Vị trí các điểm tương ứng
    public GameObject stickman;             // Stickman cần di chuyển

    private List<int> selectedPoints = new List<int>();
    private bool[] rowUsed = new bool[3];   // Đánh dấu các hàng đã chọn
    private bool isMoving = false;

    private void Start()
    {
        for (int i = 0; i < pointButtons.Length; i++)
        {
            int index = i;
            pointButtons[i].onClick.AddListener(() => OnPointSelected(index));
        }

        Debug.Log("Khởi tạo xong. Chờ người dùng chọn điểm.");
    }

    void OnPointSelected(int index)
    {
        Debug.Log($"[Click] Đã click vào point {index + 1}");

        if (isMoving)
        {
            Debug.Log("Stickman đang di chuyển. Không thể chọn thêm.");
            return;
        }

        int row = index / 3;

        // Kiểm tra thứ tự hàng: chỉ được chọn hàng tiếp theo khi hàng trước đã có
        for (int r = 0; r < row; r++)
        {
            if (!rowUsed[r])
            {
                Debug.Log($"⚠️ Cần chọn điểm ở hàng {r + 1} trước khi chọn hàng {row + 1}");
                return;
            }
        }

        if (rowUsed[row])
        {
            Debug.Log($"⚠️ Hàng {row + 1} đã có điểm được chọn rồi.");
            return;
        }

        // Kiểm tra khoảng cách không hợp lệ
        if (selectedPoints.Count > 0)
        {
            int lastIndex = selectedPoints[selectedPoints.Count - 1];
            if ((lastIndex == 0 && index == 5) ||
                (lastIndex == 2 && index == 3) ||
                (lastIndex == 3 && index == 8) ||
                (lastIndex == 5 && index == 6))
            {
                Debug.Log($"❌ Không thể chọn point {index + 1} vì vi phạm giới hạn khoảng cách với point {lastIndex + 1}");
                return;
            }
        }

        selectedPoints.Add(index);
        rowUsed[row] = true;
        pointButtons[index].interactable = false;

        Debug.Log($"✔️ Đã chọn point {index + 1} (hàng {row + 1})");

        if (selectedPoints.Count == 3)
        {
            Debug.Log("🎯 Đã chọn đủ 3 điểm! Bắt đầu di chuyển stickman...");
            StartCoroutine(MoveStickman());
        }
    }

    IEnumerator MoveStickman()
    {
        isMoving = true;

        foreach (int i in selectedPoints)
        {
            Vector3 target = pointPositions[i].position;
            Debug.Log($"➡️ Stickman sẽ di chuyển tới point {i + 1} tại {target}");

            while (Vector3.Distance(stickman.transform.position, target) > 0.05f)
            {
                stickman.transform.position = Vector3.MoveTowards(stickman.transform.position, target, 5f * Time.deltaTime);
                yield return null;
            }

            Debug.Log($"✅ Đã đến point {i + 1}");
            yield return new WaitForSeconds(0.3f);
        }

        Debug.Log("🏁 Stickman đã hoàn thành di chuyển cả 3 điểm.");
        isMoving = false;
    }
}
