using UnityEngine;

public class StairTrigger : MonoBehaviour
{
    public StairManager stairManager;
    public int stairIndex;

    private void OnTriggerEnter(Collider other)
    {
        stairManager.OnStickmanEnter(other.gameObject, stairIndex);
    }
}
