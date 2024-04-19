using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalCoinsController : MonoBehaviour
{
    public CoinCollector CoinCO;

    public Text TotalCoinsText;
    // Start is called before the first frame update
    void Start()
    {
        TotalCoinsText.text = CoinCO.totalCoins.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
