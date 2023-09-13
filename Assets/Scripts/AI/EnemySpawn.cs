using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public float timeToSpawn;
    public float numberEnemySpawn;
    public List<GameObject> agentPrefabs = new();
    public List<Transform> spawnPositions = new();
    [HideInInspector] public float timerSpawn = 0;
    
    private int agentSlot;
    private int positonSpawn;
    private int numberEnemy;
    private int numberAgent;
    private int numberPosition;
    
    
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
        timerSpawn += Time.deltaTime;

        if (timerSpawn > timeToSpawn && numberEnemy < numberEnemySpawn)
        {
            GameObject agent = Instantiate(agentPrefabs[agentSlot], spawnPositions[positonSpawn].position, Quaternion.identity);

            //3d sound
            AudioSource agentAudio = agent.AddComponent<AudioSource>();
            agentAudio.spatialBlend = 1f;
            agentAudio.reverbZoneMix = 0f;
            agentAudio.dopplerLevel = 0f;
            agentAudio.rolloffMode = AudioRolloffMode.Logarithmic;
            agentAudio.minDistance = 0f;
            agentAudio.maxDistance = 30f;
            agent.AddComponent<GetAudioSource>();

            numberEnemy++;
            timerSpawn = 0;
        }        
    }
}