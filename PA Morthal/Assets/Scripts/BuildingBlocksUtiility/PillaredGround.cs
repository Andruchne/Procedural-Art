using UnityEngine;

/// <summary>
/// This script makes it easier, to turn cornered blocks into normal ones, and vise versa
/// Deactivating the left pillar is made possible, to avoid having duplicates, if a neighbouring block of the same type is placed
/// </summary>

public class PillaredGround : MonoBehaviour
{
    [SerializeField] GameObject leftPillar;
    [SerializeField] GameObject rightPillar;
    [SerializeField] GameObject backPillar;
    [SerializeField] GameObject upperBackPillar;
    [SerializeField] GameObject upperFrontPillar;

    public void SetCorner(bool upperState, bool standState)
    {
        upperBackPillar.SetActive(upperState);
        backPillar.SetActive(standState);
    }

    public void DeactivateLeftPillar()
    {
        leftPillar.SetActive(false);
    }

    public void DeactivateRightPillar()
    {
        rightPillar.SetActive(false);
    }

    public void DeactivateBackPillar()
    {
        backPillar.SetActive(false);
    }

    public void DeactivateUpperPillars()
    {
        upperFrontPillar.SetActive(false);
        upperBackPillar.SetActive(false);
    }
}
