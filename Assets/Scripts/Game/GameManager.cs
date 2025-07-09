using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Tất cả prefab trong scene (10 cái chẳng hạn)")]
    public List<GameObject> prefabsInScene;

    [Header("Các vị trí target (5 cái)")]
    public List<Transform> targetPositions;

    private void Start()
    {
        if (prefabsInScene.Count < targetPositions.Count)
        {
            Debug.LogError("Không đủ prefab để thay cho target!");
            return;
        }

        List<GameObject> selectedPrefabs = GetRandomPrefabs(prefabsInScene, targetPositions.Count);

        for (int i = 0; i < targetPositions.Count; i++)
        {
            GameObject prefab = selectedPrefabs[i];
            Transform target = targetPositions[i];

            // Lưu lại thông tin vị trí cũ của target trong hierarchy
            int targetSiblingIndex = target.GetSiblingIndex();
            Transform parent = target.parent;

            // Đặt prefab vào đúng vị trí trong scene
            prefab.transform.position = target.position;
            

            // Đưa prefab vào đúng chỗ của target trong hierarchy
            prefab.transform.SetParent(parent);
            prefab.transform.SetSiblingIndex(targetSiblingIndex);

            Debug.Log($"✅ Đã đặt {prefab.name} vào vị trí của {target.name} tại index {targetSiblingIndex}");

            // Ẩn hoặc xoá target (tuỳ bạn chọn)
            target.gameObject.SetActive(false); // hoặc: Destroy(target.gameObject);
        }
    }

    private List<GameObject> GetRandomPrefabs(List<GameObject> sourceList, int count)
    {
        List<GameObject> tempList = new List<GameObject>(sourceList);
        List<GameObject> result = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            int randIndex = Random.Range(0, tempList.Count);
            result.Add(tempList[randIndex]);
            tempList.RemoveAt(randIndex); // không trùng
        }

        return result;
    }
}
