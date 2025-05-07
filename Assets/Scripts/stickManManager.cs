using System;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class stickManManager : MonoBehaviour
{
   // [SerializeField] private ParticleSystem blood;
    private Animator StickManAnimator;
    


    private void Start()
    {
        StickManAnimator = GetComponent<Animator>();
        StickManAnimator.SetBool("run", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("red"))
        {
            // Lấy số lượng stickman của bên xanh (this) và bên đỏ (other)
            int greenCount = transform.parent != null ? transform.parent.childCount : 1;
            int redCount = other.transform.parent != null ? other.transform.parent.childCount : 1;

            // So sánh số lượng
            if (greenCount > redCount)
            {
                // Bên xanh lớn hơn: phá hủy toàn bộ bên đỏ, phá hủy số lượng tương ứng bên xanh
                DestroyStickmans(other.transform.parent, redCount); // Phá hủy toàn bộ bên đỏ
                DestroyStickmans(transform.parent, redCount); // Phá hủy số lượng tương ứng bên xanh
                //Instantiate(blood, other.transform.position, Quaternion.identity); // Hiệu ứng máu
            }
            else if (redCount > greenCount)
            {
                // Bên đỏ lớn hơn: phá hủy toàn bộ bên xanh, phá hủy số lượng tương ứng bên đỏ
                DestroyStickmans(transform.parent, greenCount); // Phá hủy toàn bộ bên xanh
                DestroyStickmans(other.transform.parent, greenCount); // Phá hủy số lượng tương ứng bên đỏ
               // Instantiate(blood, transform.position, Quaternion.identity); // Hiệu ứng máu
            }
            else
            {
                // Bằng nhau: phá hủy cả hai bên
                DestroyStickmans(other.transform.parent, redCount);
                DestroyStickmans(transform.parent, greenCount);
               // Instantiate(blood, transform.position, Quaternion.identity);
            }
        }

        // Phần còn lại của OnTriggerEnter giữ nguyên
        switch (other.tag)
        {
            case "jump":
                transform.DOJump(transform.position, 1f, 1, 1f).SetEase(Ease.Flash).OnComplete(PlayerManager.PlayerManagerInstance.FormatStickMan);
                break;
        }

        if (other.CompareTag("stair"))
        {
            transform.parent.parent = null; // for instance tower_0
            transform.parent = null; // stickman
            GetComponent<Rigidbody>().isKinematic = GetComponent<Collider>().isTrigger = false;
            StickManAnimator.SetBool("run", false);

            if (!PlayerManager.PlayerManagerInstance.moveTheCamera)
                PlayerManager.PlayerManagerInstance.moveTheCamera = true;

            if (PlayerManager.PlayerManagerInstance.player.transform.childCount == 2)
            {
                other.GetComponent<Renderer>().material.DOColor(new Color(0.4f, 0.98f, 0.65f), 0.5f).SetLoops(1000, LoopType.Yoyo)
                    .SetEase(Ease.Flash);
            }
        }
    }

    // Hàm phụ để phá hủy số lượng stickman cụ thể
    private void DestroyStickmans(Transform parent, int count)
    {
        if (parent == null) return;

        for (int i = 0; i < count && parent.childCount > 0; i++)
        {
            Destroy(parent.GetChild(0).gameObject); // Phá hủy stickman đầu tiên trong parent
        }

        // Nếu parent không còn stickman, phá hủy chính parent
        if (parent.childCount == 0)
        {
            Destroy(parent.gameObject);
        }
    }
}