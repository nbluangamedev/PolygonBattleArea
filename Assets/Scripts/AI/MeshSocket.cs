using UnityEngine;

public class MeshSocket : MonoBehaviour
{
    public SocketID socketID;
    public HumanBodyBones bone;
    public Vector3 offset;
    public Vector3 rotation;

    Transform attachPoint;

    void Start()
    {
        Animator animator=GetComponentInParent<Animator>();
        attachPoint = new GameObject("socket" + socketID).transform;
        attachPoint.SetParent(animator.GetBoneTransform(bone));
        attachPoint.localPosition = offset;
        attachPoint.localRotation = Quaternion.Euler(rotation);
    }

    public void Attach(Transform objectTransform)
    {
        objectTransform.SetParent(attachPoint, false);
    }
}