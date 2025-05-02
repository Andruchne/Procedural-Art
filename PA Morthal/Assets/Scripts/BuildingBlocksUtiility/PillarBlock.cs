using UnityEngine;

/// <summary>
/// This let's you configure the type of pillar it is supposed to be, as well as help with transitions between other pillars
/// It is triggered from within the rule script, of whichever building needs it
/// </summary>

public class PillarBlock : MonoBehaviour
{
    [SerializeField] GameObject bigLeftPillar;
    [SerializeField] GameObject bigRightPillar;

    [SerializeField] GameObject smallLeftPillar;
    [SerializeField] GameObject smallRightPillar;

    [SerializeField] GameObject fence;
    [SerializeField] GameObject rope;

    // We only need to keep track of the current left pillar, to avoid duplicates with neighbouring pillars
    GameObject currentLeftPillar;
    GameObject currentFence;

    private void Start()
    {
        SetPillarType(false, false);
    }

    public void SetFence(bool state, bool isFence)
    {
        if (state)
        {
            if (isFence)
            {
                fence.SetActive(true);
                currentFence = fence;
            }
            else
            {
                rope.SetActive(true);
                currentFence = rope;
            }
        }
        else if (currentFence != null)
        {
            currentFence.SetActive(state);
            currentFence = null;
        }
    }

    public void DeactivateLeftPillar()
    {
        currentLeftPillar.SetActive(false);
    }

    public void SetPillarType(bool leftIsSmall, bool rightIsSmall)
    {
        if (leftIsSmall) 
        {
            SetState(true, true);
            currentLeftPillar = smallLeftPillar;
        }
        else 
        {
            SetState(true, false);
            currentLeftPillar = bigLeftPillar;
        }

        if (rightIsSmall) { SetState(false, true); }
        else { SetState(false, false); }
    }

    private void SetState(bool left, bool activateSmall)
    {
        if (left)
        {
            smallLeftPillar.SetActive(activateSmall);
            bigLeftPillar.SetActive(!activateSmall);
        }
        else
        {
            smallRightPillar.SetActive(activateSmall);
            bigRightPillar.SetActive(!activateSmall);
        }
    }
}
