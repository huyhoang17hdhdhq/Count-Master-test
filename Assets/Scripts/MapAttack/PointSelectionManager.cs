using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointSelectionManager : MonoBehaviour
{
    public StickManRunAttack stickmanrunAttack;        // Gắn trong Inspector
    public Camera cam;                         // Main Camera
    public int maxPoints = 3;                  // Tối đa 3 point
    private List<Transform> selectedPoints = new List<Transform>();

    private bool isSwiping = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectedPoints.Clear();
            isSwiping = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isSwiping = false;
            if (selectedPoints.Count > 0)
            {
                stickmanrunAttack.SetTargetPoints(selectedPoints);
                Debug.Log("Gửi stickman đến " + selectedPoints.Count + " điểm.");
            }
        }

        if (isSwiping)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Point"))
                {
                    Transform point = hit.collider.transform;

                    if (!selectedPoints.Contains(point) && selectedPoints.Count < maxPoints)
                    {
                        selectedPoints.Add(point);
                        Debug.Log("Chọn điểm: " + point.name);
                    }
                }
            }
        }
    }
}

