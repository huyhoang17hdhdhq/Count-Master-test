using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class menuManager : MonoBehaviour
{
    [SerializeField] private GameObject startMenuObj;
    public GameObject Setting;
    public GameObject Skin;
    [Header("Nguồn phát âm thanh")]
    public AudioSource audiobutton;
    public AudioClip button;
    public AudioClip audioSetting;
    [Header("UI WWIN LOSE")]
    public GameObject WinStair;
    public GameObject WinBoss;
    public GameObject lose;

    private bool winTriggered = false;
    private bool bossTriggered = false;
    private bool loseTriggered = false;
    

    public Image bossHpFill;

    private bool bossDeadHandled = false;

    public void Update()
    {
        {
            if (!winTriggered && PlayerManager.PlayerManagerInstance.transform.childCount == 1 && PlayerManager.PlayerManagerInstance.FinishLine && PlayerManager.PlayerManagerInstance.numberOfStickmans > 1)
            {
                winTriggered = true;
                StartCoroutine(CallAfterDelay(2f, Win));
            }
            if (!loseTriggered && PlayerManager.PlayerManagerInstance.numberOfStickmans < 1)
            {
                loseTriggered = true;
                StartCoroutine(CallAfterDelay(2f, Lose));
            }
            if (!bossTriggered && bossHpFill.fillAmount <= 0f && !bossDeadHandled)
            {
                bossDeadHandled = true;
                bossTriggered = true;
                StartCoroutine(CallAfterDelay(3f, Winboss));
                
            }

        }
    }
    public void StartTheGame()
    {
        startMenuObj.SetActive(false);
        PlayerManager.PlayerManagerInstance.gameState = true;

        PlayerManager.PlayerManagerInstance.player.GetChild(1).GetComponent<Animator>().SetBool("run", true);
    }
    public void SettingAll()
    {
        Setting.SetActive(true);
        if (audiobutton != null && button != null)
        {
            audiobutton.PlayOneShot(audioSetting);

        }

    }
    public void Pick()
    {
        Skin.SetActive(true);
        startMenuObj.SetActive(false);
        audiobutton.PlayOneShot(button);
    }
    public void ExitPick()
    {
        Skin.SetActive(false);
        startMenuObj.SetActive(true);
    }
    public void close()
    {
        Setting.SetActive(false );
        if (audiobutton != null && button != null)
        {
            audiobutton.PlayOneShot(button);

        }
       
    }
    public void Win()
    {
        WinStair.SetActive(true);
    }
    public void Winboss()
    {
        WinBoss.SetActive(true);
    }
    public void Lose()
    {
        lose.SetActive(true);
    }
    private IEnumerator CallAfterDelay(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }


}
