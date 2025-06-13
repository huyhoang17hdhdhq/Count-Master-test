using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickManMap : MonoBehaviour
{
    [Header("Prefab & Vị trí spawn")]
    public List<GameObject> stickmanPrefabs;
    public List<Transform> spawnPoints;

    [Header("Parent chứa Stickman")]
    public List<Transform> parentContainers;


    [Header("Thanh fill thể hiện số lượng Stickman")]
    public List<Image> fillBars;

    [Header("Cài đặt spawn")]
    public float spawnInterval = 3f;
    public int maxStickmanCount = 10;

    [Header("Danh sách Button để bật StickManMover")]
    public List<Button> activateButtons;


    [Header("Image dùng để hiện trong 30s")]
    public List<Image> targetImages;

    // Danh sách các Stickman đã spawn, chia theo index
    private List<List<GameObject>> spawnedStickmen = new List<List<GameObject>>();
    private List<int> currentCounts = new List<int>();
    private List<bool> isPausedList = new List<bool>();

    void Start()
    {
        // Khởi tạo các danh sách
        for (int i = 0; i < stickmanPrefabs.Count; i++)
        {
            int index = i;

            spawnedStickmen.Add(new List<GameObject>());
            currentCounts.Add(0);
            isPausedList.Add(false);

            // Gán listener cho mỗi nút
            if (activateButtons != null && i < activateButtons.Count)
            {
                activateButtons[i].onClick.AddListener(() => OnActivateButtonClicked(index));
            }

            // Bắt đầu coroutine spawn cho từng nhóm Stickman
            StartCoroutine(SpawnStickmanRoutine(index));
        }
    }

    IEnumerator SpawnStickmanRoutine(int index)
    {
        while (true)
        {
            if (!isPausedList[index] && currentCounts[index] < maxStickmanCount)
            {
                SpawnStickman(index);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnStickman(int index)
    {
        if (index >= stickmanPrefabs.Count || index >= spawnPoints.Count || index >= parentContainers.Count) return;

        GameObject newStickman = Instantiate(
            stickmanPrefabs[index],
            spawnPoints[index].position,
            spawnPoints[index].rotation,
            parentContainers[index] // Gán đúng parent theo index
        );

        spawnedStickmen[index].Add(newStickman);
        currentCounts[index]++;
        UpdateFillBar(index);
    }


    void UpdateFillBar(int index)
    {
        if (fillBars != null && index < fillBars.Count)
        {
            fillBars[index].fillAmount = (float)currentCounts[index] / maxStickmanCount;
        }
    }

    void OnActivateButtonClicked(int index)
    {
        // Bật StickmanMover của nhóm tương ứng
        foreach (GameObject stickman in spawnedStickmen[index])
        {
            StickmanMover mover = stickman.GetComponent<StickmanMover>();
            if (mover != null)
            {
                mover.enabled = true;
            }
        }

        FindObjectOfType<StickmanLineupManager>()?.UpdateLineupAtIndex(index);


        

        // Reset thanh fill & số lượng
        currentCounts[index] = 0;
        UpdateFillBar(index);

        // Hiện image & tắt sau 30s
        if (targetImages != null && index < targetImages.Count)
        {
            targetImages[index].gameObject.SetActive(true);
            StartCoroutine(DisableImageAfterTime(index, 30f));
        }

        // Tạm ngưng spawn trong 30s
        StartCoroutine(PauseSpawning(index, 30f));
    }

    IEnumerator PauseSpawning(int index, float duration)
    {
        isPausedList[index] = true;
        yield return new WaitForSeconds(duration);
        isPausedList[index] = false;
    }

    IEnumerator DisableImageAfterTime(int index, float duration)
    {
        yield return new WaitForSeconds(duration);

        if (targetImages != null && index < targetImages.Count)
        {
            targetImages[index].gameObject.SetActive(false);
        }
    }
}
