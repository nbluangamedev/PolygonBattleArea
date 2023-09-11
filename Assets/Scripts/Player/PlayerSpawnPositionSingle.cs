using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPositionSingle : MonoBehaviour
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

        //3d sound
        characters[selectedCharacter].AddComponent<AudioSource>();
        characters[selectedCharacter].GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SE_VOLUME_KEY", 0.3f);
        characters[selectedCharacter].AddComponent<GetAudioManager>();
    }
}