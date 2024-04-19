using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadCharacter : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public Transform spawnPoint;

    void Start()
    {
            int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
            GameObject prefab = characterPrefabs[selectedCharacter];
            GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            //Debug.Log(clone.name);

    }
}
