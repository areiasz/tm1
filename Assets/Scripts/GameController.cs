using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject GameOver;


    public float score;
    public int scoreCoin;
    public float highScore;
    public CoinCollector CoinCO;

    public Text scoreText;
    public Text scoreCoinText;
    public Text scoreRecord;

    private Player player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        // Carrega o maior score salvo
        highScore = PlayerPrefs.GetFloat("HighScore", 0);

        // Atualiza o texto com o maior score
        scoreRecord.text = "Best: " + Mathf.Round(highScore).ToString() + "m";

        CoinCO.checkCoins = 0;
        //CoinCO.checkCoins = PlayerPrefs.GetInt("coins", 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.isDead)
        score += Time.deltaTime * (5f + (player.speed / 20f));
        scoreText.text = "Score: " + Mathf.Round(score).ToString() + "m";
    }

    public void ShowGameOver()
    {
        GameOver.SetActive(true);
        CoinCO.totalCoins += CoinCO.checkCoins;
    }

    public void addCoin()
    {
        Debug.Log("Coin Collected");
        scoreCoin++;
        CoinCO.checkCoins++;
        Debug.Log(scoreCoin);
        //PlayerPrefs.SetInt("coins", scoreCoin);
        //PlayerPrefs.Save();
        scoreCoinText.text = scoreCoin.ToString();
    }

    public void UpdateHighScoreText(float newHighScore)
    {
        scoreRecord.text = "Best: " + Mathf.Round(newHighScore).ToString() + "m";
    }

    public void Exit()
    {
        CoinCO.checkCoins = 0;
        SceneManager.LoadSceneAsync(0);
    }

    public void Restart()
    {
        CoinCO.checkCoins = 0;
        SceneManager.LoadScene(1);
    }

}
