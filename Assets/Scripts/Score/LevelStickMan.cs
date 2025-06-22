using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelStickMan : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI LevelTxt;
    private int currentTarget =2;
    public int numberLevel;

    private void Start()
    {
        numberLevel = PlayerPrefs.GetInt("NumberLevel", 0); // Nếu chưa có thì là 1
         PlayerManager.PlayerManagerInstance.MakeStickMan(numberLevel);
    }

    public void levelStickMan()
    {
        PlayerManager.PlayerManagerInstance.MakeStickMan(currentTarget);
        currentTarget++;
        numberLevel = PlayerManager.PlayerManagerInstance.numberOfStickmans;//// chua luu duoc so level
        PlayerPrefs.SetInt("NumberLevel",numberLevel);
        PlayerPrefs.Save();
       
    }

    private void Update()
    {
        
        LevelTxt.text = PlayerManager.PlayerManagerInstance.numberOfStickmans.ToString();
      
    }
}
