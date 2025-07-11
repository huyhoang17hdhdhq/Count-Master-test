using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinADS : MonoBehaviour
{
    public void clickcoinADS()
    {
        CoinManager.Instance.AddCoin(500);
    }
}
