using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characters;
    public int selectedCharacter = 0;
    public TMP_Text label;
    private bool[] characterLocks;
    public GameObject Locker;
    public GameObject MenuTails;
    public GameObject MenuKnuck;
    public CoinCollector CoinCO;
    public GameObject Play;
    public GameObject NotEnough;
    public GameController gc;

    public void NextCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % characters.Length;
        characters[selectedCharacter].SetActive(true);

        label.text = characters[selectedCharacter].name;
        UpdateLockerCharacter();
    }

    public void PreviousCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if(selectedCharacter < 0)
        {
            selectedCharacter += characters.Length;
        }
        characters[selectedCharacter].SetActive(true);

        label.text = characters[selectedCharacter].name;
        UpdateLockerCharacter();
    }

    void Start()
    {
        characterLocks = new bool[characters.Length];
        LoadCharacterLocks();
        Debug.Log(characterLocks[0]);
        label.text = characters[selectedCharacter].name;
        UpdateLockerCharacter();
        //Debug.Log(characterLocks[0]);
    }

    void LoadCharacterLocks()
    {
        //Debug.Log(characterLocks[0]);
        for (int i = 0; i < characterLocks.Length; i++)
        {
            if(i == 0)
            {
                characterLocks[i] = false;
            }
            else
            {
                characterLocks[i] = PlayerPrefs.GetInt("CharacterLock_" + i, 1) == 1;
            }
        }
    }

    void SaveCharacterLocks()
    {
        for (int i = 0; i < characterLocks.Length; i++)
        {
            PlayerPrefs.SetInt("CharacterLock_" + i, characterLocks[i] ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    public void UpdateLockerCharacter()
    {

        if (characterLocks[selectedCharacter] == true)
        {
            Locker.SetActive(true);
            Play.SetActive(false);
        }
        else
        {
            Locker.SetActive(false);
            Play.SetActive(true);
        }
    }

    public void UnlockCharacter()
    {
        if (selectedCharacter == 1)
        {
            MenuTails.SetActive(true);
        }
        if (selectedCharacter == 2)
        {
            MenuKnuck.SetActive(true);
        }
    }

    void SummonNotEnough()
    {
        if(selectedCharacter == 1)
        {
            NotEnough.SetActive(false);
            MenuTails.SetActive(false);
        }

        if (selectedCharacter == 2)
        {
            NotEnough.SetActive(false);
            MenuKnuck.SetActive(false);
        }
    }
    public void Yes()
    {
        if (selectedCharacter == 1)
        {
            if (CoinCO.totalCoins >= 500)
            {
                CoinCO.totalCoins = CoinCO.totalCoins - 500;
                characterLocks[selectedCharacter] = false;
                Locker.SetActive(false);
                MenuTails.SetActive(false);
                UpdateLockerCharacter();
                SaveCharacterLocks();
            }
            else
            {
                NotEnough.SetActive(true);
                Invoke("SummonNotEnough", 3f);
                //NotEnough.SetActive(false);
                //MenuTails.SetActive(false);

            }
        }

        if (selectedCharacter == 2)
        {
            if (CoinCO.totalCoins >= 5000)
            {
                CoinCO.totalCoins = CoinCO.totalCoins - 5000;
                characterLocks[selectedCharacter] = false;
                Locker.SetActive(false);
                MenuKnuck.SetActive(false);
                UpdateLockerCharacter();
                SaveCharacterLocks();
            }
            else
            {
                NotEnough.SetActive(true);
                Invoke("SummonNotEnough", 3f);
                //NotEnough.SetActive(false);
                //MenuTails.SetActive(false);
            }
        }
    }

    public void No()
    {
        if (selectedCharacter == 1)
        {
            MenuTails.SetActive(false);
        }
        if (selectedCharacter == 2)
        {
            MenuKnuck.SetActive(false);
        }
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        //Debug.Log(selectedCharacter);
        //Debug.Log(characters[selectedCharacter].name);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void ResetProgress()
    {
        for (int i = 0; i < characterLocks.Length; i++)
        {
            if (i == 0)
            {
                characterLocks[i] = false; 
            }
            else
            {
                characterLocks[i] = true; 
            }
        }

        
        CoinCO.totalCoins = 0;

        
        gc.highScore = 0;
        PlayerPrefs.SetFloat("HighScore", gc.highScore);

        
        UpdateLockerCharacter();
    }

}
