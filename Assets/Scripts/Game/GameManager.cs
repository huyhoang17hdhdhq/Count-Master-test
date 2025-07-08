using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Reference")]
    public GameObject player;
    public GameObject road;
    public GameObject stickmanPrefab;

    private bool needToRestoreStickman = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (needToRestoreStickman && player != null)
        {
            SpawnStickman();
            needToRestoreStickman = false;
        }
    }

    public void OnButtonClick()
    {
        if (player == null || stickmanPrefab == null || road == null)
        {
            Debug.LogWarning("Player, Road hoặc StickmanPrefab chưa được gán!");
            return;
        }

        // Đặt lại vị trí player và road
        player.transform.position = new Vector3(0f, 12f, -7f);
        road.transform.position = new Vector3(0f, 11.72f, 37.5f);

        // Nếu Player chỉ có 1 con → spawn thêm stickman
        if (player.transform.childCount == 1)
        {
            SpawnStickman();
            needToRestoreStickman = true;
        }
    }

    private void SpawnStickman()
    {
        GameObject stickman = Instantiate(stickmanPrefab);

        stickman.transform.SetParent(player.transform);
        stickman.transform.localPosition = new Vector3(0f, -0.3f, 0f);
        stickman.transform.localScale = stickmanPrefab.transform.localScale;
        stickman.transform.SetSiblingIndex(2);
    }
}
