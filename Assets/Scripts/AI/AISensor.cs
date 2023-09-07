using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AISensor : MonoBehaviour
{
    public bool debugGizmos = false;
    public Color meshColor;

    public float distance = 10f;
    public float angle = 30f;
    public float height = 1.0f;
    public LayerMask sensorLayer;
    public LayerMask occlusionLayer;
    public int scanFrequency = 30;
    public bool isScanning = false;

    [SerializeField]
    private Collider[] colliders = new Collider[50];
    private Mesh mesh;
    private int count;
    private float scanInterval;
    private float scanTimer;

    [SerializeField]
    private List<GameObject> objects = new();
    public List<GameObject> Objects
    {
        get
        {
            objects.RemoveAll(obj => !obj);
            return objects;
        }
    }

    void Start()
    {
        scanInterval = 1.0f / scanFrequency;
    }

    void Update()
    {
        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
            isScanning = true;
        }
        else isScanning = false;
    }

    private void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, sensorLayer, QueryTriggerInteraction.Collide);

        objects.Clear();
        for (int i = 0; i < count; i++)
        {
            GameObject obj = colliders[i].gameObject;
            if (IsInSight(obj))
            {
                objects.Add(obj);
            }
        }
    }

    public bool IsInSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 destination = obj.transform.position;
        Vector3 direction = destination - origin;

        if (direction.y < -0.1f || direction.y > height)
        {
            return false;
        }

        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if (deltaAngle > angle)
        {
            return false;
        }

        origin.y += height / 2;
        destination.y = origin.y;
        if (Physics.Linecast(origin, destination, occlusionLayer))
        {
            return false;
        }

        return true;
    }

    private Mesh CreateMesh()
    {
        Mesh mesh = new();
        int segment = 10;
        int numTriangle = (segment * 4) + 2 + 2;    //each segments has 4 verices and 2 up and 2 downside
        int numVertices = numTriangle * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero - Vector3.up * height;
        Vector3 bottomLeft = (Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance) - Vector3.up * height;
        Vector3 bottomRight = (Quaternion.Euler(0, angle, 0) * Vector3.forward * distance) - Vector3.up * height;

        Vector3 topCenter = bottomCenter + 2 * height * Vector3.up;
        Vector3 topRight = bottomRight + 2 * height * Vector3.up;
        Vector3 topLeft = bottomLeft + 2 * height * Vector3.up;

        int vert = 0;

        //left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        //right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segment;
        for (int i = 0; i < segment; i++)
        {
            bottomLeft = (Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance) - Vector3.up * height;
            bottomRight = (Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance) - Vector3.up * height;

            topRight = bottomRight + 2 * height * Vector3.up;
            topLeft = bottomLeft + 2 * height * Vector3.up;

            //far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            //top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            //bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;
        }

        for (int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreateMesh();
        scanInterval = 1.0f / scanFrequency;
    }

    private void OnDrawGizmos()
    {
        if (debugGizmos)
        {
            if (mesh)
            {
                Gizmos.color = meshColor;
                Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
            }

            Gizmos.color = Color.green;
            foreach (var obj in Objects)
            {
                Gizmos.DrawSphere(obj.transform.position, 0.2f);
            }
        }
    }

    public int Filter(GameObject[] buffer, string layerName, string tagName = null)
    {
        int layer = LayerMask.NameToLayer(layerName);
        int count = 0;
        foreach (var obj in Objects)
        {
            if (tagName != null && !obj.CompareTag(tagName))
            {
                continue;
            }

            if (obj.layer == layer)
            {
                buffer[count] = obj;
                ++count;
            }

            if (buffer.Length == count)
            {
                break;          //buffer is full
            }
        }
        return count;
    }
}