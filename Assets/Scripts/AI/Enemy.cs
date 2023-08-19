using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject agentPrefabs;
    //luu ragdoll, trang thai default gan truoc khi active

    private bool isActive = false;
    public bool IsActive => isActive;

    public void Active()
    {
        isActive = true;
        agentPrefabs.SetActive(isActive);
    }

    public void Deactive()
    {
        isActive = false;
        agentPrefabs.SetActive(isActive);
    }
}
