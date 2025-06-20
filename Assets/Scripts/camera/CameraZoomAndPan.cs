using UnityEngine;
using Cinemachine;

public class CinemachineZoomAndPan : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float zoomStep = 5f;
    public float minFOV = 15f;
    public float maxFOV = 60f;
    public float rotateStep = 4f;
    public float dragThreshold = 20f;

    private float defaultFOV;
    private Vector2 lastMousePos;
    private bool isDragging = false;

    void Start()
    {
        if (virtualCamera == null)
        {
            Debug.LogError("Chưa gán virtualCamera!");
            return;
        }

        defaultFOV = virtualCamera.m_Lens.FieldOfView;
    }

    void Update()
    {
        if (virtualCamera == null) return;

        HandleZoom();
        HandlePan();
    }

    void HandleZoom()
    {
        float fov = virtualCamera.m_Lens.FieldOfView;

        // Zoom bằng chuột
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            fov -= scroll > 0 ? zoomStep : -zoomStep;
            virtualCamera.m_Lens.FieldOfView = Mathf.Clamp(fov, minFOV, maxFOV);
        }

        // Zoom bằng 2 ngón tay
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            Vector2 prevPos0 = t0.position - t0.deltaPosition;
            Vector2 prevPos1 = t1.position - t1.deltaPosition;

            float prevDistance = Vector2.Distance(prevPos0, prevPos1);
            float currDistance = Vector2.Distance(t0.position, t1.position);
            float delta = currDistance - prevDistance;

            if (Mathf.Abs(delta) > 1f)
            {
                fov -= delta > 0 ? zoomStep : -zoomStep;
                virtualCamera.m_Lens.FieldOfView = Mathf.Clamp(fov, minFOV, maxFOV);
            }
        }
    }

    void HandlePan()
    {
        if (virtualCamera.m_Lens.FieldOfView >= defaultFOV - 3f)
            return;

        // Kéo chuột trái
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastMousePos;
            lastMousePos = Input.mousePosition;

            if (Mathf.Abs(delta.x) > dragThreshold)
            {
                float rotY = delta.x > 0 ? rotateStep : -rotateStep;
                transform.Rotate(Vector3.up, rotY, Space.World);
            }
        }

        // Kéo 1 ngón tay (touch)
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Moved)
            {
                if (Mathf.Abs(t.deltaPosition.x) > dragThreshold)
                {
                    float rotY = t.deltaPosition.x > 0 ? rotateStep : -rotateStep;
                    transform.Rotate(Vector3.up, rotY, Space.World);
                }
            }
        }
    }
}
