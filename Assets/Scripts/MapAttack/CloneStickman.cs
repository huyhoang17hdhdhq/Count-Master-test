using UnityEngine;

public class CloneStickMan : MonoBehaviour
{
    [Header("Prefab Stickman")]
    public GameObject stickmanPrefab;

    [Header("Vị trí trung tâm đội hình")]
    public Vector3 center = Vector3.zero;

    [Header("Khoảng cách giữa các Stickman (mật độ)")]
    public float spacing = 1.5f;

    [Header("Parent chứa các Stickman clone")]
    public Transform parentContainer;

    private void Start()
    {
        ShipManager shipManager = FindObjectOfType<ShipManager>();

        if (shipManager != null && stickmanPrefab != null)
        {
            int maxScore = shipManager.MaxScore;
            CloneNow(maxScore);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy ShipManager hoặc chưa gán stickmanPrefab.");
        }
    }

    /// <summary>
    /// Hàm clone số lượng stickman bất kỳ không phụ thuộc ShipManager
    /// </summary>
    public void CloneNow(int amount)
    {
        ClearClones();
        SpawnStickmen(amount);
        ArrangeHexGrid();
    }

    /// <summary>
    /// Xoá tất cả stickman đã clone
    /// </summary>
    public void ClearClones()
    {
        if (parentContainer != null)
        {
            for (int i = parentContainer.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(parentContainer.GetChild(i).gameObject);
            }
        }
    }

    private void SpawnStickmen(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject stickman = Instantiate(stickmanPrefab, Vector3.zero, Quaternion.identity);

            if (parentContainer != null)
                stickman.transform.SetParent(parentContainer, worldPositionStays: false);
        }
    }

    /// <summary>
    /// Sắp xếp các Stickman theo dạng lưới lục giác đều
    /// </summary>
    private void ArrangeHexGrid()
    {
        int count = parentContainer.childCount;
        if (count == 0) return;

        float hexWidth = spacing;
        float hexHeight = Mathf.Sqrt(3f) / 2f * spacing;

        int columns = Mathf.CeilToInt(Mathf.Sqrt(count));
        int rows = Mathf.CeilToInt((float)count / columns);

        int index = 0;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (index >= count) break;

                Transform child = parentContainer.GetChild(index);

                float offsetX = (row % 2 == 0) ? 0f : hexWidth * 0.5f;

                Vector3 localPos = new Vector3(
                    col * hexWidth + offsetX,
                    0,
                    row * hexHeight
                );

                // Center the whole group
                float centerX = (columns - 1) * hexWidth / 2f;
                float centerZ = (rows - 1) * hexHeight / 2f;
                localPos -= new Vector3(centerX, 0, centerZ);

                child.localPosition = center + localPos;
                index++;
            }
        }
    }

    [ContextMenu("Test Clone 10 Stickman")]
    private void TestClone()
    {
        CloneNow(10);
    }
}
