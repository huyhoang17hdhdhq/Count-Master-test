using UnityEngine;

public class RandomObjectToggle : MonoBehaviour
{
    [Header("Tham chiếu 2 GameObject")]
    public GameObject object1;
    public GameObject object2;

    private const string ToggleKey = "LastToggleIndex"; // Key để lưu trong PlayerPrefs

    void Start()
    {
        ToggleAlternatingObject();
    }

    public void ToggleAlternatingObject()
    {
        int lastIndex = PlayerPrefs.GetInt(ToggleKey, 0); // Mặc định là 0 nếu chưa có

        bool isEven = lastIndex % 2 == 0;

        if (object1 != null) object1.SetActive(isEven);
        if (object2 != null) object2.SetActive(!isEven);

        // Tăng giá trị và lưu lại
        PlayerPrefs.SetInt(ToggleKey, lastIndex + 1);
        PlayerPrefs.Save(); // Lưu thật sự (optional nhưng nên có)
    }
}
