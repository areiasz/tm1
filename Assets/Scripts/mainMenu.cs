using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public CoinCollector CoinCO;


    public void PlayMenu()
    {
        SceneManager.LoadSceneAsync(2);
        int coins = PlayerPrefs.GetInt("coins", 0);

    }

    public void ExitMenu()
    {
        Application.Quit();
    }

    public void ResetProgressMenu()
    {
        CharacterSelection characterSelection = FindObjectOfType<CharacterSelection>();
        PlayerPrefs.DeleteAll();
        CoinCO.totalCoins = 0;
        Debug.Log(characterSelection);
        characterSelection.ResetProgress();
        Debug.Log("WORKING");
    }

}
