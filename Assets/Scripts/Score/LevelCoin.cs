using UnityEngine;
using TMPro;

public class LevelCoin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI LevelcoinTxt;
    public static LevelCoin Instance;

    [SerializeField] private int level = 1;
    public int bonusCoinThisLevel = 0;
   

    private const string LEVEL_KEY = "LevelCoin_Level";

    private void Awake()
    {
        Instance = this;
        level = PlayerPrefs.GetInt(LEVEL_KEY, 1);
    }

    public void IncreaseLevel()
    {
        level++;
        PlayerPrefs.SetInt(LEVEL_KEY, level);
        PlayerPrefs.Save();
    }

    public void CalculateBonus(int finalCoin)
    {
        float bonus = finalCoin * 0.1f * level;
        bonusCoinThisLevel = Mathf.RoundToInt(bonus);

        Debug.Log($"[LevelCoin] Tính bonus từ finalCoin: {finalCoin} x 0.1 x {level} = {bonusCoinThisLevel}");
    }


    public int GetBonusCoin()
    {
        return bonusCoinThisLevel;
    }
    private void Update()
    {
        LevelcoinTxt.text = level.ToString();
    }
}
