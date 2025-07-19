using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class ShipManager : MonoBehaviour
{
    public static ShipManager Instance { get; private set; }

    [Header("Các thanh fill điểm (tối đa 10)")]
    public Image fillBar1;
    public Image fillBar2;

    [Header("Hiển thị khi đầy điểm")]
    public GameObject imageAttack;
    public GameObject imageAttackship;

    [Header("Animator sẽ chạy Depart")]
    public Animator shipAnimator;

    [Header("Nguồn phát âm thanh")]
    public AudioSource audiobutton;
    public AudioClip button;

    public AudioSource Attack;
    public AudioSource Attackship;

    private int score = 0;
    private int maxScore = 10;
    public int MaxScore => maxScore;

    public float delay = 2f;

    private void Awake()
    {
        // Gán instance singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // đảm bảo chỉ có 1 instance tồn tại
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // nếu bạn muốn giữ nó khi load scene
        }
    }

    private void Start()
    {
        // Thêm sự kiện click nếu có Button component
        if (imageAttack != null)
        {
            Button btn = imageAttack.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(OnAttackClick);
            }
        }

        score = PlayerPrefs.GetInt("ShipScore", 0);
        UpdateFillBars();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("blue"))
        {
            if (score < maxScore)
            {
                score++;
                UpdateFillBars();

                if (audiobutton != null && button != null)
                {
                    audiobutton.PlayOneShot(button);
                }
            }

            Destroy(other.gameObject);
        }
    }

    private void UpdateFillBars()
    {
        float fillAmount = (float)score / maxScore;

        if (fillBar1 != null)
            fillBar1.fillAmount = fillAmount;

        if (fillBar2 != null)
            fillBar2.fillAmount = fillAmount;

        // Nếu đầy thì bật imageAttack
        if (fillBar1 != null && fillBar1.fillAmount >= 1f && imageAttack != null && imageAttackship != null)
        {
            imageAttack.SetActive(true);
            imageAttackship.SetActive(true);
            if (Attack != null) Attack.Play();
        }

        PlayerPrefs.SetInt("ShipScore", score);
        PlayerPrefs.Save();
    }
    public bool IsFull()
    {
        return score >= maxScore;
    }


    private void OnAttackClick()
    {
        if (fillBar1 != null)
        {
            fillBar1.fillAmount = 0;
            fillBar1.gameObject.SetActive(false);
        }

        if (fillBar2 != null)
        {
            fillBar2.fillAmount = 0;
        }

        if (imageAttackship != null)
        {
            imageAttackship.SetActive(false);
        }

        if (Attackship != null)
        {
            Attackship.Play();
        }

        if (shipAnimator != null)
        {
            shipAnimator.SetTrigger("Depart");
        }

        // Reset score
        score = 0;
        PlayerPrefs.SetInt("ShipScore", score);
        PlayerPrefs.Save();

        StartCoroutine(LoadSceneAfterDelay());
    }

    IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(2);
    }
}
