using UnityEngine;

public class CloneGateBost : MonoBehaviour
{
    public static CloneGateBost Instance;

    public int randomGateBost;

    private void Awake()
    {
        Instance = this;
    }

    public void SetRandomNumber(int value)
    {
        randomGateBost = value;
        Debug.Log($"[CloneGateBost] Đã nhận randomNumber = {randomGateBost}");
    }
}
