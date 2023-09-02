using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characters;

    private int selectedCharacter = 0;
    private int check;
    private float speedRotateCharacter;

    private void Start()
    {
        if (DataManager.HasInstance)
        {
            speedRotateCharacter = DataManager.Instance.globalConfig.speedRotateCharacter;
        }

        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Register(ListenType.CHARACTER_SELECTION, UpdateSelection);
        }
    }

    private void OnDestroy()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Unregister(ListenType.CHARACTER_SELECTION, UpdateSelection);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            characters[selectedCharacter].transform.Rotate(new Vector3(0, -Input.GetAxis("Mouse X") * speedRotateCharacter, 0));
        }
    }

    private void UpdateSelection(object select)
    {
        if (select is int value)
        {
            //Debug.Log(value);
            check = value;
            if (check == 0)
            {
                characters[selectedCharacter].SetActive(false);
                selectedCharacter--;
                if (selectedCharacter < 0)
                {
                    selectedCharacter += characters.Length;
                }
                characters[selectedCharacter].SetActive(true);
                characters[selectedCharacter].transform.rotation = Quaternion.LookRotation(-Vector3.forward, Vector3.up);
                if (ListenerManager.HasInstance)
                {
                    ListenerManager.Instance.BroadCast(ListenType.SELECTED_CHARACTER, selectedCharacter);
                }
            }
            else
            {
                characters[selectedCharacter].SetActive(false);
                selectedCharacter = (selectedCharacter + 1) % characters.Length;
                characters[selectedCharacter].SetActive(true);
                characters[selectedCharacter].transform.rotation = Quaternion.LookRotation(-Vector3.forward,Vector3.up);
                if (ListenerManager.HasInstance)
                {
                    ListenerManager.Instance.BroadCast(ListenType.SELECTED_CHARACTER, selectedCharacter);
                }
            }
        }
    }    
}