using UnityEngine;

public class HierarchySwapper : MonoBehaviour
{
    public Transform childA;
    public Transform childB;

    public void Swap()
    {
        // Lưu lại cha, vị trí trên hierarchy và vị trí thế giới
        Transform parentA = childA.parent;
        int indexA = childA.GetSiblingIndex();
        Vector3 worldPosA = childA.position;
        Quaternion worldRotA = childA.rotation;

        Transform parentB = childB.parent;
        int indexB = childB.GetSiblingIndex();
        Vector3 worldPosB = childB.position;
        Quaternion worldRotB = childB.rotation;

        // Đổi cha
        childA.SetParent(parentB);
        childB.SetParent(parentA);

        // Đặt lại vị trí và rotation theo thế giới
        childA.position = worldPosB;
        childA.rotation = worldRotB;

        childB.position = worldPosA;
        childB.rotation = worldRotA;

        // Đổi vị trí trên Hierarchy
        childA.SetSiblingIndex(indexB);
        childB.SetSiblingIndex(indexA);
    }
}
