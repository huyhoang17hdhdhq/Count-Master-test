using System.Collections.Generic;
using UnityEngine;


public class StickManMaterial : MonoBehaviour
{
    [Header("Danh sách materials giống như bên ImageSpinAndStop")]
    public List<Material> materials;

    private int lastSavedIndex = -1;
    private const string MATERIAL_INDEX_KEY = "SavedMaterialIndex";

    private SkinnedMeshRenderer skinnedRenderer;

    void Start()
    {
        skinnedRenderer = GetComponentInChildren<SkinnedMeshRenderer>(); // Tìm trong con nếu không gắn trực tiếp
        UpdateMaterial(); // Áp material lần đầu
    }

    void Update()
    {
        int currentIndex = PlayerPrefs.GetInt(MATERIAL_INDEX_KEY, -1);

        if (currentIndex != lastSavedIndex)
        {
            UpdateMaterial();
        }
    }

    void UpdateMaterial()
    {
        int index = PlayerPrefs.GetInt(MATERIAL_INDEX_KEY, -1);

        if (index >= 0 && index < materials.Count && skinnedRenderer != null)
        {
            skinnedRenderer.material = materials[index];
            lastSavedIndex = index;
            Debug.Log($"[Stickman] Gán material mới (SkinnedMesh) - index: {index}");
        }
        else
        {
            Debug.LogWarning("[Stickman] Material index không hợp lệ hoặc SkinnedMeshRenderer chưa được gắn!");
        }
    }
}
