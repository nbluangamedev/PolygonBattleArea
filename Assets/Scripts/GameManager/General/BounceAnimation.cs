using UnityEngine;

public class BounceAnimation : MonoBehaviour
{
    public float bounceSpeed = 8f;
    public float bounceAmplitude = 0.05f;
    public float rotationSpeed = 90f;

    private float startingHeight;
    private float timeOffset;

    private void Start()
    {
        startingHeight = transform.localPosition.y;
        timeOffset = Random.value * Mathf.PI * 2;
    }

    private void Update()
    {
        //Bounce animation
        float finalHeight = startingHeight + Mathf.Sin(Time.time * bounceSpeed + timeOffset) * bounceAmplitude;
        var position = transform.localPosition;
        position.y = finalHeight;
        transform.localPosition = position;

        //Spin
        Vector3 rotation = transform.localRotation.eulerAngles;
        rotation.y += rotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
    }
}