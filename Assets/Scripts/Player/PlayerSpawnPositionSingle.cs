using System.Collections.Generic;
using Unity.VisualScripting;
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
        characters[selectedCharacter].AddComponent<AudioListener>();
        characters[selectedCharacter].AddComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SE_VOLUME_KEY", 0.3f);
        characters[selectedCharacter].AddComponent<GetAudioManager>();
    }

    private void Update()
    {
        if (!characters[selectedCharacter])
        {
            this.AddComponent<AudioListener>();
        }
    }
}
