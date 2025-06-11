using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSpinAndStop : MonoBehaviour
{
    public Image spinningImage;                   // Hình quay
    public Renderer playerRenderer;               // Renderer của player
    public List<Material> materials;              // 9 material tương ứng với vùng

    const string MATERIAL_INDEX_KEY = "SavedMaterialIndex";

    void Start()
    {
        // Gán lại material đã lưu nếu có
        if (PlayerPrefs.HasKey(MATERIAL_INDEX_KEY))
        {
            int savedIndex = PlayerPrefs.GetInt(MATERIAL_INDEX_KEY);
            if (savedIndex >= 0 && savedIndex < materials.Count)
            {
                playerRenderer.material = materials[savedIndex];
                Debug.Log($"[DEBUG] Gán lại material đã lưu: Index {savedIndex}");
            }
        }
    }

    void ApplyMaterialAtArrow()
    {
        float zRotation = spinningImage.rectTransform.eulerAngles.z;
        float correctedAngle = (360f - zRotation) % 360f;

        string regionName = "Không xác định";
        int materialIndex = -1;

        // Dựa vào góc corrected xác định vùng màu
        if (correctedAngle >= 0f && correctedAngle <= 36f)
        {
            materialIndex = 0; regionName = "Xanh lá cây";
        }
        else if (correctedAngle > 36f && correctedAngle <= 73f)
        {
            materialIndex = 1; regionName = "Xanh lam";
        }
        else if (correctedAngle > 73f && correctedAngle <= 110f)
        {
            materialIndex = 2; regionName = "Xám";
        }
        else if (correctedAngle > 110f && correctedAngle <= 147f)
        {
            materialIndex = 3; regionName = "Cam";
        }
        else if (correctedAngle > 147f && correctedAngle <= 184f)
        {
            materialIndex = 4; regionName = "Đỏ";
        }
        else if (correctedAngle > 184f && correctedAngle <= 216f)
        {
            materialIndex = 5; regionName = "Xanh nhạt";
        }
        else if (correctedAngle > 216f && correctedAngle <= 242f)
        {
            materialIndex = 6; regionName = "Đen";
        }
        else if (correctedAngle > 242f && correctedAngle <= 278f)
        {
            materialIndex = 7; regionName = "Vàng";
        }
        else if (correctedAngle > 278f && correctedAngle <= 314f)
        {
            materialIndex = 8; regionName = "Tím";
        }
        else
        {
            materialIndex = 0; regionName = "Xanh lá cây";
        }

        // DEBUG
        Debug.Log($"[DEBUG] Z: {zRotation:F2}° | Corrected: {correctedAngle:F2}°");
        Debug.Log($"[DEBUG] Vùng màu: {regionName} (material index: {materialIndex})");

        // Áp dụng material
        if (materialIndex >= 0 && materialIndex < materials.Count)
        {
            playerRenderer.material = materials[materialIndex];
            PlayerPrefs.SetInt(MATERIAL_INDEX_KEY, materialIndex); // Lưu lại chỉ số material
            PlayerPrefs.Save(); // Đảm bảo lưu
        }
        else
        {
            Debug.LogWarning("Material index nằm ngoài danh sách!");
        }
    }

    IEnumerator SpinWheel(float duration)
    {
        float time = 0f;
        float totalRotation = Random.Range(360f, 720f);
        float startZ = spinningImage.rectTransform.eulerAngles.z;
        float endZ = startZ + totalRotation;

        while (time < duration)
        {
            float t = time / duration;
            float easedT = 1f - Mathf.Pow(1f - t, 3f); // Ease out cubic
            float currentZ = Mathf.Lerp(startZ, endZ, easedT);
            spinningImage.rectTransform.eulerAngles = new Vector3(0, 0, currentZ);

            time += Time.deltaTime;
            yield return null;
        }

        spinningImage.rectTransform.eulerAngles = new Vector3(0, 0, endZ % 360f);
        ApplyMaterialAtArrow();
    }

    public void OnSpinButtonClicked()
    {
        StartCoroutine(SpinWheel(4f));
    }
}
