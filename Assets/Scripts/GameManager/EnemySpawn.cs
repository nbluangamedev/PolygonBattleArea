using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public float timeToSpawn;
    public float numberEnemySpawn;
    public List<GameObject> agentPrefabs = new();
    public List<Transform> spawnPositions = new();

    private int agentSlot;
    private int positonSpawn;
    private int numberEnemy;
    private int numberAgent;
    private int numberPosition;
    private float timer = 0;

    private void Start()
    {
        numberAgent = agentPrefabs.Count;
        numberPosition = spawnPositions.Count;
        numberEnemy = 0;
    }

    private void Update()
    {
        agentSlot = Mathf.RoundToInt(Random.Range(0, numberAgent - 1));
        positonSpawn = Mathf.RoundToInt(Random.Range(0, numberPosition - 1));
        timer += Time.deltaTime;
        if (timer > timeToSpawn && numberEnemy < numberEnemySpawn)
        {
            GameObject agent = Instantiate(agentPrefabs[agentSlot], spawnPositions[positonSpawn].position, Quaternion.identity);
            
            //3d sound
            AudioSource agentAudio = agent.AddComponent<AudioSource>();
            agentAudio.spatialBlend = 1f;
            agentAudio.dopplerLevel = 0f;
            agentAudio.rolloffMode = AudioRolloffMode.Linear;
            agentAudio.minDistance = 1f;
            agentAudio.maxDistance = 30f;
            agent.AddComponent<GetAudioSource>();

            numberEnemy++;
            timer = 0;
        }
    }
}
