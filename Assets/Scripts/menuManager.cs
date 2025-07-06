using UnityEngine;
using UnityEngine.Timeline;

public class menuManager : MonoBehaviour
{
    [SerializeField] private GameObject startMenuObj;
    public GameObject Setting;
    public GameObject Skin;
    [Header("Nguồn phát âm thanh")]
    public AudioSource audiobutton;
    public AudioClip button;
    public AudioClip audioSetting;

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

}
