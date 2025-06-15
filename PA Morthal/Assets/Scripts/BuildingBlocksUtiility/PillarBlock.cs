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

    [SerializeField] GameObject plankFence;
    [SerializeField] GameObject ropeFence;

    GameObject currentFence;

    bool leftIsSmall;
    bool rightIsSmall;

    private void Start()
    {
        CheckState();
    }

    public bool IsLeftPillarSmall()
    {
        return leftIsSmall;
    }

    public bool IsRightPillarSmall()
    {
        return rightIsSmall;
    }

    // Determines, whether the fence is supposed to be with ropes or wooden planks.
    // Deactivates current fence, if parameter state is false
    public void SetFence(bool state, bool isPlank = true)
    {
        if (state)
        {
            if (currentFence != null) currentFence.SetActive(false);

            if (isPlank)
            {
                plankFence.SetActive(true);
                currentFence = plankFence;
            }
            else
            {
                ropeFence.SetActive(true);
                currentFence = ropeFence;
            }
        }
        else if (currentFence != null)
        {
            currentFence.SetActive(state);
            currentFence = null;
        }
    }

    // Determines, whether the pillar is supposed to be small or big
    public void SetPillarType(bool leftIsSmall, bool rightIsSmall)
    {
        this.leftIsSmall = leftIsSmall;
        this.rightIsSmall = rightIsSmall;

        if (leftIsSmall)
        {
            SetState(true, true);
        }
        else
        {
            SetState(true, false);
        }

        if (rightIsSmall) 
        { 
            SetState(false, true);
        }
        else 
        { 
            SetState(false, false);
        }
    }

    // When two neighbouring fences are instantiated, pillars overlap.
    // To avoid unnecessary active gameobjects in the scene, we deactivate the neihbouring left pillar, of the newly instantiated one.
    public void DeactivateLeftPillar()
    {
        bigLeftPillar.SetActive(false);
        smallLeftPillar.SetActive(false);
    }

    public void DeactivateRightPillar()
    {
        bigRightPillar.SetActive(false);
        smallRightPillar.SetActive(false);
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

    private void CheckState()
    {
        // Check pillar state
        if (bigLeftPillar.activeSelf) { leftIsSmall = false; }
        else { leftIsSmall = true; }

        if (bigRightPillar.activeSelf) { rightIsSmall = false; }
        else { rightIsSmall = true; }

        // Check fence state
        if (plankFence.activeSelf) { currentFence = plankFence; }
        else if (ropeFence.activeSelf) { currentFence = ropeFence; }
        else { currentFence = null; }
    }
}
