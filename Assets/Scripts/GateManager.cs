using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GateManager : MonoBehaviour
{
    public TextMeshPro GateNo;
    public int randomNumber;
    public bool multiply;

    void Start()
    {
        if (multiply)
        {
            randomNumber = Random.Range(1, 3);
            GateNo.text = "X" + randomNumber;
        }
        else
        {
            // 👉 Chỉ chọn ngẫu nhiên 1 trong 3 số: 20, 14, 18
            int[] allowedValues = { 20, 14, 18 };
            randomNumber = allowedValues[Random.Range(0, allowedValues.Length)];
            GateNo.text = randomNumber.ToString();
        }
    }
}
