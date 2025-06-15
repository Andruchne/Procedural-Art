using UnityEngine;

public class Watermill : MonoBehaviour
{
    [SerializeField] float watermillSpeed = 25;

    void Update()
    {
        RotateMill();
    }

    void RotateMill()
    {
        transform.Rotate(Vector3.right * -watermillSpeed * Time.deltaTime);
    }
}
