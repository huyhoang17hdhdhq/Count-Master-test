using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickManRunAttack : MonoBehaviour
{
    public float speed = 3f;
    private Queue<Vector3> movePoints = new Queue<Vector3>();
    private bool isMoving = false;

    public void SetTargetPoints(List<Transform> points)
    {
        movePoints.Clear();
        foreach (Transform point in points)
        {
            movePoints.Enqueue(point.position);
        }

        if (movePoints.Count > 0)
        {
            StartCoroutine(MoveToPoints());
        }
        

    }

    private IEnumerator MoveToPoints()
    {
        isMoving = true;

        while (movePoints.Count > 0)
        {
            Vector3 target = movePoints.Dequeue();
            while (Vector3.Distance(transform.position, target) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                yield return null;
            }
        }

        isMoving = false;
    }
}
