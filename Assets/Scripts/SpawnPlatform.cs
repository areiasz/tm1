using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SpawnPlatform : MonoBehaviour
{

    public List<GameObject> platforms = new List<GameObject>();
    public List<Transform> currentPlatforms = new List<Transform>();
    public GameObject speedBoostPrefabs;
    public float speedBoostChance = 0.25f;

    public int offset;

    private Transform player;
    private Transform currentPlatformPoint;
    private int platformIndex;

    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < platforms.Count; i++)
        {
            Transform p = Instantiate(platforms[i], new Vector3(0, 0, i * 86), transform.rotation).transform;
            currentPlatforms.Add(p);
            offset += 86;
        }

        currentPlatformPoint = currentPlatforms[platformIndex].GetComponent<Platform>().point;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = player.position.z - currentPlatformPoint.position.z;

        if (distance >= 5)
        {
            Recycle(currentPlatforms[platformIndex].gameObject);
            platformIndex++;

            if (platformIndex > currentPlatforms.Count - 1)
            {
                platformIndex = 0;
            }

            currentPlatformPoint = currentPlatforms[platformIndex].GetComponent<Platform>().point;

            if(Random.value  < speedBoostChance)
            {
                Debug.Log("Spawnou???");
                SpawnSpeedBoost(currentPlatforms[platformIndex].gameObject);
            }
        }
    }
    void SpawnSpeedBoost(GameObject platform)
    {
        Transform[] spawnLocations = speedBoostPrefabs.GetComponentsInChildren<Transform>();

        int randomIndex = Random.Range(1, spawnLocations.Length);
        Vector3 spawnPosition = spawnLocations[randomIndex].position;

        Debug.Log("Vamos ver");
        Instantiate(speedBoostPrefabs, spawnPosition, Quaternion.identity, platform.transform);
    }


    public void Recycle(GameObject platform)
    {
        platform.transform.position = new Vector3(0, 0, offset);
        offset += 86;
    }

}
