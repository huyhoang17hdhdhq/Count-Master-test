﻿using System.Collections;
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

    [Header("Image dùng để hiện khi dừng spawn")]
    public List<Image> targetImages;

    private List<List<GameObject>> spawnedStickmen = new List<List<GameObject>>();
    private List<int> currentCounts = new List<int>();

    private BuyBuilding buyBuilding;

    void Start()
    {
        buyBuilding = FindObjectOfType<BuyBuilding>();

        for (int i = 0; i < stickmanPrefabs.Count; i++)
        {
            int index = i;

            spawnedStickmen.Add(new List<GameObject>());
            currentCounts.Add(0);

            if (activateButtons != null && i < activateButtons.Count)
            {
                activateButtons[i].onClick.AddListener(() => OnActivateButtonClicked(index));
            }

            StartCoroutine(SpawnStickmanRoutine(index));
        }
    }

    IEnumerator SpawnStickmanRoutine(int index)
    {
        while (true)
        {
            // ❌ Không spawn nếu chưa mua building
            if (buyBuilding != null &&
                (index >= buyBuilding.buildingUnlocked.Count || !buyBuilding.buildingUnlocked[index]))
            {
                yield return new WaitForSeconds(spawnInterval);
                continue;
            }

            // ✅ Nếu thanh fill trong ShipManager chưa đầy thì spawn
            if (ShipManager.Instance != null && !ShipManager.Instance.IsFull())
            {
                if (currentCounts[index] < maxStickmanCount)
                {
                    SpawnStickman(index);
                }

                // Ẩn target image khi đang spawn
                if (targetImages != null && index < targetImages.Count)
                    targetImages[index].gameObject.SetActive(false);
            }
            else
            {
                // ✅ Nếu ShipManager đầy thì hiện target image
                if (targetImages != null && index < targetImages.Count)
                    targetImages[index].gameObject.SetActive(true);
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
            parentContainers[index]
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
        for (int i = spawnedStickmen[index].Count - 1; i >= 0; i--)
        {
            GameObject stickman = spawnedStickmen[index][i];

            if (stickman == null)
            {
                spawnedStickmen[index].RemoveAt(i);
                continue;
            }

            StickmanMover mover = stickman.GetComponent<StickmanMover>();
            if (mover != null)
            {
                mover.enabled = true;
            }
        }

        FindObjectOfType<StickmanLineupManager>()?.UpdateLineupAtIndex(index);

        currentCounts[index] = 0;
        UpdateFillBar(index);
    }
}
