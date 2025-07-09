using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine.UI;

public class GateAttack : MonoBehaviour
{
    [Header("List nút bấm cho từng hàng")]
    public List<Button> list1Buttons;
    public List<Button> list2Buttons;
    public List<Button> list3Buttons;

    [Header("Các điểm tương ứng (Transform)")]
    public List<Transform> list1Points;
    public List<Transform> list2Points;
    public List<Transform> list3Points;

    private int currentListIndex = 0; // Đang chọn list nào (0 -> 1 -> 2)
    private List<int> selectedIndices = new List<int>(); // Lưu index đã chọn
    private int lastSelectedIndex = -1; // Index của list trước đó

    private int numberOfStickmans;
    public Transform player;
    [SerializeField] private GameObject stickMan;
    [Range(0f, 1f)][SerializeField] private float DistanceFactor, Radius;

    [Header("Điểm cố định cuối cùng sau khi chạy qua 3 điểm")]
    public Transform finalPoint;

    void Start()
    {
        player = transform;

        SetupButtons(list1Buttons, 0);
        SetupButtons(list2Buttons, 1);
        SetupButtons(list3Buttons, 2);

        // Reset màu ban đầu nếu cần
        ResetButtonColors(list1Buttons);
        ResetButtonColors(list2Buttons);
        ResetButtonColors(list3Buttons);
    }

    public void MakeStickMan(int number)
    {
        for (int i = numberOfStickmans; i < number; i++)
        {
            Instantiate(stickMan, transform.position, quaternion.identity, transform);
        }

        numberOfStickmans = transform.childCount - 1;

        FormatStickMan();
    }

    public void FormatStickMan()
    {
        for (int i = 1; i < player.childCount; i++)
        {
            var x = DistanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * Radius);
            var z = DistanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * Radius);

            var NewPos = new Vector3(x, -0.55f, z);
            player.transform.GetChild(i).DOLocalMove(NewPos, 0.5f).SetEase(Ease.OutBack);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("gate"))
        {
            other.transform.parent.GetChild(0).GetComponent<BoxCollider>().enabled = false; // gate 1
            other.transform.parent.GetChild(1).GetComponent<BoxCollider>().enabled = false; // gate 2

            var gateManager = other.GetComponent<GateManager>();
            numberOfStickmans = transform.childCount - 1;

            if (gateManager.multiply)
            {
                MakeStickMan(numberOfStickmans * gateManager.randomNumber);
            }
            else
            {
                MakeStickMan(numberOfStickmans + gateManager.randomNumber);
            }
        }
    }

    void SetupButtons(List<Button> buttonList, int listIndex)
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            int capturedIndex = i; // Để tránh lỗi closure
            buttonList[i].onClick.AddListener(() => OnButtonClicked(listIndex, capturedIndex));
        }
    }

    void OnButtonClicked(int listIndex, int indexInList)
    {
        if (listIndex != currentListIndex)
            return; // Chỉ cho chọn đúng list

        List<Button> currentButtons = GetButtonListByIndex(listIndex);

        for (int i = 0; i < currentButtons.Count; i++)
        {
            Button btn = currentButtons[i];

            if (i == indexInList)
            {
                btn.GetComponent<Image>().color = Color.green; // ✅ Đổi màu nút đã chọn
            }

            btn.interactable = false; // ❌ Tắt tất cả button trong list (chỉ chọn 1 lần)
        }

        selectedIndices.Add(indexInList);
        lastSelectedIndex = indexInList;
        currentListIndex++;

        Debug.Log($"Chọn List {listIndex + 1}, Index: {indexInList}");

        if (currentListIndex < 3)
        {
            ApplySelectionRules(GetButtonListByIndex(currentListIndex), lastSelectedIndex);
        }
        else
        {
            Debug.Log("Đã chọn đủ 3 list:");
            for (int i = 0; i < selectedIndices.Count; i++)
            {
                Debug.Log($"- List {i + 1}: Index {selectedIndices[i]} → Điểm: {GetPointBySelection(i, selectedIndices[i]).name}");
            }

            MovePlayerAlongSelectedPoints();

        }
    }

    void ApplySelectionRules(List<Button> nextButtonList, int prevIndex)
    {
        for (int i = 0; i < nextButtonList.Count; i++)
        {
            bool interactable = true;

            if (prevIndex == 0 && i == 2) // Chọn 1 → không được chọn 3
                interactable = false;
            else if (prevIndex == 2 && i == 0) // Chọn 3 → không được chọn 1
                interactable = false;

            nextButtonList[i].interactable = interactable;
        }
    }

    void ResetButtonColors(List<Button> buttonList)
    {
        foreach (var btn in buttonList)
        {
            btn.GetComponent<Image>().color = Color.white;
        }
    }

    List<Button> GetButtonListByIndex(int listIndex)
    {
        return listIndex switch
        {
            0 => list1Buttons,
            1 => list2Buttons,
            2 => list3Buttons,
            _ => null
        };
    }

    Transform GetPointBySelection(int listIndex, int pointIndex)
    {
        return listIndex switch
        {
            0 => list1Points[pointIndex],
            1 => list2Points[pointIndex],
            2 => list3Points[pointIndex],
            _ => null
        };
    }
    void MovePlayerAlongSelectedPoints()
    {
        List<Transform> path = new List<Transform>();

        for (int i = 0; i < selectedIndices.Count; i++)
        {
            Transform point = GetPointBySelection(i, selectedIndices[i]);
            path.Add(point);
        }

        // 👉 Thêm điểm cuối cố định
        if (finalPoint != null)
        {
            path.Add(finalPoint);
        }

        StartCoroutine(MoveAlongPoints(path, 1f));
    }


    IEnumerator MoveAlongPoints(List<Transform> points, float durationPerPoint)
    {
        foreach (var point in points)
        {
            // Di chuyển từ vị trí hiện tại đến điểm tiếp theo trong duration
            Vector3 startPos = player.position;
            Vector3 targetPos = point.position;
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime / durationPerPoint;
                player.position = Vector3.Lerp(startPos, targetPos, t);
                yield return null;
            }

            player.position = targetPos;
        }

        Debug.Log("🏁 Player đã đến điểm cuối cùng.");
    }

}
