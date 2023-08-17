using UnityEngine;

[ExecuteInEditMode]
public class AITargetingSystem : MonoBehaviour
{
    public float memorySpan = 3.0f;
    public float distanceWeight = 1.0f;
    public float angleWeight = 1.0f;
    public float ageWeight = 1.0f;

    private AISensorMemory memory = new(10);
    private AISensor sensor;
    private AIMemory bestMemory = null;

    public bool HasTarget
    { get { return (bestMemory != null); } }

    public GameObject Target
    {
        get
        {
            if (HasTarget)
            {
                return bestMemory.gameObject;
            }
            else { return null; }
        }
    }

    public Vector3 TargetPosition
    {
        get
        {
            if (HasTarget)
            {
                return bestMemory.gameObject.transform.position;
            }
            else { return Vector3.zero; }
        }
    }

    public bool TargetInSight
    {
        get
        {
            if (HasTarget)
            {
                return bestMemory.Age < 0.5f;       //seconds
            }
            else
            {
                return false;
            }
        }
    }

    public float TargetDistance
    {
        get
        {
            if (HasTarget)
            {
                return bestMemory.distance;
            }
            else
            {
                return Mathf.Infinity;
            }
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
        foreach (AIMemory memory in memory.memories)
        {
            if(memory.gameObject!=this.gameObject)
            {
                memory.score = CalculateScore(memory);
                if (bestMemory == null || memory.score > bestMemory.score)
                {
                    bestMemory = memory;
                }
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
        float score = distanceScore + angleScore + ageScore;
        return score;
    }

    private void OnDrawGizmos()
    {
        float maxScore = float.MinValue;
        foreach (AIMemory memory in memory.memories)
        {
            maxScore = Mathf.Max(maxScore, memory.score);
        }

        foreach (AIMemory memory in memory.memories)
        {
            Color color = Color.red;
            if (memory == bestMemory)
            {
                color = Color.yellow;
            }
            color.a = memory.score / maxScore;
            Gizmos.color = color;

            Gizmos.DrawSphere(memory.position, .2f);
        }
    }
}