using UnityEngine;
using DG.Tweening;

public class PingPongMover : MonoBehaviour
{
    [Header("Hai điểm di chuyển qua lại")]
    public Transform pointA;
    public Transform pointB;

    [Header("Thời gian di chuyển giữa 2 điểm")]
    public float moveDuration = 2f;

    [Header("Tốc độ xoay 360 độ (giây)")]
    public float rotationDuration = 1f;

    private void Start()
    {
        StartPingPong();
        StartRotation();
    }

    void StartPingPong()
    {
        transform.DOMove(pointB.position, moveDuration)
                 .SetEase(Ease.InOutSine)
                 .OnComplete(() => MoveBack());
    }

    void MoveBack()
    {
        transform.DOMove(pointA.position, moveDuration)
                 .SetEase(Ease.InOutSine)
                 .OnComplete(() => StartPingPong());
    }

    void StartRotation()
    {
        transform.DORotate(new Vector3(0, 0, 360), rotationDuration, RotateMode.FastBeyond360)
                 .SetEase(Ease.Linear)
                 .SetLoops(-1); // 🔁 Lặp vô hạn
    }
}
