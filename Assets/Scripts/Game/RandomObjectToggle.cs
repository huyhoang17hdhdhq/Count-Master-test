using UnityEngine;

public class RandomObjectToggle : MonoBehaviour
{
    [Header("Tham chiếu 2 GameObject")]
    public GameObject object1;
    public GameObject object2;

    void Start()
    {
        ToggleRandomObject();
    }

    public void ToggleRandomObject()
    {
        int randomIndex = Random.Range(0, 2); // Trả về 0 hoặc 1

        if (randomIndex == 0)
        {
            if (object1 != null) object1.SetActive(true);
            if (object2 != null) object2.SetActive(false);
        }
        else
        {
            if (object1 != null) object1.SetActive(false);
            if (object2 != null) object2.SetActive(true);
        }
    }
}
