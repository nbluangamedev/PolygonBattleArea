using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPosition : MonoBehaviour
{
    private List<GameObject> characters;
    private int selectedCharacter;

    private void Awake()
    {
        if (GameManager.HasInstance)
        {
            selectedCharacter = GameManager.Instance.SelectedCharacter;
        }        
    }

    private void Start()
    {
        characters = new();
        foreach (Transform character in transform)
        {
            characters.Add(character.gameObject);
            character.gameObject.SetActive(false);
        }
        characters[selectedCharacter].SetActive(true);
    }
}