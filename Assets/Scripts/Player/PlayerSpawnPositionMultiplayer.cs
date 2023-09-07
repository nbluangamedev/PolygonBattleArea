using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPositionMultiplayer : MonoBehaviour
{
    public List<GameObject> characterPrefabs;
    public List<Transform> spawnPositions;
    public GameObject mainCameraPrefabs;
    public WeaponAnimationEvent animationEvents;

    private int selectedCharacter;
    private int numberCharacter;
    private int numberPosition;
    private GameObject player;


    private void Awake()
    {
        //animationEvents.weaponAnimationEvent.AddListener(OnAnimationMoveEvent);

        numberPosition = spawnPositions.Count;
        numberCharacter = characterPrefabs.Count;
    }

    private void Start()
    {
        int positonSpawn = Mathf.RoundToInt(Random.Range(0, numberPosition - 1));
        selectedCharacter = Mathf.RoundToInt(Random.Range(0, numberCharacter - 1));

        switch (selectedCharacter)
        {
            case 0:
                GameObject mainCamera = Instantiate(mainCameraPrefabs);
                player = Instantiate(characterPrefabs[0], spawnPositions[positonSpawn]);
                player.GetComponent<CharacterLocomotion>().animationEvents = animationEvents;
                player.GetComponent<ActiveWeapon>().crossHairTarget = mainCamera.transform.Find("CrossHairTarget");
                break;

            case 1: Instantiate(characterPrefabs[1], spawnPositions[positonSpawn]); break;
            case 2: Instantiate(characterPrefabs[2], spawnPositions[positonSpawn]); break;
            case 3: Instantiate(characterPrefabs[3], spawnPositions[positonSpawn]); break;
        }
    }

    //private void OnAnimationMoveEvent(string eventName)
    //{
    //    switch (eventName)
    //    {
    //        case "Crouch":
    //            CrouchSound();
    //            break;
    //        case "Sprinting":
    //            SprintSound();
    //            break;
    //        case "Locomotion":
    //            LocomotionSound();
    //            break;
    //        case "Jump":
    //            JumpSound();
    //            break;
    //        case "Landing":
    //            LandingSound();
    //            break;
    //    }
    //}

    //private void CrouchSound()
    //{
    //    if (AudioManager.HasInstance)
    //    {
    //        AudioManager.Instance.PlaySE(AUDIO.SE_PL_STEP2);
    //    }
    //}

    //private void SprintSound()
    //{
    //    if (AudioManager.HasInstance)
    //    {
    //        AudioManager.Instance.PlaySE(AUDIO.SE_PL_STEP4);
    //    }
    //}

    //private void LocomotionSound()
    //{
    //    if (AudioManager.HasInstance)
    //    {
    //        AudioManager.Instance.PlaySE(AUDIO.SE_PL_STEP1);
    //    }
    //}

    //private void JumpSound()
    //{
    //    if (AudioManager.HasInstance)
    //    {
    //        AudioManager.Instance.PlaySE(AUDIO.SE_PL_JUMP1);
    //        //Debug.Log("jump");
    //    }
    //}

    //private void LandingSound()
    //{
    //    if (AudioManager.HasInstance)
    //    {
    //        AudioManager.Instance.PlaySE(AUDIO.SE_LANDING);
    //        //Debug.Log("landing");
    //    }
    //}
}