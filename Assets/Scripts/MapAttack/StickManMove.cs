using UnityEngine;
using DG.Tweening;

public class StickManMove : MonoBehaviour
{
    public float moveDuration = 1f;

    public void Move(Vector3 targetPos)
    {
        Debug.Log($"{name} moving to {targetPos}");
        transform.DOMove(targetPos, moveDuration).SetEase(Ease.Linear);
    }

}
