using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [Header("Nguồn phát âm thanh")]
    public AudioSource audiobutton;
    public AudioClip button;
    public void Map()
    {
        StartCoroutine(PlaySoundAndLoadScene());
    }

    

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
    public void MapAttackMini()
    {
        SceneManager.LoadScene(2);
    }
    private IEnumerator PlaySoundAndLoadScene()
    {
        if (audiobutton != null && button != null)
        {
            audiobutton.PlayOneShot(button);
            yield return new WaitForSeconds(button.length);
        }

        SceneManager.LoadScene(1);
    }
    public void reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
