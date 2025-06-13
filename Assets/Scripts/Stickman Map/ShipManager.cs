using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;


public class ShipManager : MonoBehaviour
{
    [Header("Các thanh fill điểm (tối đa 10)")]
    public Image fillBar1;
    public Image fillBar2;


    [Header("Hiển thị khi đầy điểm")]
    public GameObject imageAttack;
    public GameObject imageAttackship;

    [Header("Animator sẽ chạy Depart")]
    public Animator shipAnimator;

    public AudioSource Attack;
    public AudioSource Attackship;

    private int score = 0;
    private int maxScore = 10;
    public int MaxScore => maxScore;


    public float delay = 2f;


    private void Start()
    {
        // Thêm sự kiện click nếu có Button component
        if (imageAttack != null)
        {
            Button button = imageAttack.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(OnAttackClick);
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
            Attack.Play();
        }
        PlayerPrefs.SetInt("ShipScore", score);
        PlayerPrefs.Save();
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

        StartCoroutine(LoadSceneAfterDelay());



    }
    IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(2); 
    }
}
