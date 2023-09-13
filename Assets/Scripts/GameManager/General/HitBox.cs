using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Health health;
    public Rigidbody rb;

    public void OnHit(RaycastWeapon weapon, Vector3 direction)
    {
        health.TakeDamage(weapon.damage, direction, rb);
    }

    public void OnHitHead(Vector3 direction)
    {
        health.TakeDamage(100f, direction, rb);
        if(AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_HEADSHOT1);
        }
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.BroadCast(ListenType.ENEMY_HEADSHOT, 1);
        }
    }
} 