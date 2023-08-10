using UnityEngine;

[ExecuteInEditMode]
public class AITargetingSystem : MonoBehaviour
{
    public float memorySpan = 3.0f;
    public float distanceWeight = 1.0f;
    public float angleWeight = 1.0f;
    public float ageWeight = 1.0f;

    private AISensorMemory memory = new AISensorMemory(10);
    private AISensor sensor;
    private AIMemory bestMemory;

    public bool HasTarget
    {
        get
        {
            return (bestMemory != null);
        }
    }

    public GameObject Target
    {
        get
        {
            return bestMemory.gameObject;
        }
    }

    public Vector3 TargetPosition
    {
        get
        {
            return bestMemory.gameObject.transform.position;
        }
    }

    public bool TargetInSight
    {
        get
        {
            return bestMemory.Age < 0.5f;       //seconds
        }
    }

    public float TargetDistance
    {
        get
        {
            return bestMemory.distance;
        }
    }


    private void Start()
    {
        sensor = GetComponent<AISensor>();
    }

    private void Update()
    {
        memory.UpdateSenses(sensor);
        memory.ForgetMemories(memorySpan);

        EvaluateScores();
    }

    private void EvaluateScores()
    {
        bestMemory = null;
        foreach (var memory in memory.memories)
        {
            memory.score = CalculateScore(memory);
            if (bestMemory == null || memory.score>bestMemory.score)
            {
                bestMemory = memory;
            }
        }
    }

    private float Normalize(float value, float maxValue)
    {
        return 1.0f - (value / maxValue);
    }

    private float CalculateScore(AIMemory memory)
    {
        float distanceScore = Normalize(memory.distance, sensor.distance) * distanceWeight;
        float angleScore = Normalize(memory.angle, sensor.angle) * angleWeight;
        float ageScore = Normalize(memory.Age, memorySpan) * ageWeight;
        return distanceScore + angleScore + ageScore;
    }

    private void OnDrawGizmos()
    {
        float maxScore = float.MinValue;
        foreach (var memory in memory.memories)
        {
            maxScore = Mathf.Max(maxScore, memory.score);
        }

        foreach (var memory in memory.memories)
        {
            Color color = Color.red;
            if(memory == bestMemory)
            {
                color = Color.yellow;
            }
            color.a = memory.score / maxScore;
            Gizmos.color = color;
            Gizmos.DrawSphere(memory.position, .2f);
        }
    }
}