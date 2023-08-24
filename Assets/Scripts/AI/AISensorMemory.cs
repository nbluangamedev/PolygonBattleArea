using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AIMemory
{
    public float Age
    { get { return Time.time - lastSeen; } }

    public GameObject gameObject;
    public Vector3 position;
    public Vector3 direction;
    public float distance;
    public float angle;
    public float lastSeen;
    public float score;
}

public class AISensorMemory
{
    public List<AIMemory> memories = new();
    private GameObject[] characters;

    public AISensorMemory(int maxPlayers)
    {
        characters = new GameObject[maxPlayers];
    }

    public void UpdateSenses(AISensor sensor)
    {
        int targets = sensor.Filter(characters, "Character", "Player");
        for (int i = 0; i < targets; ++i)
        {
            GameObject target = characters[i];
            PushMemory(sensor.gameObject, target);
        }
    }

    public void PushMemory(GameObject agent, GameObject target)
    {
        AIMemory memory = FilterMemory(target);
        memory.gameObject = target;
        memory.position = target.transform.position;
        memory.direction = target.transform.position - agent.transform.position;
        memory.distance = memory.direction.magnitude;
        memory.angle = Vector3.Angle(agent.transform.forward, memory.direction);
        memory.lastSeen = Time.time;
    }

    public AIMemory FilterMemory(GameObject gameObject)
    {
        AIMemory memory = memories.Find(x => x.gameObject == gameObject);
        if (memory == null)
        {
            memory = new AIMemory();
            memories.Add(memory);
        }
        return memory;
    }

    public void ForgetMemories(float olderThan)
    {
        memories.RemoveAll(m => m.Age > olderThan);
        memories.RemoveAll(m => !m.gameObject);
        memories.RemoveAll(m => m.gameObject.GetComponent<Health>().IsDead());
    }
}