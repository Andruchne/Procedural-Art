using UnityEngine;

public class Stairs : MonoBehaviour
{
    [SerializeField] PillarBlock upperPillar;

    // It's not quite correctly rotated, so we're deactivating opposites
    public void DeactivateRightPillar()
    {
        upperPillar.DeactivateLeftPillar();
    }

    public void DeactivateLeftPillar()
    {
        upperPillar.DeactivateRightPillar();
    }
}
