using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // Đừng quên thêm namespace DOTween

public class ImageMover : MonoBehaviour
{
    public Image targetImage;   // Image mà bạn muốn di chuyển
    public float moveDuration = 2f; // Thời gian di chuyển
    public float targetXPosition = 500f; // Vị trí X đích đến

    private void Start()
    {
        // Kiểm tra xem Image có được gán hay chưa
        if (targetImage != null)
        {
            // Di chuyển Image theo trục X liên tục với kiểu lặp lại Yoyo
            MoveImageLoop(targetXPosition, moveDuration);
        }
        else
        {
            Debug.LogError("Target Image is not assigned!");
        }
    }

    // Hàm để di chuyển Image theo trục X liên tục (loop)
    public void MoveImageLoop(float targetX, float duration)
    {
        // Lấy vị trí hiện tại của Image
        Vector3 currentPos = targetImage.transform.position;

        // Di chuyển Image tới vị trí mới theo trục X với kiểu lặp lại Yoyo
        targetImage.transform.DOMoveX(targetX, duration)
            .SetLoops(-1, LoopType.Yoyo) // Lặp lại vô hạn và đi qua lại
            .SetEase(Ease.Linear); // Dùng hiệu ứng di chuyển đều
    }
}
