using UnityEngine;

public class Branch : MonoBehaviour
{
    [SerializeField] GameObject branchStart;

    public Vector3 GetDirectionTo(Vector3 targetPos)
    {
        return (targetPos - branchStart.transform.position).normalized;
    }

    public float GetDistanceTo(Vector3 targetPos)
    {
        return (targetPos - branchStart.transform.position).magnitude;
    }
}
