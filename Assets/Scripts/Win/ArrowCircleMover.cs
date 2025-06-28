using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ArrowCircleMover : MonoBehaviour
{
    [Header("UI Mũi tên")]
    public RectTransform arrowImage;

    [Header("Thông số đường tròn")]
    public float radius = 100f;
    public float startAngle = 45f;
    public float endAngle = 135f;
    public float speed = 50f; // độ/giây

    [Header("Tâm vòng tròn")]
    public Vector2 center = Vector2.zero;

    public bool isSpinning = true;
    private float currentAngle;
    private int spinDirection = 1;

    public GameObject Clonerandom;

    void Start()
    {
        currentAngle = startAngle;
    }

    void Update()
    {
        if (isSpinning)
        {
            float deltaAngle = speed * Time.unscaledDeltaTime * spinDirection;
            currentAngle += deltaAngle;

            if (currentAngle >= endAngle)
            {
                currentAngle = endAngle;
                spinDirection = -1;
            }
            else if (currentAngle <= startAngle)
            {
                currentAngle = startAngle;
                spinDirection = 1;
            }

            UpdateArrow(currentAngle);
        }
    }

    void UpdateArrow(float angleDeg)
    {
        float angleRad = angleDeg * Mathf.Deg2Rad;
        float x = Mathf.Cos(angleRad) * radius;
        float y = Mathf.Sin(angleRad) * radius;
        Vector2 newPos = center + new Vector2(x, y);
        arrowImage.anchoredPosition = newPos;

        float nextAngle = angleDeg + 1f * spinDirection;
        float nextRad = nextAngle * Mathf.Deg2Rad;
        Vector2 nextPos = center + new Vector2(Mathf.Cos(nextRad) * radius, Mathf.Sin(nextRad) * radius);
        Vector2 dir = (nextPos - newPos).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        arrowImage.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }

    public void StopSpinning()
    {
        isSpinning = false;
        CheckImageUnderArrow();
    }

    void CheckImageUnderArrow()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = RectTransformUtility.WorldToScreenPoint(null, arrowImage.position);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            string name = result.gameObject.name;

            if (CloneGateBost.Instance == null)
            {
                Debug.LogWarning("❌ CloneGateBost.Instance is null!");
                return;
            }

            if (name == "10")
            {
                CloneGateBost.Instance.SetRandomNumber(10);
                Time.timeScale = 1f;
                Clonerandom.SetActive(false);
            }
            else if (name == "40")
            {
                CloneGateBost.Instance.SetRandomNumber(40);
                Time.timeScale = 1f;
                Clonerandom.SetActive(false);
            }
            else if (name == "50")
            {
                CloneGateBost.Instance.SetRandomNumber(50);
                Time.timeScale = 1f;
                Clonerandom.SetActive(false);
            }
            else if (name == "80")
            {
                CloneGateBost.Instance.SetRandomNumber(80);
                Time.timeScale = 1f;
                Clonerandom.SetActive(false);
            }
            else
            {
                Debug.Log($"✅ Mũi tên chạm vào: {name}");
               
            }
        }

        if (results.Count == 0)
        {
            Debug.Log("❌ Mũi tên không chạm vào bất kỳ image nào.");
        }
    }
}
