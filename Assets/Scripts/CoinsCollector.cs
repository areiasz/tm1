using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsCollector : MonoBehaviour
{
    public GameController gc;
    public int TotalCoins;


    public Text TotalCoinsText;

    //public void TotalCoinsCollecter()
    //{
    //    TotalCoins = gc.scoreCoin;
    //    TotalCoinsText.text = TotalCoins.ToString();
    //}
    // Start is called before the first frame update
    void Start()
    {
        TotalCoins = gc.scoreCoin;
        TotalCoinsText.text = TotalCoins.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
