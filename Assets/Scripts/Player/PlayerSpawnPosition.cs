using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPosition : MonoBehaviour
{
    public List<GameObject> characterPrefabs;
    public List<Transform> spawnPositions;

    private int selectedCharacter;
    private int numberPosition;

    private void Awake()
    {
        if (GameManager.HasInstance)
        {
            selectedCharacter = GameManager.Instance.SelectedCharacter;
        }
        numberPosition = spawnPositions.Count;
    }

    private void Start()
    {
        //Debug.Log("selected play index: " + selectedCharacter);
        int positonSpawn = Mathf.RoundToInt(Random.Range(0, numberPosition - 1));
        switch (selectedCharacter)
        {
            case 0: Instantiate(characterPrefabs[0], spawnPositions[positonSpawn]); break;
            case 1: Instantiate(characterPrefabs[1], spawnPositions[positonSpawn]); break;
            case 2: Instantiate(characterPrefabs[2], spawnPositions[positonSpawn]); break;
            case 3: Instantiate(characterPrefabs[3], spawnPositions[positonSpawn]); break;
            default: Instantiate(characterPrefabs[0], spawnPositions[positonSpawn]); break;
        }
    }
}