using UnityEngine;
using DG.Tweening;

public class UIDropMover : MonoBehaviour
{
    [Header("Tham chiếu RectTransform cần di chuyển")]
    public RectTransform uiElement;

    [Header("Vị trí đích (theo trục Y)")]
    public float targetY = 0f;

    [Header("Thời gian di chuyển")]
    public float duration = 1f;

    [Header("Kiểu easing")]
    public Ease easeType = Ease.OutBounce;

    void Start()
    {
        
    }

    public void MoveDown()
    {
        Vector2 targetPos = new Vector2(uiElement.anchoredPosition.x, targetY);
        uiElement.DOAnchorPos(targetPos, duration).SetEase(easeType);
    }
}
